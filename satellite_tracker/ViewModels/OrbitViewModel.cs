using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using satellite_tracker.Models;
using satellite_tracker.Views.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace satellite_tracker.ViewModels
{
    public class OrbitViewModel : ObservableObject
    {
        public static OrbitViewModel Default { get; } = new OrbitViewModel();

        public OrbitLineControl OrbitLineControl { get; set; }

        public double WindowWidth { get; set; } = 0;
        public double WindowHeight { get; set; } = 0;

        private Satellite _selectedSat;
        public Satellite SelectedSat
        {
            get => _selectedSat;
            set
            {
                SetProperty(ref _selectedSat, value);

                UpdateOrbit();
                UpdateIndicator();
            }
        }

        public ObservableCollection<SatelliteIndicator> Indicators { get; } = new ObservableCollection<SatelliteIndicator>();

        public ObserverIndicator Observer { get; } = new ObserverIndicator();

        public RelayCommand<(double, double)?> SizeCommand { get; }

        public OrbitViewModel()
        {
            SizeCommand = new RelayCommand<(double, double)?>(OnSize);
        }

        private void OnSize((double, double)? size)
        {
            if (size != null)
            {
                WindowWidth = size.Value.Item1;
                WindowHeight = size.Value.Item2;

                UpdateOrbit();
                UpdateIndicator();
            }
        }

        public void UpdateOrbit()
        {
            OrbitLineControl?.ClearOrbitLine();

            if (SelectedSat == null || SelectedSat.TrackingInfoItem == null || SelectedSat.TrackingInfoItem.Coordinates == null)
            {
                return;
            }

            int oldX = 0, oldY = 0;

            foreach (var coordinate in SelectedSat.TrackingInfoItem.Coordinates)
            {
                int x = (int)((coordinate.getLongitude() + 180.0) * (WindowWidth / 360.0));
                int y = (int)((90.0 - coordinate.getLatitude()) * (WindowHeight / 180.0));

                if (Math.Abs(x - oldX) < WindowWidth / 2)
                {
                    if (oldX != 0 && oldY != 0)
                    {
                        OrbitLineControl?.AddOrbitLine(oldX, oldY, x, y);
                    }
                }

                oldX = x;
                oldY = y;
            }

            OrbitLineControl?.DrawOrbitLine();
        }

        public void UpdateIndicator()
        {
            foreach (var indicator in Indicators)
            {
                indicator.UpdateIndicator(SelectedSat, WindowWidth, WindowHeight);
            }

            UpdateObserverIndicator();
        }

        public void UpdateIndicatorBySat(Satellite sat)
        {
            var indicator = Indicators.FirstOrDefault(x => x.Sat == sat);
            if (indicator != null)
            {
                indicator.UpdateIndicator(SelectedSat, WindowWidth, WindowHeight);
            }

            UpdateObserverIndicator();
        }

        private void UpdateObserverIndicator()
        {
            Observer.UpdateIndicator(ObserverInfoViewModel.Default.Latitude, ObserverInfoViewModel.Default.Longitude, WindowWidth, WindowHeight);
        }

        public void AddDisplayTarget(Satellite sat)
        {
            if (Indicators.Any(x => x.Sat.SatCatItem.NORAD_CAT_ID == sat.SatCatItem.NORAD_CAT_ID))
            {
                return;
            }

            Indicators.Add(new SatelliteIndicator() { Sat = sat });
        }

        public void RemoveDisplayTarget(Satellite sat)
        {
            var indicator = Indicators.Where(x => x.Sat.SatCatItem.NORAD_CAT_ID == sat.SatCatItem.NORAD_CAT_ID).FirstOrDefault();
            if (indicator != null)
            {
                Indicators.Remove(indicator);
            }
        }
    }
}