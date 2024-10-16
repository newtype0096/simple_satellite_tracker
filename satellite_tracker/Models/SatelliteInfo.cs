using CelesTrakLib.Datas;
using SatelliteTrackerLib;

namespace satellite_tracker.Models
{
    public class SatelliteInfo
    {
        public SatelliteCatalogData CatalogData { get; set; }
        public TrackingData TrackingData { get; set; }
    }
}