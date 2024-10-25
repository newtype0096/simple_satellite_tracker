using satellite_tracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace satellite_tracker
{
    class PanesStyleSelector : StyleSelector
    {
        public Style CommonStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is SatelliteInfoViewModel)
            {
                return CommonStyle;
            }

            if (item is SatelliteStatsViewModel)
            {
                return CommonStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
