using CelesTrakLib.Data;
using System.Collections.Generic;

namespace CelesTrakLib.Response
{
    public class GetStationsResponse : CelesTrackResponse
    {
        public List<Satellite> Stations { get; set; }
    }
}