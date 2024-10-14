using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Datas
{
    public class OrbitalData
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

        public void Copy(OrbitalData data)
        {
            OBJECT_NAME = data.OBJECT_NAME;
            OBJECT_ID = data.OBJECT_ID;
            EPOCH = data.EPOCH;
            MEAN_MOTION = data.MEAN_MOTION;
            ECCENTRICITY = data.ECCENTRICITY;
            INCLINATION = data.INCLINATION;
            RA_OF_ASC_NODE = data.RA_OF_ASC_NODE;
            ARG_OF_PERICENTER = data.ARG_OF_PERICENTER;
            MEAN_ANOMALY = data.MEAN_ANOMALY;
            EPHEMERIS_TYPE = data.EPHEMERIS_TYPE;
            CLASSIFICATION_TYPE = data.CLASSIFICATION_TYPE;
            NORAD_CAT_ID = data.NORAD_CAT_ID;
            ELEMENT_SET_NO = data.ELEMENT_SET_NO;
            REV_AT_EPOCH = data.REV_AT_EPOCH;
            BSTAR = data.BSTAR;
            MEAN_MOTION_DOT = data.MEAN_MOTION_DOT;
            MEAN_MOTION_DDOT = data.MEAN_MOTION_DDOT;
        }
    }
}
