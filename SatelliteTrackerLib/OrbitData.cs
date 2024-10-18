using One_Sgp4;
using System;
using System.Collections.Generic;

namespace SatelliteTrackerLib
{
    public class OrbitData
    {
        public DateTime LastUpdate { get; set; }
        public List<Coordinate> Coordinates { get; } = new List<Coordinate>();
    }
}