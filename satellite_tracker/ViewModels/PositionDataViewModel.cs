using CommunityToolkit.Mvvm.ComponentModel;
using satellite_tracker.Models;

namespace satellite_tracker.ViewModels
{
    public class PositionDataViewModel : ObservableObject
    {
        public static PositionDataViewModel Default { get; } = new PositionDataViewModel();

        private SatelliteInfo _selectedInfo;
        public SatelliteInfo SelectedInfo
        {
            get => _selectedInfo;
            set => SetProperty(ref _selectedInfo, value);
        }
    }
}