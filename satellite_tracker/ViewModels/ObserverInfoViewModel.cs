using satellite_tracker.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;

namespace satellite_tracker.ViewModels
{
    public class ObserverInfoViewModel : PaneViewModel
    {
        public static ObserverInfoViewModel Default { get; } = new ObserverInfoViewModel();

        private List<PropertyItem> _observerInfos = new List<PropertyItem>();
        public List<PropertyItem> ObserverInfos
        {
            get => _observerInfos;
            set => SetProperty(ref _observerInfos, value);
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public ObserverInfoViewModel()
            : base("Observer Info")
        {
            ObserverInfos.Add(new PropertyItem() { Name = "Latitude" });
            ObserverInfos.Add(new PropertyItem() { Name = "Longitude" });
            ObserverInfos.Add(new PropertyItem() { Name = "Local Time zone" });

            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.StatusChanged += (s, e) =>
            {
                if (e.Status == GeoPositionStatus.Ready)
                {
                    var coord = watcher.Position.Location;
                    if (!coord.IsUnknown)
                    {
                        FindInfo("Latitude").Data = Latitude = GlobalData.Default.CelesTrak.ObserverLatitude = coord.Latitude;
                        FindInfo("Longitude").Data = Longitude = GlobalData.Default.CelesTrak.ObserverLongitude = coord.Longitude;
                        FindInfo("Local Time zone").Data = TimeZoneInfo.Local.DisplayName;
                    }
                }
            };
            watcher.Start();
        }

        private PropertyItem FindInfo(string name)
        {
            return ObserverInfos.Find(x => x.Name == name);
        }
    }
}