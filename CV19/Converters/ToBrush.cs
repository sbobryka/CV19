using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace CV19.Converters
{
    internal class ToBrush : MultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3) return null;

            byte[] RGB = new byte[3];
            RGB[0] = System.Convert.ToByte(values[0]);
            RGB[1] = System.Convert.ToByte(values[1]);
            RGB[2] = System.Convert.ToByte(values[2]);

            var color = Color.FromRgb(RGB[0], RGB[1], RGB[2]);
            var brush = new SolidColorBrush(color);
            return brush;
        }
    }
}
