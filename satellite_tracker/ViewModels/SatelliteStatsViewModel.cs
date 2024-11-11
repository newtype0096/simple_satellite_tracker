using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using satellite_tracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace satellite_tracker.ViewModels
{
    public class SatelliteStatsViewModel : PaneViewModel
    {
        public static SatelliteStatsViewModel Default { get; } = new SatelliteStatsViewModel();

        private string _trackingTargetListFileName;

        private ObservableCollection<Satellite> _satellites = new ObservableCollection<Satellite>();
        public ObservableCollection<Satellite> Satellites
        {
            get => _satellites;
            set => SetProperty(ref _satellites, value);
        }

        private Satellite _selectedSat;
        public Satellite SelectedSat
        {
            get => _selectedSat;
            set
            {
                OrbitViewModel.Default.SelectedSat = value;
                Orbit3dViewModel.Default.SelectedSat = value;
                SatelliteInfoViewModel.Default.SelectedSat = value;

                SetProperty(ref _selectedSat, value);
            }
        }

        public RelayCommand SettingsCommand { get; }
        public RelayCommand<Satellite> RemoveTrackingTargetCommand { get; }

        public SatelliteStatsViewModel()
            : base("Satellite Stats")
        {
            _trackingTargetListFileName = Path.Combine(GlobalData.Default.DataDirectory, "tracking_targets.txt");

            SettingsCommand = new RelayCommand(OnSettings);
            RemoveTrackingTargetCommand = new RelayCommand<Satellite>(OnRemoveTrackingTargetCommand);

            GlobalData.Default.CelesTrak.UpdateGpDataCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var sat = Satellites.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (sat != null)
                    {
                        sat.TrackingInfoItem = trackingInfo;
                    }
                };

            GlobalData.Default.CelesTrak.UpdatePositionCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var sat = Satellites.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (sat != null)
                    {
                        sat.Latitude = trackingInfo.Latitude;
                        sat.Longitude = trackingInfo.Longitude;
                        sat.Altitude = trackingInfo.Altitude;
                        sat.Speed = trackingInfo.Speed;
                        sat.RightAscension = trackingInfo.RightAscension;
                        sat.Declination = trackingInfo.Declination;
                        sat.KM = trackingInfo.Km;
                        sat.Azimuth = trackingInfo.Azimuth;
                        sat.Elevation = trackingInfo.Elevation;

                        sat.TrackingInfoItem = trackingInfo;

                        if (OrbitViewModel.Default.SelectedSat?.SatCatItem.NORAD_CAT_ID == norad_cat_id)
                        {
                            OrbitViewModel.Default.UpdateIndicator();
                        }
                    }
                };

            GlobalData.Default.CelesTrak.UpdateCoordinatesCallback +=
                (string norad_cat_id, TrackingInfo trackingInfo) =>
                {
                    var sat = Satellites.FirstOrDefault(x => x.SatCatItem.NORAD_CAT_ID == norad_cat_id);
                    if (sat != null)
                    {
                        sat.TrackingInfoItem = trackingInfo;

                        if (OrbitViewModel.Default.SelectedSat?.SatCatItem.NORAD_CAT_ID == norad_cat_id)
                        {
                            App.Current.Dispatcher.Invoke(() => { OrbitViewModel.Default.UpdateOrbit(); });
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
                foreach (var sat in vm.TargetSatellites)
                {
                    var sats = Satellites.Where(x => x.SatCatItem.NORAD_CAT_ID == sat.SatCatItem.NORAD_CAT_ID);
                    if (!sats.Any())
                    {
                        Satellites.Add(sat);
                        GlobalData.Default.CelesTrak.AddTrackingTarget(sat.SatCatItem.NORAD_CAT_ID, sat.SatCatItem);
                        OrbitViewModel.Default.AddDisplayTarget(sat);
                    }
                }

                WriteTrackingTargetList();
            }
        }

        private void OnRemoveTrackingTargetCommand(Satellite selectedSat)
        {
            if (selectedSat == null)
            {
                return;
            }

            if (Satellites.Remove(selectedSat))
            {
                OrbitViewModel.Default.RemoveDisplayTarget(selectedSat);
                GlobalData.Default.CelesTrak.RemoveTrackingTarget(selectedSat.SatCatItem.NORAD_CAT_ID);

                WriteTrackingTargetList();
            }
        }

        private void WriteTrackingTargetList()
        {
            var catalogDatas = Satellites.Select(x => x.SatCatItem);
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
                        var sat = new Satellite() { SatCatItem = satCat };
                        Satellites.Add(sat);

                        OrbitViewModel.Default.AddDisplayTarget(sat);
                        GlobalData.Default.CelesTrak.AddTrackingTarget(satCat.NORAD_CAT_ID, satCat);
                    }
                }
            }
        }
    }
}