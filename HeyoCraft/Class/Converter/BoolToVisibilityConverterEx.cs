using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HeyoCraft.Class.Converter
{
    class BoolToVisibilityConverterEx : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool src = (bool)value;
                if (((string)parameter)?.Length > 0)
                    src = !src;
                if (src)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool src = ((Visibility)value) == Visibility.Visible;
                if (((string)parameter)?.Length>0)
                    src = !src;

                return src;
            }
            catch
            {
                return false;
            }
        }
    }
}
