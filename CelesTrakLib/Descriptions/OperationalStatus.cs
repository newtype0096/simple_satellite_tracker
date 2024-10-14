using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Descriptions
{
    public static class OperationalStatus
    {
        public static Dictionary<string, string> Datas = new Dictionary<string, string>()
        {
            { "+", "Operational" },
            { "-", "Nonoperational" },
            { "P", "Partially Operational [Partially fulfilling primary mission or secondary mission(s)]" },
            { "B", "Backup/Standby [Previously operational satellite put into reserve status]" },
            { "S", "Spare [New satellite awaiting full activation]" },
            { "X", "Extended Mission" },
            { "D", "Decayed" },
            { "?", "Unknown" }
        };
    }
}
