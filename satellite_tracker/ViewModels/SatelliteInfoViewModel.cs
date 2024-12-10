using satellite_tracker.Models;
using System.Collections.Generic;

namespace satellite_tracker.ViewModels
{
    public class SatelliteInfoViewModel : PaneViewModel
    {
        public static SatelliteInfoViewModel Default { get; } = new SatelliteInfoViewModel();

        public List<PropertyItem> SatelliteInfos { get; } = new List<PropertyItem>();

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
            : base("Satellite Info")
        {
            SatelliteInfos.Add(new PropertyItem() { Name = "Name" });
            SatelliteInfos.Add(new PropertyItem() { Name = "NORAD ID" });
            SatelliteInfos.Add(new PropertyItem() { Name = "International Designator" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Perigee" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Apogee" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Inclination" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Period" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Launch date" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Source" });
            SatelliteInfos.Add(new PropertyItem() { Name = "Launch site" });
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

        private PropertyItem FindInfo(string name)
        {
            return SatelliteInfos.Find(x => x.Name == name);
        }
    }
}