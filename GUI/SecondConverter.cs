using System;
using System.Collections.Generic;
using System.Windows.Data;
using PracticalWork;
using System.Text;

namespace GUI
{
    class SecondConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataItem ob = (DataItem)value;
            string res = "";
            res += ob.Module + "\n";
            return res;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
