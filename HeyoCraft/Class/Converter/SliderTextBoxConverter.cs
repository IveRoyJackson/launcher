using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HeyoCraft.Class.Converter
{
    class SliderTextBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value.ToString();
            s= s.Substring(0, s.IndexOf('.') < 0 ? s.Length : s.IndexOf('.'));
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return double.Parse((string)value);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
