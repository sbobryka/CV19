using CV19.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CV19.Services
{
    internal class DataService
    {
        private const string DATA_SOURCE_ADDRESS = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        private static async Task<Stream> GetDataStream()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(DATA_SOURCE_ADDRESS, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using Stream dataStream = (SynchronizationContext.Current is null ? GetDataStream() : Task.Run(GetDataStream)).Result;
            using StreamReader dataReader = new StreamReader(dataStream);

            while (!dataReader.EndOfStream)
            {
                string line = dataReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                yield return line.Replace(", ", " ");
            }
        }

        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();

        private static IEnumerable<(string country, string province, (double lat, double lon) place, int[] counts)> GetCountriesData()
        {
            IEnumerable<string[]> lines = GetDataLines().Skip(1).Select(l => l.Split(','));

            foreach (string[] line in lines)
            {
                string province = line[0].Trim();
                string country = line[1].Trim(' ', '"');
                double.TryParse(line[2].Trim().Replace('.', ','), out double latitude);
                double.TryParse(line[3].Trim().Replace('.', ','), out double longitude);
                //double latitude = double.Parse(line[2].Trim().Replace('.', ','));
                //double longitude = double.Parse(line[3].Trim().Replace('.', ','));

                int[] counts = line.Skip(4).Select(int.Parse).ToArray();

                yield return (country, province, (latitude, longitude), counts);
            }
        }

        public IEnumerable<CountryInfo> GetData()
        {
            DateTime[] dates = GetDates();

            var data = GetCountriesData().GroupBy(d => d.country);

            foreach (var countryInfo in data)
            {
                var country = new CountryInfo()
                {
                    Name = countryInfo.Key,
                    Provinces = countryInfo.Select(c => new PlaceInfo
                    {
                        Name = c.province,
                        Location = new Point(c.place.lat, c.place.lon),
                        Counts = dates.Zip(c.counts, (date, count) => new ConfirmedCount { Date = date, Count = count })
                    })
                };
                yield return country;
            }
        }
    }
}
