namespace satellite_tracker.Models
{
    public class SatelliteIndicator : Indicator
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

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public SatelliteIndicator()
        {
        }

        public void UpdateIndicator(Satellite selectedSat, double parentWidth, double parentHeight)
        {
            if (Sat == null || Sat.TrackingInfoItem == null || Sat.TrackingInfoItem.CoordinateItem == null)
            {
                IsVisible = false;
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