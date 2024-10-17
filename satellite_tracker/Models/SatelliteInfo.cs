using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using SatelliteTrackerLib;

namespace satellite_tracker.Models
{
    public class SatelliteInfo : ObservableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
        }

        private string _norad_cat_id;
        public string Norad_Cat_Id
        {
            get => _norad_cat_id;
            private set => SetProperty(ref _norad_cat_id, value);
        }

        private string _epoch;
        public string Epoch
        {
            get => _epoch;
            private set => SetProperty(ref _epoch, value);
        }

        private string _mean_motion;
        public string Mean_Motion
        {
            get => _mean_motion;
            private set => SetProperty(ref _mean_motion, value);
        }

        private string _eccentricity;
        public string Eccentricity
        {
            get => _eccentricity;
            private set => SetProperty(ref _eccentricity, value);
        }

        private string _inclination;
        public string Inclination
        {
            get => _inclination;
            private set => SetProperty(ref _inclination, value);
        }

        private string _ra_of_asc_node;
        public string Ra_of_Asc_Node
        {
            get => _ra_of_asc_node;
            private set => SetProperty(ref _ra_of_asc_node, value);
        }

        private string _arg_of_pericenter;
        public string Arg_of_Pericenter
        {
            get => _arg_of_pericenter;
            private set => SetProperty(ref _arg_of_pericenter, value);
        }

        private string _mean_anomaly;
        public string Mean_Anomaly
        {
            get => _mean_anomaly;
            private set => SetProperty(ref _mean_anomaly, value);
        }

        private string _element_set_no;
        public string Element_Set_No
        {
            get => _element_set_no;
            private set => SetProperty(ref _element_set_no, value);
        }

        private string _rev_at_epoch;
        public string Rev_at_Epoch
        {
            get => _rev_at_epoch;
            private set => SetProperty(ref _rev_at_epoch, value);
        }

        private string _bstar;
        public string Bstar
        {
            get => _bstar;
            private set => SetProperty(ref _bstar, value);
        }

        private string _mean_motion_dot;
        public string Mean_Motion_Dot
        {
            get => _mean_motion_dot;
            private set => SetProperty(ref _mean_motion_dot, value);
        }

        private string _mean_motion_ddot;
        public string Mean_Motion_Ddot
        {
            get => _mean_motion_ddot;
            private set => SetProperty(ref _mean_motion_ddot, value);
        }

        private SatelliteCatalogData _catalogData;
        public SatelliteCatalogData CatalogData
        {
            get => _catalogData;
            set
            {
                Name = value.OBJECT_NAME;
                Norad_Cat_Id = value.NORAD_CAT_ID;

                SetProperty(ref _catalogData, value);
            }
        }

        private TrackingData _trackingData;
        public TrackingData TrackingData
        {
            get => _trackingData;
            set
            {
                Epoch = value.OrbitalData.EPOCH;
                Mean_Motion = value.OrbitalData.MEAN_MOTION;
                Eccentricity = value.OrbitalData.ECCENTRICITY;
                Inclination = value.OrbitalData.INCLINATION;
                Ra_of_Asc_Node = value.OrbitalData.RA_OF_ASC_NODE;
                Arg_of_Pericenter = value.OrbitalData.ARG_OF_PERICENTER;
                Mean_Anomaly = value.OrbitalData.MEAN_ANOMALY;
                Element_Set_No = value.OrbitalData.ELEMENT_SET_NO;
                Rev_at_Epoch = value.OrbitalData.REV_AT_EPOCH;
                Bstar = value.OrbitalData.BSTAR;
                Mean_Motion_Dot = value.OrbitalData.MEAN_MOTION_DOT;
                Mean_Motion_Ddot = value.OrbitalData.MEAN_MOTION_DDOT;

                SetProperty(ref _trackingData, value);
            }
        }
    }
}