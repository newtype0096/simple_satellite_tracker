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
            set => SetProperty(ref _trackingData, value);
        }
    }
}