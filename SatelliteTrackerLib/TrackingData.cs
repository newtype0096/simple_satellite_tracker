using CelesTrakLib.Datas;
using One_Sgp4;
using System;

namespace SatelliteTrackerLib
{
    public class TrackingData
    {
        public DateTime LastApiTry { get; set; }
        public DateTime LastApiUpdate { get; set; }
        public OrbitalData OrbitalData { get; set; }
        public TleData TleData { get; set; }

        public DateTime LastPositionUpdate { get; set; }
        public Sgp4Data PositionData { get; set; }
    }
}