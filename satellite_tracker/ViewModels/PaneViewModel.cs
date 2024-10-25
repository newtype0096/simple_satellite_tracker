using CommunityToolkit.Mvvm.ComponentModel;

namespace satellite_tracker.ViewModels
{
    public class PaneViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public PaneViewModel(string title)
        {
            Title = title;
        }
    }
}