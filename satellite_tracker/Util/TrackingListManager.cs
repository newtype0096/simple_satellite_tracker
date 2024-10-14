using System.Collections.Generic;
using System.IO;

namespace satellite_tracker.Util
{
    public class TrackingListManager
    {
        private string _trackingListFileName;

        private HashSet<string> _datas { get; } = new HashSet<string>();
        public IReadOnlyCollection<string> Datas => _datas;

        public TrackingListManager()
        {
            _trackingListFileName = Path.Combine(GlobalData.Default.CurrentDirectory, "tracking.txt");

            LoadTrackingList();
        }

        public void AddTrackingTarget(string id)
        {
            if (_datas.Add(id))
            {
                RewriteTrackingList();
            }
        }

        public void RemoveTrackingTarget(string id)
        {
            if (_datas.Remove(id))
            {
                RewriteTrackingList();
            }
        }

        public bool IsTrackingTarget(string id)
        {
            return _datas.Contains(id);
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
                        _datas.Add(line);

                        line = reader.ReadLine();
                    }
                }
            }
        }

        private void RewriteTrackingList()
        {
            using (var writer = new StreamWriter(_trackingListFileName))
            {
                foreach (var data in _datas)
                {
                    writer.WriteLine(data);
                }
            }
        }
    }
}