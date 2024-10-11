using CelesTrakLib;
using CelesTrakLib.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.ViewModels
{
    public class SatelliteSearchWindowViewModel : ObservableObject, IModalDialogViewModel
    {
        public SatelliteSearchWindowViewModel()
        {
            InitDialogCommand = new RelayCommand(OnInitDialog);
        }

        private void OnInitDialog()
        {
            LoadSatcat();
        }

        private void LoadSatcat()
        {
            if (CelesTrak.Default.GetSatelliteCatalogs(out var response))
            {
                SatelliteCatalog = new ObservableCollection<SatelliteCatalog>(response.SatelliteCatalogs);
            }
        }

        public bool? DialogResult { get; private set; }

        public ObservableCollection<SatelliteCatalog> SatelliteCatalog
        {
            get => _satelliteCatalogs;
            set => SetProperty(ref _satelliteCatalogs, value);
        }

        public RelayCommand InitDialogCommand { get; }

        private ObservableCollection<SatelliteCatalog> _satelliteCatalogs;
    }
}
