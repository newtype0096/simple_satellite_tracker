using CommunityToolkit.Mvvm.DependencyInjection;
using satellite_tracker.ViewModels;

namespace satellite_tracker
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowVM => Ioc.Default.GetRequiredService<MainWindowViewModel>();
    }
}