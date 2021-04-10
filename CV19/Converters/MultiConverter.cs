using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CV19.Converters
{
    internal abstract class MultiConverter : IMultiValueConverter
    {
        public readonly object unSet = DependencyProperty.UnsetValue;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException("Обратное преобразование не поддерживается");
    }
}
