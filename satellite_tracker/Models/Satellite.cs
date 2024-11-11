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

        private string _azimuthText;
        public string AzimuthText
        {
            get => _azimuthText;
            set => SetProperty(ref _azimuthText, value);
        }

        private double _km;
        public double KM
        {
            get => _km;
            set => SetProperty(ref _km, value);
        }

        public double Azimuth
        {
            set
            {
                string symbol = string.Empty;

                if (value >= 0 && value < 22.5 || value >= 337.5 && value <= 360)
                {
                    symbol = "N";
                }
                else if (value >= 22.5 && value < 67.5)
                {
                    symbol = "NE";
                }
                else if (value >= 67.5 && value < 112.5)
                {
                    symbol = "E";
                }
                else if (value >= 112.5 && value < 157.5)
                {
                    symbol = "SE";
                }
                else if (value >= 157.5 && value < 202.5)
                {
                    symbol = "S";
                }
                else if (value >= 202.5 && value < 247.5)
                {
                    symbol = "SW";
                }
                else if (value >= 247.5 && value < 292.5)
                {
                    symbol = "W";
                }
                else if (value >= 292.5 && value < 337.5)
                {
                    symbol = "NW";
                }
                else
                {
                    symbol = "Invalid";
                }

                AzimuthText = $"{value:F2} ({symbol})";
            }
        }

        private double _elevation;
        public double Elevation
        {
            get => _elevation;
            set => SetProperty(ref _elevation, value);
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