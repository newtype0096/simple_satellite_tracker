using CelesTrakLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Response
{
    public class GetSatelliteCatalogsResponse : CelesTrackResponse
    {
        public List<SatelliteCatalog> SatelliteCatalogs { get; set; }
    }
}
