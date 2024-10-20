using satellite_tracker.ViewModels;
using System.Windows.Controls;

namespace satellite_tracker.Views
{
    /// <summary>
    /// OrbitView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrbitView : UserControl
    {
        public OrbitView()
        {
            InitializeComponent();

            DataContext = OrbitViewModel.Default;

            Loaded += (sender, e) =>
            {
                OrbitViewModel.Default.WindowWidth = ActualWidth;
                OrbitViewModel.Default.WindowHeight = ActualHeight;
            };
        }
    }
}