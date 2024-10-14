using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Datas
{
    public class SatelliteCatalog
    {
        public SatelliteCatalogData Data { get; set; }
        public string SourceDescription { get; set; }
        public string LaunchSiteDescription { get; set; }
        public string OpsStatusDescription { get; set; }
    }
}
