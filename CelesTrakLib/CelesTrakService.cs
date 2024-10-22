﻿using CelesTrakLib.Datas;
using One_Sgp4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CelesTrakLib
{
    public delegate void UpdateGpData(string norad_cat_id, TrackingInfo trackingInfo);
    public delegate void UpdatePosition(string norad_cat_id, TrackingInfo trackingInfo);
    public delegate void UpdateCoordinates(string noard_cat_id, TrackingInfo trackingInfo);

    public class CelesTrakService
    {
        private string _workingDirectory;

        private bool _threadExit = false;
        private Thread _gpDataUpdateThread;
        private Thread _trackingThread;

        private CelesTrakApi _celesTrakApi;
        private SQLiteHelper _sqliteHelper;

        private object _cs = new object();
        private Dictionary<string, TrackingInfo> _targets = new Dictionary<string, TrackingInfo>();

        public UpdateGpData UpdateGpDataCallback { get; set; }
        public UpdatePosition UpdatePositionCallback { get; set; }
        public UpdateCoordinates UpdateCoordinatesCallback { get; set; }

        public CelesTrakService(string workingDirectory)
        {
            _workingDirectory = workingDirectory;

            _celesTrakApi = new CelesTrakApi();

            _sqliteHelper = new SQLiteHelper(Path.Combine(_workingDirectory, "celestrakdb.sqlite"));
            _sqliteHelper.Init();
        }

        public void Start()
        {
            _gpDataUpdateThread = new Thread(new ParameterizedThreadStart(GpDataUpdateThread));
            _gpDataUpdateThread.Start(this);

            _trackingThread = new Thread(new ParameterizedThreadStart(TrackingThread));
            _trackingThread.Start(this);
        }

        public void Stop()
        {
            _threadExit = true;
            _trackingThread.Join();
            _gpDataUpdateThread.Join();
        }

        public void AddTrackingTarget(string norad_cat_id, SatCat satCat)
        {
            lock (_cs)
            {
                if (!_targets.ContainsKey(norad_cat_id))
                {
                    _targets.Add(norad_cat_id, new TrackingInfo() { SatCatItem = satCat });
                }
            }
        }

        public void RemoveTrackingTarget(string norad_cat_id)
        {
            lock (_cs)
            {
                if (_targets.ContainsKey(norad_cat_id))
                {
                    _targets.Remove(norad_cat_id);
                }
            }
        }

        public bool IsTrackingTarget(string norad_cat_id)
        {
            lock (_cs)
            {
                return _targets.ContainsKey(norad_cat_id);
            }
        }

        private static void GpDataUpdateThread(object param)
        {
            var obj = (CelesTrakService)param;
            obj.GpDataUpdateThreadProc();
        }

        private void GpDataUpdateThreadProc()
        {
            DateTime lastGpDataUpdate = DateTime.MinValue;

            if (_sqliteHelper.SelectLastUpdate("gp_data", out var lastUpdate) && lastUpdate != null)
            {
                lastGpDataUpdate = lastUpdate.DATETIME;
            }

            while (!_threadExit)
            {
                var timeSpan = DateTime.Now - lastGpDataUpdate;
                if (timeSpan.TotalHours >= 1 || DateTime.Now.Hour != lastGpDataUpdate.Hour)
                {
                    if (_celesTrakApi.DownloadGpDatas(out var response))
                    {
                        lastGpDataUpdate = DateTime.Now;
                        _sqliteHelper.UpsertGpDatas(response.GpDatas);
                        _sqliteHelper.UpsertLastUpdate(new LastUpdate() { CATEGORY = "gp_data", DATETIME = lastGpDataUpdate });
                    }
                    else
                    {
                        if (response.ErrorCode == 403)
                        {
                            lastGpDataUpdate = DateTime.Now;
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        private static void TrackingThread(object param)
        {
            var obj = (CelesTrakService)param;
            obj.TrackingThreadProc();
        }

        private void TrackingThreadProc()
        {
            while (!_threadExit)
            {
                lock (_cs)
                {
                    foreach (var target in _targets)
                    {
                        if (_threadExit)
                        {
                            break;
                        }

                        var gpDataTimeSpan = DateTime.Now - target.Value.LastGpDataUpdateTime;
                        if (gpDataTimeSpan.TotalMinutes >= 1)
                        {
                            if (GetGpData(target.Key, out var gpData) && gpData != null)
                            {
                                target.Value.GpDataItem = gpData;
                                target.Value.TleItem = ParserTLE.parseTle(gpData.LINE1, gpData.LINE2, gpData.OBJECT_NAME);
                                target.Value.LastGpDataUpdateTime = DateTime.Now;

                                UpdateGpDataCallback?.Invoke(target.Key, target.Value);
                            }
                        }

                        var positionTimeSpan = DateTime.Now - target.Value.LastPositionUpdateTime;
                        if (positionTimeSpan.TotalSeconds >= 5 && target.Value.TleItem != null)
                        {
                            target.Value.Sgp4DataItem = SatFunctions.getSatPositionAtTime(target.Value.TleItem, new EpochTime(DateTime.UtcNow), Sgp4.wgsConstant.WGS_84);
                            target.Value.LastPositionUpdateTime = DateTime.Now;

                            UpdatePositionCallback?.Invoke(target.Key, target.Value);
                        }

                        var coordinatesTimeSpan = DateTime.Now - target.Value.LastCoordinatesUpdateTime;
                        if (coordinatesTimeSpan.TotalMinutes >= 60 && target.Value.TleItem != null)
                        {
                            EpochTime startTime = new EpochTime(DateTime.UtcNow.AddHours(-1.5));
                            EpochTime stopTime = new EpochTime(DateTime.UtcNow.AddHours(1.5));

                            var sgp4Propagator = new Sgp4(target.Value.TleItem, Sgp4.wgsConstant.WGS_84);
                            sgp4Propagator.runSgp4Cal(startTime, stopTime, 1 / 3.0);

                            target.Value.Coordinates.Clear();

                            var results = sgp4Propagator.getResults();
                            foreach (var result in results)
                            {
                                var coordinate = SatFunctions.calcSatSubPoint(startTime, result, Sgp4.wgsConstant.WGS_84);
                                target.Value.Coordinates.Add(coordinate);
                            }

                            target.Value.LastCoordinatesUpdateTime = DateTime.Now;

                            UpdateCoordinatesCallback?.Invoke(target.Key, target.Value);
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        public bool GetSatCats(out List<SatCat> satCats)
        {
            bool needDownload = true;

            if (_sqliteHelper.SelectLastUpdate("satcat", out var lastUpdate) && lastUpdate != null)
            {
                var timeSpan = DateTime.Now - lastUpdate.DATETIME;
                if (timeSpan.TotalDays < 1)
                {
                    needDownload = false;
                }
            }

            if (needDownload)
            {
                if (_celesTrakApi.DownloadSatCats(out var response))
                {
                    _sqliteHelper.UpsertSatCats(response.SatCats);
                    _sqliteHelper.UpsertLastUpdate(new LastUpdate() { CATEGORY = "satcat", DATETIME = DateTime.Now });
                }
            }

            return _sqliteHelper.SelectSatCats(out satCats);
        }

        public bool GetGpData(string norad_cat_id, out GpData gpData)
        {
            return _sqliteHelper.SelectGpData(norad_cat_id, out gpData);
        }
    }
}