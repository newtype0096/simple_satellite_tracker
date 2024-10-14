using CelesTrakLib.Datas;
using System.Collections.Generic;

namespace CelesTrakLib.Responses
{
    public class GetSatelliteCatalogsResponse : CelesTrackResponse
    {
        public List<SatelliteCatalog> SatelliteCatalogs { get; } = new List<SatelliteCatalog>();
    }
}