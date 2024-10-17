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

    public class SatelliteTracker
    {
        private readonly object _cs = new object();

        private bool _threadExit = false;
        private Thread _trackingThread;

        private readonly Dictionary<string, TrackingData> _targets = new Dictionary<string, TrackingData>();

        public UpdateTrackingData UpdateTrackingDataCallback { get; set; }

        public void Start()
        {
            _trackingThread = new Thread(new ParameterizedThreadStart(TrackingThreadProc));
            _trackingThread.Start(this);
        }

        public void Stop()
        {
            _threadExit = true;
            _trackingThread.Join();
        }

        public void AddTrackingTarget(string norad_cat_id)
        {
            lock (_cs)
            {
                if (!_targets.ContainsKey(norad_cat_id))
                {
                    _targets.Add(norad_cat_id, new TrackingData());
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

        private static void TrackingThreadProc(object param)
        {
            var obj = (SatelliteTracker)param;

            while (!obj._threadExit)
            {
                lock (obj._cs)
                {
                    foreach (var target in obj._targets)
                    {
                        var apiTryTimeSpan = DateTime.Now - target.Value.LastApiTry;
                        var apiUpdateTimeSpan = DateTime.Now - target.Value.LastApiUpdate;
                        if (apiUpdateTimeSpan.TotalHours >= 2 && apiTryTimeSpan.TotalSeconds >= 30)
                        {
                            target.Value.LastApiTry = DateTime.Now;

                            Task.Run(() =>
                                {
                                    if (CelesTrak.Default.GetOribitalData(target.Key, out var orbitalDataResponse))
                                    {
                                        target.Value.OrbitalData = orbitalDataResponse.Data;
                                    }

                                    if (CelesTrak.Default.GetTleData(target.Key, out var tleDataResponse))
                                    {
                                        target.Value.TleData = tleDataResponse.Data;
                                    }

                                    if (target.Value.OrbitalData != null && target.Value.TleData != null)
                                    {
                                        target.Value.LastApiUpdate = DateTime.Now;

                                        obj.UpdateTrackingDataCallback?.Invoke(target.Key, target.Value);                                        
                                    }
                                }
                            );
                        }

                        if (target.Value.OrbitalData != null && target.Value.TleData != null)
                        {
                            var positionTimeSpan = DateTime.Now - target.Value.LastPositionUpdate;
                            if (positionTimeSpan.TotalSeconds >= 5 && target.Value.LastApiUpdate != DateTime.MinValue)
                            {
                                Task.Run(() =>
                                    {
                                        var tleItem = ParserTLE.parseTle(target.Value.TleData.Line1, target.Value.TleData.Line2, target.Value.OrbitalData.OBJECT_NAME);
                                        target.Value.PositionData = SatFunctions.getSatPositionAtTime(tleItem, new EpochTime(DateTime.UtcNow), Sgp4.wgsConstant.WGS_84);
                                        target.Value.LastPositionUpdate = DateTime.Now;

                                        obj.UpdateTrackingDataCallback?.Invoke(target.Key, target.Value);                                        
                                    }
                                );
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
