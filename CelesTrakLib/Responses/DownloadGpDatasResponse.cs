using CelesTrakLib.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Responses
{
    public class DownloadGpDatasResponse : CelesTrackResponse
    {
        public List<GpData> GpDatas { get; set; }
    }
}
