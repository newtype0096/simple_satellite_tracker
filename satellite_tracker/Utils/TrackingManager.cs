using CelesTrakLib.Datas;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace satellite_tracker.Utils
{
    public class TrackingManager
    {
        private string _trackingListFileName;

        private bool _trackingThreadExit = false;
        private Thread _trackingThread;

        private Dictionary<string, OrbitalData> _targets = new Dictionary<string, OrbitalData>();
        public IReadOnlyDictionary<string, OrbitalData> Targets => _targets;

        public TrackingManager()
        {
            _trackingListFileName = Path.Combine(GlobalData.Default.CurrentDirectory, "tracking.txt");
            LoadTrackingList();
        }

        public void Start()
        {
            _trackingThread = new Thread(new ParameterizedThreadStart(TrackingThreadProc));
            _trackingThread.Start(this);
        }

        public void Stop()
        {
            _trackingThreadExit = true;
            _trackingThread.Join();
        }

        public void AddTrackingTarget(string id)
        {
            if (_targets.ContainsKey(id))
            {
                _targets.Add(id, new OrbitalData() { NORAD_CAT_ID = id });

                RewriteTrackingList();
            }
        }

        public void RemoveTrackingTarget(string id)
        {
            if (_targets.Remove(id))
            {
                RewriteTrackingList();
            }
        }

        public bool IsTrackingTarget(string id)
        {
            return _targets.ContainsKey(id);
        }

        private void LoadTrackingList()
        {
            if (File.Exists(_trackingListFileName))
            {
                using (var reader = new StreamReader(_trackingListFileName))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        _targets.Add(line, new OrbitalData() { NORAD_CAT_ID = line });

                        line = reader.ReadLine();
                    }
                }
            }
        }

        private void RewriteTrackingList()
        {
            using (var writer = new StreamWriter(_trackingListFileName))
            {
                foreach (var data in _targets)
                {
                    writer.WriteLine(data);
                }
            }
        }

        private static void TrackingThreadProc(object param)
        {
            var obj = (TrackingManager)param;

            while (obj._trackingThreadExit)
            {
                Thread.Sleep(100);
            }
        }
    }
}