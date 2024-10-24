﻿using CommunityToolkit.Mvvm.ComponentModel;
using satellite_tracker.Models;
using System.Collections.Generic;

namespace satellite_tracker.ViewModels
{
    public class SatelliteInfoViewModel : ObservableObject
    {
        public static SatelliteInfoViewModel Default { get; } = new SatelliteInfoViewModel();

        private List<SatelliteInfo> _satelliteInfos = new List<SatelliteInfo>();
        public List<SatelliteInfo> SatelliteInfos
        {
            get => _satelliteInfos;
            set => SetProperty(ref _satelliteInfos, value);
        }

        private Satellite _selectedSat;
        public Satellite SelectedSat
        {
            get => _selectedSat;
            set
            {
                SetProperty(ref _selectedSat, value);

                UpdateInfo();
            }
        }

        public SatelliteInfoViewModel()
        {
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Name" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "NORAD ID" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "International Designator" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Perigee" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Apogee" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Inclination" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Period" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Launch date" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Source" });
            SatelliteInfos.Add(new SatelliteInfo() { Name = "Launch site" });
        }

        private void UpdateInfo()
        {
            if (SelectedSat == null)
            {
                foreach (var info in SatelliteInfos)
                {
                    info.Data = null;
                }
                return;
            }

            FindInfo("Name").Data = SelectedSat.SatCatItem.OBJECT_NAME;
            FindInfo("NORAD ID").Data = SelectedSat.SatCatItem.NORAD_CAT_ID;
            FindInfo("International Designator").Data = SelectedSat.SatCatItem.OBJECT_ID;
            FindInfo("Perigee").Data = $"{SelectedSat.SatCatItem.PERIGEE} km";
            FindInfo("Apogee").Data = $"{SelectedSat.SatCatItem.APOGEE} km";
            FindInfo("Inclination").Data = $"{SelectedSat.SatCatItem.INCLINATION} °";
            FindInfo("Period").Data = $"{SelectedSat.SatCatItem.PERIOD} minutes";
            FindInfo("Launch date").Data = SelectedSat.SatCatItem.LAUNCH_DATE;
            FindInfo("Source").Data = CelesTrakLib.Dictionaries.Sources.GetSourceFull(SelectedSat.SatCatItem.OWNER);
            FindInfo("Launch site").Data = CelesTrakLib.Dictionaries.LaunchSites.GetLaunchSiteFull(SelectedSat.SatCatItem.LAUNCH_SITE);
        }

        private SatelliteInfo FindInfo(string name)
        {
            return SatelliteInfos.Find(x => x.Name == name);
        }
    }
}