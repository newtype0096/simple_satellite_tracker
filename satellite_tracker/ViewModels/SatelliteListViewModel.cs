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
        private string _trackingTargetListFileName;

        private ObservableCollection<SatelliteInfo> _satelliteInfos = new ObservableCollection<SatelliteInfo>();
        public ObservableCollection<SatelliteInfo> SatelliteInfos
        {
            get => _satelliteInfos;
            set => SetProperty(ref _satelliteInfos, value);
        }

        public RelayCommand SettingsCommand { get; }

        public SatelliteListViewModel()
        {
            _trackingTargetListFileName = Path.Combine(GlobalData.Default.CurrentDirectory, "tracking_targets.txt");

            SettingsCommand = new RelayCommand(OnSettings);

            LoadTrackingTargetList();
        }

        private void OnSettings()
        {
            var vm = new SatelliteSearchWindowViewModel();
            if (GlobalData.Default.DialogService.ShowDialog(this, vm) == true)
            {
                foreach (var info in vm.TargetSatelliteCatalogInfos)
                {
                    GlobalData.Default.SatelliteTracker.AddTrackingTarget(info.Data.NORAD_CAT_ID);

                    var infos = SatelliteInfos.Where(x => x.CatalogData.NORAD_CAT_ID == info.Data.NORAD_CAT_ID);
                    if (!infos.Any())
                    {
                        SatelliteInfos.Add(new SatelliteInfo() { CatalogData = info.Data });
                    }
                }

                WriteTrackingTargetList();
            }
        }

        private void WriteTrackingTargetList()
        {
            var catalogDatas = SatelliteInfos.Select(x => x.CatalogData);
            string jsonString = JsonConvert.SerializeObject(catalogDatas, Formatting.Indented);

            File.WriteAllText(_trackingTargetListFileName, jsonString);
        }

        private void LoadTrackingTargetList()
        {
            if (File.Exists(_trackingTargetListFileName))
            {
                string jsonString = File.ReadAllText(_trackingTargetListFileName);
                var catalogs = JsonConvert.DeserializeObject<ObservableCollection<SatelliteCatalogData>>(jsonString);
                if (catalogs != null)
                {
                    foreach (var data in catalogs)
                    {
                        GlobalData.Default.SatelliteTracker.AddTrackingTarget(data.NORAD_CAT_ID);

                        SatelliteInfos.Add(new SatelliteInfo() { CatalogData = data});
                    }
                }
            }
        }
    }
}