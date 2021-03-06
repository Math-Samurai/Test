﻿using System;
using System.Collections.Generic;
using System.Windows.Data;
using PracticalWork;
using System.Text;

namespace GUI
{
    class ThirdConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            V3DataOnGrid ob = (V3DataOnGrid)value;
            string res = "";
            if (ob != null)
            {
                res += "Количество точек по оси х: " + ob.Ox.Count + "\nКоличество точек по оси y: " + ob.Oy.Count + "\n";
            }
            return res;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
