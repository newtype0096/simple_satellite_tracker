using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.Models
{
    public class Indicator : ObservableObject
    {
        private bool _isVisible = false;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private int _markerLeft;
        public int MarkerLeft
        {
            get => _markerLeft;
            set => SetProperty(ref _markerLeft, value);
        }

        private int _markerTop;
        public int MarkerTop
        {
            get => _markerTop;
            set => SetProperty(ref _markerTop, value);
        }

        public int MarkerWidth { get; } = 10;

        public int MarkerHeight { get; } = 10;

        private int _labelLeft;
        public int LabelLeft
        {
            get => _labelLeft;
            set => SetProperty(ref _labelLeft, value);
        }

        private int _labelTop;
        public int LabelTop
        {
            get => _labelTop;
            set => SetProperty(ref _labelTop, value);
        }

        private int _labelWidth;
        public int LabelWidth
        {
            get => _labelWidth;
            set => SetProperty(ref _labelWidth, value);
        }

        private string _labelText;
        public string LabelText
        {
            get => _labelText;
            set => SetProperty(ref _labelText, value);
        }
    }
}
