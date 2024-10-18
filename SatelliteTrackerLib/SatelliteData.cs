namespace SatelliteTrackerLib
{
    public class SatelliteData
    {
        public TrackingData TrackingData { get; } = new TrackingData();
        public OrbitData OrbitData { get; } = new OrbitData();
    }
}