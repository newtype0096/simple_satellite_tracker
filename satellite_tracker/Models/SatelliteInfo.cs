using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using One_Sgp4;
using System.Collections.Generic;

namespace satellite_tracker.Models
{
    public class SatelliteInfo : ObservableObject
    {
        public bool IsTracking { get; set; }

        private double _mean_motion_dot;
        public double Mean_Motion_Dot
        {
            get => _mean_motion_dot;
            set => SetProperty(ref _mean_motion_dot, value);
        }

        private double _mean_motion_ddot;
        public double Mean_Motion_Ddot
        {
            get => _mean_motion_ddot;
            set => SetProperty(ref _mean_motion_ddot, value);
        }

        private double _bstar;
        public double BSTAR
        {
            get => _bstar;
            set => SetProperty(ref _bstar, value);
        }

        private double _ephemeris_type;
        public double Ephemeris_Type
        {
            get => _ephemeris_type;
            set => SetProperty(ref _ephemeris_type, value);
        }

        private double _element_set_no;
        public double Element_Set_No
        {
            get => _element_set_no;
            set => SetProperty(ref _element_set_no, value);
        }

        private double _inclination;
        public double Inclination
        {
            get => _inclination;
            set => SetProperty(ref _inclination, value);
        }

        private double _raan;
        public double RAAN
        {
            get => _raan;
            set => SetProperty(ref _raan, value);
        }

        private double _eccentricity;
        public double Eccentricity
        {
            get => _eccentricity;
            set => SetProperty(ref _eccentricity, value);
        }

        private double _arg_of_pericenter;
        public double Arg_Of_Pericenter
        {
            get => _arg_of_pericenter;
            set => SetProperty(ref _arg_of_pericenter, value);
        }

        private double _mean_anomaly;
        public double Mean_Anomaly
        {
            get => _mean_anomaly;
            set => SetProperty(ref _mean_anomaly, value);
        }

        private double _mean_motion;
        public double Mean_Motion
        {
            get => _mean_motion;
            set => SetProperty(ref _mean_motion, value);
        }

        private SatCat _satCatItem;
        public SatCat SatCatItem
        {
            get => _satCatItem;
            set => SetProperty(ref _satCatItem, value);
        }

        private TrackingInfo _trackingInfoItem;
        public TrackingInfo TrackingInfoItem
        {
            get => _trackingInfoItem;
            set => SetProperty(ref _trackingInfoItem, value);
        }
    }
}