namespace CelesTrakLib.Data
{
    public class Satellite
    {
        public string object_name { get; set; }
        public string object_id { get; set; }
        public string epoch { get; set; }
        public double mean_motion { get; set; }
        public double eccentricity { get; set; }
        public double inclination { get; set; }
        public double ra_of_asc_node { get; set; }
        public double arg_of_pericenter { get; set; }
        public double mean_anomaly { get; set; }
        public int ephemeris_type { get; set; }
        public string classification_type { get; set; }
        public int norad_cat_id { get; set; }
        public int element_set_no { get; set; }
        public int rev_at_epoch { get; set; }
        public double bstar { get; set; }
        public double mean_motion_dot { get; set; }
        public double mean_motion_ddot { get; set; }
    }
}