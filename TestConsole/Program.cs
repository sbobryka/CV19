using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        private const string dataUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        private static async Task<Stream> GetDataStream()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(dataUrl, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var dataStream = GetDataStream().Result;
            using var dataReader = new StreamReader(dataStream);

            while (!dataReader.EndOfStream)
            {
                var line = dataReader.ReadLine();
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

        private static IEnumerable<(string Country, string Province, int[] Counts)> GetData()
        {
            IEnumerable<string[]> lines = GetDataLines().Skip(1).Select(l => l.Split(','));

            foreach (string[] line in lines)
            {
                string province = line[0].Trim();
                string country = line[1].Trim(' ', '"');
                int[] counts = line.Skip(4).Select(int.Parse).ToArray();

                yield return (country, province, counts);
            }
        }

        static void Main(string[] args)
        {
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    //HttpResponseMessage httpResponseMessage = httpClient.GetAsync(dataUrl).Result;
            //    //string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //    string result = httpClient.GetStringAsync(dataUrl).Result;
            //    Console.WriteLine(result);
            //}

            //foreach (var line in GetDataLines())
            //    Console.WriteLine(line);

            //var dates = GetDates();
            //Console.WriteLine(string.Join("\n", dates));

            var russiaData = GetData().First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(string.Join("\n", GetDates()
                .Zip(russiaData.Counts, (date, count) => $"{date:dd:MM:yyyy} - {count}")));
        }
    }
}
