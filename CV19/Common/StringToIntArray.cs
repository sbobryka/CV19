using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace CV19.Common
{
    [MarkupExtensionReturnType(typeof(int[]))]
    internal class StringToIntArray : MarkupExtension
    {
        [ConstructorArgument("Str")]
        public string Str { get; set; }
        [ConstructorArgument("Separator")]
        public char Separator { get; set; } = ';';

        public StringToIntArray() { }
        public StringToIntArray(string str) => Str = str;
        public StringToIntArray(string str, char separator) : this(str) => Separator = separator;

        public override object ProvideValue(IServiceProvider serviceProvider) =>
            Str.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries).DefaultIfEmpty().Select(int.Parse).ToArray();
    }
}
