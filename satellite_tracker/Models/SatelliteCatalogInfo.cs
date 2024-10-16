using CelesTrakLib.Datas;

namespace satellite_tracker.Models
{
    public class SatelliteCatalogInfo
    {
        public SatelliteCatalogData Data { get; set; }

        public bool IsTracking { get; set; }
    }
}