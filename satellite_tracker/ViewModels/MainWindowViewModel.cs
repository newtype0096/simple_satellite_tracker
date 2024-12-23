﻿using CommunityToolkit.Mvvm.ComponentModel;
using MvvmDialogs;
using System.Collections.Generic;

namespace satellite_tracker.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public SatelliteInfoViewModel SatelliteInfo => SatelliteInfoViewModel.Default;

        public SatelliteStatsViewModel SatelliteStats => SatelliteStatsViewModel.Default;

        public ObserverInfoViewModel ObserverInfo => ObserverInfoViewModel.Default;

        public Orbit3dViewModel Orbit3d => Orbit3dViewModel.Default;

        private PaneViewModel[] _panes;
        public IEnumerable<PaneViewModel> Panes
        {
            get
            {
                if (_panes == null)
                {
                    _panes = new PaneViewModel[] { SatelliteInfo, SatelliteStats, ObserverInfo, Orbit3d };
                }

                return _panes;
            }
        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            GlobalData.Default.DialogService = dialogService;
        }
    }
}