using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using One_Sgp4;
using System.Collections.Generic;

namespace satellite_tracker.Models
{
    public class Satellite : ObservableObject
    {
        public bool IsTracking { get; set; }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private double _altitude;
        public double Altitude
        {
            get => _altitude;
            set => SetProperty(ref _altitude, value);
        }

        private double _speed;
        public double Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        private string _rightAscension;
        public string RightAscension
        {
            get => _rightAscension;
            set => SetProperty(ref _rightAscension, value);
        }

        private string _declination;
        public string Declination
        {
            get => _declination;
            set => SetProperty(ref _declination, value);
        }

        private string _localSiderealTime;
        public string LocalSiderealTime
        {
            get => _localSiderealTime;
            set => SetProperty(ref _localSiderealTime, value);
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