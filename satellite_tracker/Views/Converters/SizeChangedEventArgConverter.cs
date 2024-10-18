using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace satellite_tracker.Views.Converters
{
    public class SizeChangedEventArgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SizeChangedEventArgs args)
            {
                return (args.NewSize.Width, args.NewSize.Height);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}