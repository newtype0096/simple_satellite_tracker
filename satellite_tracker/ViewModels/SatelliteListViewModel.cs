using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using satellite_tracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace satellite_tracker.ViewModels
{
    public class SatelliteListViewModel : ObservableObject
    {
        public static SatelliteListViewModel Default { get; } = new SatelliteListViewModel();

        private string _trackingTargetListFileName;

        private ObservableCollection<SatelliteInfo> _satelliteInfos = new ObservableCollection<SatelliteInfo>();
        public ObservableCollection<SatelliteInfo> SatelliteInfos
        {
            get => _satelliteInfos;
            set => SetProperty(ref _satelliteInfos, value);
        }

        private SatelliteInfo _selectedInfo;
        public SatelliteInfo SelectedInfo
        {
            get => _selectedInfo;
            set
            {
                //PositionDataViewModel.Default.SelectedInfo = value;
                OrbitViewModel.Default.SelectedInfo = value;

                SetProperty(ref _selectedInfo, value);
            }
        }

        public RelayCommand SettingsCommand { get; }
        public RelayCommand<SatelliteInfo> RemoveTrackingTargetCommand { get; }

        public SatelliteListViewModel()
        {
            _trackingTargetListFileName = Path.Combine(GlobalData.Default.DataDirectory, "tracking_targets.txt");

            SettingsCommand = new RelayCommand(OnSettings);
            RemoveTrackingTargetCommand = new RelayCommand<SatelliteInfo>(OnRemoveTrackingTargetCommand);

            GlobalData.Default.CelesTrak.UpdateGpDataCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var info = SatelliteInfos.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (info != null)
                    {
                        info.TrackingInfoItem = trackingInfo;
                    }
                };

            GlobalData.Default.CelesTrak.UpdatePositionCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var info = SatelliteInfos.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (info != null)
                    {
                        info.Latitude = trackingInfo.Latitude;
                        info.Longitude = trackingInfo.Longitude;
                        info.Altitude = trackingInfo.Altitude;
                        info.Speed = trackingInfo.Speed;
                        info.RightAscension = trackingInfo.RightAscension;
                        info.Declination = trackingInfo.Declination;

                        info.TrackingInfoItem = trackingInfo;

                        if (OrbitViewModel.Default.SelectedInfo?.SatCatItem.NORAD_CAT_ID == norad_cat_id)
                        {
                            OrbitViewModel.Default.UpdateGp();
                        }
                    }
                };

            GlobalData.Default.CelesTrak.UpdateCoordinatesCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var info = SatelliteInfos.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (info != null)
                    {
                        info.TrackingInfoItem = trackingInfo;

                        if (OrbitViewModel.Default.SelectedInfo?.SatCatItem.NORAD_CAT_ID == norad_cat_id)
                        {
                            OrbitViewModel.Default.UpdateOrbit();
                        }
                    }
                };

            LoadTrackingTargetList();
        }

        private void OnSettings()
        {
            var vm = new SatelliteSearchWindowViewModel();
            if (GlobalData.Default.DialogService.ShowDialog(this, vm) == true)
            {
                foreach (var info in vm.TargetSatelliteInfos)
                {
                    var infos = SatelliteInfos.Where(x => x.SatCatItem.NORAD_CAT_ID == info.SatCatItem.NORAD_CAT_ID);
                    if (!infos.Any())
                    {
                        SatelliteInfos.Add(info);
                        GlobalData.Default.CelesTrak.AddTrackingTarget(info.SatCatItem.NORAD_CAT_ID, info.SatCatItem);
                    }
                }

                WriteTrackingTargetList();
            }
        }

        private void OnRemoveTrackingTargetCommand(SatelliteInfo selectedInfo)
        {
            if (selectedInfo == null)
            {
                return;
            }

            if (SatelliteInfos.Remove(selectedInfo))
            {
                GlobalData.Default.CelesTrak.RemoveTrackingTarget(selectedInfo.SatCatItem.NORAD_CAT_ID);

                WriteTrackingTargetList();
            }
        }

        private void WriteTrackingTargetList()
        {
            var catalogDatas = SatelliteInfos.Select(x => x.SatCatItem);
            string jsonString = JsonConvert.SerializeObject(catalogDatas, Formatting.Indented);

            File.WriteAllText(_trackingTargetListFileName, jsonString);
        }

        private void LoadTrackingTargetList()
        {
            if (File.Exists(_trackingTargetListFileName))
            {
                string jsonString = File.ReadAllText(_trackingTargetListFileName);
                var satCats = JsonConvert.DeserializeObject<ObservableCollection<SatCat>>(jsonString);
                if (satCats != null)
                {
                    foreach (var satCat in satCats)
                    {
                        GlobalData.Default.CelesTrak.AddTrackingTarget(satCat.NORAD_CAT_ID, satCat);

                        SatelliteInfos.Add(new SatelliteInfo() { SatCatItem = satCat });
                    }
                }
            }
        }
    }
}