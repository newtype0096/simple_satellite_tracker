using CommunityToolkit.Mvvm.ComponentModel;

namespace satellite_tracker.Models
{
    public class SatelliteIndicator : ObservableObject
    {
        private Satellite _sat;
        public Satellite Sat
        {
            get => _sat;
            set
            {
                LabelText = value.SatCatItem.OBJECT_NAME;

                SetProperty(ref _sat, value);
            }
        }

        private bool _isVisible = false;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
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

        public SatelliteIndicator()
        {
        }

        public void UpdateIndicator(Satellite selectedSat, double parentWidth, double parentHeight)
        {
            if (Sat == null || Sat.TrackingInfoItem == null || Sat.TrackingInfoItem.CoordinateItem == null)
            {
                IsSelected = false;
                return;
            }

            IsVisible = true;
            IsSelected = Sat == selectedSat;

            var coordinate = Sat.TrackingInfoItem.CoordinateItem;
            int x = (int)((coordinate.getLongitude() + 180.0) * (parentWidth / 360.0));
            int y = (int)((90.0 - coordinate.getLatitude()) * (parentHeight / 180.0));

            MarkerLeft = x - (MarkerWidth / 2);
            MarkerTop = y - (MarkerHeight / 2);

            LabelLeft = x - (LabelWidth / 2);
            LabelTop = MarkerTop + MarkerHeight;
        }
    }
}