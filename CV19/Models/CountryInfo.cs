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
                var averageX = Provinces.Average(p => p.Location.X);
                var averageY = Provinces.Average(p => p.Location.Y);
                return new Point(averageX, averageY);
            }
            set => base.Location = value;
        }
        
        public IEnumerable<PlaceInfo> Provinces { get; set; }

        private IEnumerable<ConfirmedCount> counts;

        public override IEnumerable<ConfirmedCount> Counts 
        { 
            get
            {
                if (counts != null) return counts;

                var pointsCount = Provinces.FirstOrDefault()?.Counts?.Count() ?? 0;
                if (pointsCount == 0) return Enumerable.Empty<ConfirmedCount>();

                var provincePoints = Provinces.Select(p => p.Counts.ToArray()).ToArray();
                var points = new ConfirmedCount[pointsCount];
                foreach (var province in provincePoints)
                {
                    for (int i = 0; i < pointsCount; i++)
                    {
                        if (points[i].Date == default)
                        {
                            points[i] = province[i];
                        }
                        else
                        {
                            points[i].Count += province[i].Count;
                        }
                    }
                }

                return counts = points;
            }
            set => counts = value;
        }
    }
}
