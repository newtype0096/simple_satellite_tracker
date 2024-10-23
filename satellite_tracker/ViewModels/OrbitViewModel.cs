using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using satellite_tracker.Models;
using satellite_tracker.Views.Controls;
using System;

namespace satellite_tracker.ViewModels
{
    public class OrbitViewModel : ObservableObject
    {
        public static OrbitViewModel Default { get; } = new OrbitViewModel();

        public OrbitLineControl OrbitLineControl { get; set; }

        public double WindowWidth { get; set; } = 0;
        public double WindowHeight { get; set; } = 0;

        private bool _isGpMarkerVisible = false;
        public bool IsGpMarkerVisible
        {
            get => _isGpMarkerVisible;
            set => SetProperty(ref _isGpMarkerVisible, value);
        }

        private int _gpMarkerLeft;
        public int GpMarkerLeft
        {
            get => _gpMarkerLeft;
            set => SetProperty(ref _gpMarkerLeft, value);
        }

        private int _gpMarkerTop;
        public int GpMarkerTop
        {
            get => _gpMarkerTop;
            set => SetProperty(ref _gpMarkerTop, value);
        }

        public int GpMarkerWidth { get; } = 10;
        public int GpMarkerHeight { get; } = 10;

        private SatelliteInfo _selectedInfo;
        public SatelliteInfo SelectedInfo
        {
            get => _selectedInfo;
            set
            {
                SetProperty(ref _selectedInfo, value);

                UpdateOrbit();
                UpdateGp();
            }
        }

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
                UpdateGp();
            }
        }

        public void UpdateOrbit()
        {
            OrbitLineControl?.ClearOrbitLine();

            if (SelectedInfo == null || SelectedInfo.TrackingInfoItem == null || SelectedInfo.TrackingInfoItem.Coordinates == null)
            {
                return;
            }

            int oldX = 0, oldY = 0;

            foreach (var coordinate in SelectedInfo.TrackingInfoItem.Coordinates)
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
        }

        public void UpdateGp()
        {
            if (SelectedInfo == null || SelectedInfo.TrackingInfoItem == null || SelectedInfo.TrackingInfoItem.CoordinateItem == null)
            {
                IsGpMarkerVisible = false;
                return;
            }

            var coordinate = SelectedInfo.TrackingInfoItem.CoordinateItem;
            int x = (int)((coordinate.getLongitude() + 180.0) * (WindowWidth / 360.0));
            int y = (int)((90.0 - coordinate.getLatitude()) * (WindowHeight / 180.0));

            GpMarkerLeft = x - (GpMarkerWidth / 2);
            GpMarkerTop = y - (GpMarkerHeight / 2);

            IsGpMarkerVisible = true;
        }
    }
}