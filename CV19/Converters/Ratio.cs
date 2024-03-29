﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    internal class Ratio : Converter
    {
        [ConstructorArgument("K")]
        public double K { get; set; } = 1;

        public Ratio() { }

        public Ratio(double k) => K = k;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            return System.Convert.ToDouble(value, culture) * K;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString())) return null;
            return System.Convert.ToDouble(value, culture) / K;
        }
    }
}
