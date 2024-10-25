using satellite_tracker.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace satellite_tracker
{
    internal class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SatelliteInfoViewTemplate { get; set; }

        public DataTemplate SatelliteStatsViewTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SatelliteInfoViewModel)
            {
                return SatelliteInfoViewTemplate;
            }

            if (item is SatelliteStatsViewModel)
            {
                return SatelliteStatsViewTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}