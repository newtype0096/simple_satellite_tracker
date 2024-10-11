using satellite_tracker.ViewModels;
using System.Windows.Controls;

namespace satellite_tracker.Views
{
    /// <summary>
    /// SatelliteListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SatelliteListView : UserControl
    {
        public SatelliteListView()
        {
            InitializeComponent();

            DataContext = new SatelliteListViewModel();
        }
    }
}