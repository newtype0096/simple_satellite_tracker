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

        private SatelliteInfo _selectedInfo;
        public SatelliteInfo SelectedInfo
        {
            get => _selectedInfo;
            set
            {
                SetProperty(ref _selectedInfo, value);

                UpdateOrbit();
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
            }
        }

        private void UpdateOrbit()
        {
            if (SelectedInfo == null || SelectedInfo.Coordinates == null)
            {
                return;
            }

            var orbitLines = new List<OrbitLine>();
            int oldX = 0, oldY = 0;

            foreach (var coordinate in SelectedInfo.Coordinates)
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
    }
}