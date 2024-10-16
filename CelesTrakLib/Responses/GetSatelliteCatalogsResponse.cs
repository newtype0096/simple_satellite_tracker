using CelesTrakLib.Datas;
using System.Collections.Generic;

namespace CelesTrakLib.Responses
{
    public class GetSatelliteCatalogsResponse : CelesTrackResponse
    {
        public List<SatelliteCatalogData> SatelliteCatalogs { get; set; }
    }
}