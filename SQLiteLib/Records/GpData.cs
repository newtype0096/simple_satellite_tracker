using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteLib.Records
{
    public class GpData
    {
        public string OBJECT_NAME { get; set; }
        public string OBJECT_ID { get; set; }
        public string EPOCH { get; set; }
        public string MEAN_MOTION { get; set; }
        public string ECCENTRICITY { get; set; }
        public string INCLINATION { get; set; }
        public string RA_OF_ASC_NODE { get; set; }
        public string ARG_OF_PERICENTER { get; set; }
        public string MEAN_ANOMALY { get; set; }
        public string EPHEMERIS_TYPE { get; set; }
        public string CLASSIFICATION_TYPE { get; set; }
        public string NORAD_CAT_ID { get; set; }
        public string ELEMENT_SET_NO { get; set; }
        public string REV_AT_EPOCH { get; set; }
        public string BSTAR { get; set; }
        public string MEAN_MOTION_DOT { get; set; }
        public string MEAN_MOTION_DDOT { get; set; }
        public string LINE1{ get; set; }
        public string LINE2 { get; set; }
        public DateTime UPDATE_DATETIME { get; set; }
    }
}
