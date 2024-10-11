using CelesTrakLib;
using CelesTrakLib.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace satellite_tracker.ViewModels
{
    public class SatelliteSearchWindowViewModel : ObservableObject, IModalDialogViewModel
    {
        public bool? DialogResult { get; private set; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private ObservableCollection<SatelliteCatalog> _satelliteCatalogs;
        public ObservableCollection<SatelliteCatalog> SatelliteCatalog
        {
            get => _satelliteCatalogs;
            set => SetProperty(ref _satelliteCatalogs, value);
        }

        public RelayCommand InitDialogCommand { get; }

        public SatelliteSearchWindowViewModel()
        {
            InitDialogCommand = new RelayCommand(OnInitDialog);
        }

        private void OnInitDialog()
        {
            IsBusy = true;

            var worker = new BackgroundWorker();
            worker.DoWork +=
                (sender, e) =>
                {
                    LoadSatcat();
                };
            worker.RunWorkerCompleted +=
                (sender, e) =>
                {
                    IsBusy = false;
                };
            worker.RunWorkerAsync();
        }

        private void LoadSatcat()
        {
            if (CelesTrak.Default.GetSatelliteCatalogs(out var response))
            {
                SatelliteCatalog = new ObservableCollection<SatelliteCatalog>(response.SatelliteCatalogs);
            }
        }
    }
}