using CommunityToolkit.Mvvm.ComponentModel;

namespace satellite_tracker.Models
{
    public class PropertyItem : ObservableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private object _data;
        public object Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }
    }
}