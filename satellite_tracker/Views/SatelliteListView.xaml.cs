using satellite_tracker.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

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

            DataContext = SatelliteListViewModel.Default;
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var dataGrid = (DataGrid)sender;
                SatelliteListViewModel.Default.RemoveTrackingTargetCommand.Execute(dataGrid.SelectedItem);
            }
        }
    }
}