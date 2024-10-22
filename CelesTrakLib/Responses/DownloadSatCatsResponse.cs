using CelesTrakLib.Datas;
using System.Collections.Generic;

namespace CelesTrakLib.Responses
{
    public class DownloadSatCatsResponse : CelesTrackResponse
    {
        public List<SatCat> SatCats { get; set; }
    }
}