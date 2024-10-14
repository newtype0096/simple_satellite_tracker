using CelesTrakLib.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.Models
{
    public class SatelliteCatalogInfo
    {
        public SatelliteCatalog Data { get; set; }

        public bool IsTracking { get; set; }
    }
}
