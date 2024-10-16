using CelesTrakLib.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteTrackerLib
{
    public class TrackingData
    {
        public OrbitalData OrbitalData { get; set; }
        public TleData TleData { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
