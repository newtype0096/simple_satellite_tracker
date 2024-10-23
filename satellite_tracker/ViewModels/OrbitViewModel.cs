using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using satellite_tracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace satellite_tracker.ViewModels
{
    public class OrbitViewModel : ObservableObject
    {
        public class OrbitLine
        {
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
        }

        public static OrbitViewModel Default { get; } = new OrbitViewModel();

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

        private ObservableCollection<OrbitLine> _orbitLines;
        public ObservableCollection<OrbitLine> OrbitLines
        {
            get => _orbitLines;
            set => SetProperty(ref _orbitLines, value);
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
            if (SelectedInfo == null || SelectedInfo.TrackingInfoItem == null || SelectedInfo.TrackingInfoItem.Coordinates == null)
            {
                OrbitLines = null;
                return;
            }

            var orbitLines = new List<OrbitLine>();
            int oldX = 0, oldY = 0;

            foreach (var coordinate in SelectedInfo.TrackingInfoItem.Coordinates)
            {
                int x = (int)((coordinate.getLongitude() + 180.0) * (WindowWidth / 360.0));
                int y = (int)((90.0 - coordinate.getLatitude()) * (WindowHeight / 180.0));

                if (Math.Abs(x - oldX) < WindowWidth / 2)
                {
                    if (oldX != 0 && oldY != 0)
                    {
                        orbitLines.Add(new OrbitLine() { X1 = oldX, Y1 = oldY, X2 = x, Y2 = y });
                    }
                }

                oldX = x;
                oldY = y;
            }

            OrbitLines = new ObservableCollection<OrbitLine>(orbitLines);
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