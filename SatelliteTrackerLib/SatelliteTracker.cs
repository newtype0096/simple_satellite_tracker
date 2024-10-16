using CelesTrakLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SatelliteTrackerLib
{
    public class SatelliteTracker
    {
        private readonly object _cs = new object();

        private bool _threadExit = false;
        private Thread _trackingThread;

        private readonly Dictionary<string, TrackingData> _targets = new Dictionary<string, TrackingData>();

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
                        var timeSpan = DateTime.Now - target.Value.LastUpdate;
                        if (timeSpan.TotalHours >= 2)
                        {
                            if (CelesTrak.Default.GetOribitalData(target.Key, out var orbitalDataResponse))
                            {
                                target.Value.OrbitalData = orbitalDataResponse.Data;
                            }

                            if (CelesTrak.Default.GetTleData(target.Key, out var tleDataResponse))
                            {
                                target.Value.TleData = tleDataResponse.Data;
                            }

                            target.Value.LastUpdate = DateTime.Now;
                            break;
                        }
                        else if (timeSpan.TotalSeconds >= 1)
                        {

                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
