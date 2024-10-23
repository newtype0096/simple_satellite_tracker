using One_Sgp4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Datas
{
    public class TrackingInfo
    {
        public SatCat SatCatItem { get; set; }

        public DateTime LastGpDataUpdateTime { get; set; }
        public GpData GpDataItem { get; set; }
        public Tle TleItem { get; set; }

        public DateTime LastPositionUpdateTime { get; set; }
        public Sgp4Data Sgp4DataItem { get; set; }
        public Coordinate CoordinateItem { get; set; }

        public DateTime LastCoordinatesUpdateTime { get; set; }
        public List<Coordinate> Coordinates { get; } = new List<Coordinate>();
    }
}
