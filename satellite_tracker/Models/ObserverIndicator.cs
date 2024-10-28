namespace satellite_tracker.Models
{
    public class ObserverIndicator : Indicator
    {
        public ObserverIndicator()
        {
            LabelText = "Observer";
        }

        public void UpdateIndicator(double latitude, double longitude, double parentWidth, double parentHeight)
        {
            if (latitude == 0 && longitude == 0)
            {
                IsVisible = false;
                return;
            }

            IsVisible = true;

            int x = (int)((longitude + 180.0) * (parentWidth / 360.0));
            int y = (int)((90.0 - latitude) * (parentHeight / 180.0));

            MarkerLeft = x - (MarkerWidth / 2);
            MarkerTop = y - (MarkerHeight / 2);

            LabelLeft = x - (LabelWidth / 2);
            LabelTop = MarkerTop + MarkerHeight;
        }
    }
}