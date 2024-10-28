using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace satellite_tracker.Views.Controls
{
    /// <summary>
    /// ClockControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClockControl : UserControl
    {
        private DispatcherTimer _timer;

        public ClockControl()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            LocalTimeZone.Content = TimeZoneInfo.Local.Id;

            UtcTime.Content = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
            KstTime.Content = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UtcTime.Content = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
            KstTime.Content = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
