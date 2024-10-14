﻿using CelesTrakLib;
using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.ViewModels
{
    public class SatelliteListViewModel : ObservableObject
    {
        public SatelliteListViewModel()
        {
            SettingsCommand = new RelayCommand(OnSettings);
        }

        public RelayCommand SettingsCommand { get; }

        private void OnSettings()
        {
            var vm = new SatelliteSearchWindowViewModel();
            GlobalData.Default.DialogService.ShowDialog(this, vm);
        }
    }
}
