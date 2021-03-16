using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Text;
using PracticalWork;

namespace GUI
{
    class FirstConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataItem ob = (DataItem)value;
            string res = "";
            res += "x = " + ob.Coord.X + ", y = " + ob.Coord.Y + " ";
            return res;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
