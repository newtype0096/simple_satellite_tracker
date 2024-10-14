using CelesTrakLib.Datas;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace satellite_tracker.Utils
{
    public class TrackingManager
    {
        private string _trackingListFileName;

        private Dictionary<string ,OrbitalData> _targets = new Dictionary<string, OrbitalData>();
        public IReadOnlyDictionary<string, OrbitalData> Targets => _targets;

        public TrackingManager()
        {
            _trackingListFileName = Path.Combine(GlobalData.Default.CurrentDirectory, "tracking.txt");

            LoadTrackingList();
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
    }
}