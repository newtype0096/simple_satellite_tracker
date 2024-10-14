using CelesTrakLib;
using CelesTrakLib.Datas;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace satellite_tracker.ViewModels
{
    public class SatelliteSearchWindowViewModel : ObservableObject, IModalDialogViewModel
    {
        private List<SatelliteCatalog> _satelliteCatalogs;

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
            set
            {
                SetProperty(ref _searchText, value.ToUpper());

                FilterSatCat();
            }
        }

        private ObservableCollection<SatelliteCatalog> _filteredSatelliteCatalogs;
        public ObservableCollection<SatelliteCatalog> FilteredSatelliteCatalog
        {
            get => _filteredSatelliteCatalogs;
            set => SetProperty(ref _filteredSatelliteCatalogs, value);
        }

        private List<SatelliteCatalog> _targetSatelliteCatalogs = new List<SatelliteCatalog>();
        public List<SatelliteCatalog> TargetSatelliteCatalogs
        {
            get => _targetSatelliteCatalogs;
            set => SetProperty(ref _targetSatelliteCatalogs, value);
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
            if (CelesTrak.Default.GetSatelliteCatalogs(out var response))
            {
                _satelliteCatalogs = response.SatelliteCatalogs;
            }
        }

        private void FilterSatCat()
        {
            if (_satelliteCatalogs == null)
            {
                return;
            }

            var checkBoxResult = _satelliteCatalogs
                .Where(x => IsOnOrbit ? x.OPS_STATUS_CODE != "D" : true)
                .Where(x => IsActive ? x.OPS_STATUS_CODE == "+" || x.OPS_STATUS_CODE == "P" : true)
                .Where(x => IsPayload ? x.OBJECT_TYPE == "PAY" : true);

            var searchResult = checkBoxResult.Where(
                x => x.OBJECT_ID.ToUpper().Contains(SearchText) ||
                x.NORAD_CAT_ID.ToUpper().Contains(SearchText) ||
                x.OBJECT_NAME.ToUpper().Contains(SearchText) ||
                x.OWNER.ToUpper().Contains(SearchText) ||
                x.LAUNCH_DATE.ToUpper().Contains(SearchText) ||
                x.LAUNCH_SITE.ToUpper().Contains(SearchText) ||
                x.DECAY_DATE.ToUpper().Contains(SearchText) ||
                x.OPS_STATUS_CODE.ToUpper().Contains(SearchText));

            FilteredSatelliteCatalog = new ObservableCollection<SatelliteCatalog>(searchResult);
        }
    }
}