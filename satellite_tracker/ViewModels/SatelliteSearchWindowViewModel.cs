using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using satellite_tracker.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace satellite_tracker.ViewModels
{
    public class SatelliteSearchWindowViewModel : ObservableObject, IModalDialogViewModel
    {
        private List<Satellite> _satellites = new List<Satellite>();

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isPayload = false;
        public bool IsPayload
        {
            get => _isPayload;
            set
            {
                SetProperty(ref _isPayload, value);

                FilterSatCat();
            }
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                SetProperty(ref _isActive, value);

                FilterSatCat();
            }
        }

        private bool _isOnOrbit = false;
        public bool IsOnOrbit
        {
            get => _isOnOrbit;
            set
            {
                SetProperty(ref _isOnOrbit, value);

                FilterSatCat();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value.ToUpper());
        }

        private ObservableCollection<Satellite> _filteredSatellites;
        public ObservableCollection<Satellite> FilteredSatellites
        {
            get => _filteredSatellites;
            set => SetProperty(ref _filteredSatellites, value);
        }

        private List<Satellite> _targetSatellites = new List<Satellite>();
        public List<Satellite> TargetSatellites
        {
            get => _targetSatellites;
            set => SetProperty(ref _targetSatellites, value);
        }

        public RelayCommand InitDialogCommand { get; }
        public RelayCommand OkCommand { get; }

        public SatelliteSearchWindowViewModel()
        {
            InitDialogCommand = new RelayCommand(OnInitDialog);
            OkCommand = new RelayCommand(OnOk);
        }

        private void OnInitDialog()
        {
            IsBusy = true;

            var worker = new BackgroundWorker();
            worker.DoWork +=
                (sender, e) =>
                {
                    LoadSatcat();

                    FilterSatCat();
                };
            worker.RunWorkerCompleted +=
                (sender, e) =>
                {
                    IsBusy = false;
                };
            worker.RunWorkerAsync();
        }

        private void OnOk()
        {
            DialogResult = true;
        }

        private void LoadSatcat()
        {
            if (GlobalData.Default.CelesTrak.GetSatCats(out var satCats))
            {
                foreach (var satCat in satCats)
                {
                    var sat = new Satellite();
                    sat.SatCatItem = satCat;
                    sat.IsTracking = GlobalData.Default.CelesTrak.IsTrackingTarget(satCat.NORAD_CAT_ID);

                    _satellites.Add(sat);
                }
            }
        }

        public void FilterSatCat()
        {
            if (_satellites == null)
            {
                return;
            }

            var checkBoxResult = _satellites
                .Where(x => IsOnOrbit ? x.SatCatItem.OPS_STATUS_CODE != "D" : true)
                .Where(x => IsActive ? x.SatCatItem.OPS_STATUS_CODE == "+" || x.SatCatItem.OPS_STATUS_CODE == "P" : true)
                .Where(x => IsPayload ? x.SatCatItem.OBJECT_TYPE == "PAY" : true);

            var searchResult = checkBoxResult.Where(
                x => x.SatCatItem.OBJECT_ID.ToUpper().Contains(SearchText) ||
                x.SatCatItem.NORAD_CAT_ID.ToUpper().Contains(SearchText) ||
                x.SatCatItem.OBJECT_NAME.ToUpper().Contains(SearchText) ||
                x.SatCatItem.OWNER.ToUpper().Contains(SearchText) ||
                x.SatCatItem.LAUNCH_DATE.ToUpper().Contains(SearchText) ||
                x.SatCatItem.LAUNCH_SITE.ToUpper().Contains(SearchText) ||
                x.SatCatItem.DECAY_DATE.ToUpper().Contains(SearchText) ||
                x.SatCatItem.OPS_STATUS_CODE.ToUpper().Contains(SearchText));

            FilteredSatellites = new ObservableCollection<Satellite>(searchResult);
        }
    }
}