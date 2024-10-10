using System.ComponentModel;

namespace CelesTrakLib.Query
{
    public enum Format
    {
        [Description("TLE/3LE")]
        tle,

        [Description("2LE")]
        __2le,

        [Description("OMM XML")]
        xml,

        [Description("OMM KVN")]
        kvn,

        [Description("JSON")]
        json,

        [Description("JSON PP")]
        json_pretty,

        [Description("CSV")]
        csv
    }
}