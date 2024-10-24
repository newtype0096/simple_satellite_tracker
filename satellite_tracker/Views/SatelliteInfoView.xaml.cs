using satellite_tracker.ViewModels;
using System.Windows.Controls;

namespace satellite_tracker.Views
{
    /// <summary>
    /// SatelliteInfoView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SatelliteInfoView : UserControl
    {
        public SatelliteInfoView()
        {
            InitializeComponent();

            DataContext = SatelliteInfoViewModel.Default;
        }
    }
}