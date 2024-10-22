using CelesTrakLib;
using One_Sgp4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SatelliteTrackerLib
{
    public delegate void UpdateTrackingData(string norad_cat_id, TrackingData trackingData);
    public delegate void UpdateOrbitData(string norad_cat_id, OrbitData orbitData);

    public class SatelliteTracker
    {
        private readonly object _cs = new object();

        private bool _threadExit = false;
        private Thread _trackingThread;

        private readonly Dictionary<string, SatelliteData> _targets = new Dictionary<string, SatelliteData>();

        public UpdateTrackingData UpdateTrackingDataCallback { get; set; }
        public UpdateOrbitData UpdateOrbitDataCallback { get; set; }

        public void Start()
        {
            _trackingThread = new Thread(new ParameterizedThreadStart(TrackingThread));
            _trackingThread.Start(this);
        }

        public void Stop()
        {
            _threadExit = true;
            _trackingThread.Join();
        }

        private static void TrackingThread(object param)
        {
            var obj = (SatelliteTracker)param;
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
                        var apiTryTimeSpan = DateTime.Now - target.Value.TrackingData.LastApiTry;
                        var apiUpdateTimeSpan = DateTime.Now - target.Value.TrackingData.LastApiUpdate;
                        if (apiUpdateTimeSpan.TotalHours >= 2 && apiTryTimeSpan.TotalSeconds >= 30)
                        {
                            target.Value.TrackingData.LastApiTry = DateTime.Now;

                            //Task.Run(() =>
                            //{
                            //    if (CelesTrak.Default.GetOribitalData(target.Key, out var orbitalDataResponse))
                            //    {
                            //        target.Value.TrackingData.OrbitalData = orbitalDataResponse.Data;
                            //    }

                            //    if (CelesTrak.Default.GetTleData(target.Key, out var tleDataResponse))
                            //    {
                            //        target.Value.TrackingData.TleData = tleDataResponse.Data;
                            //    }

                            //    if (target.Value.TrackingData.OrbitalData != null && target.Value.TrackingData.TleData != null)
                            //    {
                            //        target.Value.TrackingData.LastApiUpdate = DateTime.Now;
                            //        target.Value.OrbitData.LastUpdate = DateTime.MinValue;

                            //        obj.UpdateTrackingDataCallback?.Invoke(target.Key, target.Value.TrackingData);
                            //    }
                            //});
                        }

                        if (target.Value.TrackingData.OrbitalData != null && target.Value.TrackingData.TleData != null)
                        {
                            var positionTimeSpan = DateTime.Now - target.Value.TrackingData.LastPositionUpdate;
                            if (positionTimeSpan.TotalSeconds >= 5 && target.Value.TrackingData.LastApiUpdate != DateTime.MinValue)
                            {
                                //Task.Run(() =>
                                //{
                                //    var tleItem = ParserTLE.parseTle(target.Value.TrackingData.TleData.Line1, target.Value.TrackingData.TleData.Line2, target.Value.TrackingData.OrbitalData.OBJECT_NAME);
                                //    target.Value.TrackingData.PositionData = SatFunctions.getSatPositionAtTime(tleItem, new EpochTime(DateTime.UtcNow), Sgp4.wgsConstant.WGS_84);
                                //    target.Value.TrackingData.LastPositionUpdate = DateTime.Now;

                                //    obj.UpdateTrackingDataCallback?.Invoke(target.Key, target.Value.TrackingData);
                                //});
                            }

                            var simulationTimeSpan = DateTime.Now - target.Value.OrbitData.LastUpdate;
                            if (simulationTimeSpan.TotalMinutes >= 60)
                            {
                                //Task.Run(() =>
                                //{
                                //    target.Value.OrbitData.Coordinates.Clear();

                                //    var tleItem = ParserTLE.parseTle(target.Value.TrackingData.TleData.Line1, target.Value.TrackingData.TleData.Line2, target.Value.TrackingData.OrbitalData.OBJECT_NAME);

                                //    EpochTime calcTime = new EpochTime(tleItem.getEpochYear(), tleItem.getEpochDay());
                                //    EpochTime startTime = new EpochTime(calcTime);
                                //    EpochTime endTime = new EpochTime(calcTime);

                                //    calcTime.addHours(-1);
                                //    startTime.addHours(-1);
                                //    endTime.addHours(1);

                                //    var sgp4 = new Sgp4(tleItem, Sgp4.wgsConstant.WGS_84);
                                //    sgp4.runSgp4Cal(startTime, endTime, 1);

                                //    var results = sgp4.getResults();
                                //    foreach (var result in results)
                                //    {
                                //        var satOnGround = SatFunctions.calcSatSubPoint(calcTime, result, Sgp4.wgsConstant.WGS_84);
                                //        target.Value.OrbitData.Coordinates.Add(satOnGround);

                                //        calcTime.addMinutes(1);
                                //    }

                                //    target.Value.OrbitData.LastUpdate = DateTime.Now;

                                //    obj.UpdateOrbitDataCallback?.Invoke(target.Key, target.Value.OrbitData);
                                //});
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
