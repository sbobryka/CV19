using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CV19.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    internal class Linear : Converter
    {
        public double K { get; set; } = 1;
        public double B { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            var x = System.Convert.ToDouble(value, culture);
            return x * K + B;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            var y = System.Convert.ToDouble(value, culture);
            return (y - B) / K;
        }
    }
}
