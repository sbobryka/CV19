using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace CV19.Converters
{
    internal class ToRGBString : MultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length != 3) throw new ArgumentOutOfRangeException("Неверное количество элементов массива");

            if (values[0] == unSet || values[1] == unSet || values[2] == unSet) return null;

            byte[] RGB = new byte[3];
            RGB[0] = System.Convert.ToByte(values[0]);
            RGB[1] = System.Convert.ToByte(values[1]);
            RGB[2] = System.Convert.ToByte(values[2]);

            return $"R:{RGB[0]} G:{RGB[1]} B:{RGB[2]}";
        }
    }
}
