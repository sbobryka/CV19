using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CV19.Models
{
    internal class CountryInfo : PlaceInfo
    {
        public override Point Location
        {
            get
            {
                var averageX = ProvinceCounts.Average(p => p.Location.X);
                var averageY = ProvinceCounts.Average(p => p.Location.Y);
                return new Point(averageX, averageY);
            }
            set => base.Location = value;
        }
        public IEnumerable<PlaceInfo> ProvinceCounts { get; set; }
    }
}
