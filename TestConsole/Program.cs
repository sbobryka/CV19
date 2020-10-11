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
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(dataUrl, HttpCompletionOption.ResponseHeadersRead);
                return await response.Content.ReadAsStreamAsync();
            }
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var dataStream = GetDataStream().Result;
            using var dataReader = new StreamReader(dataStream);

            while (!dataReader.EndOfStream)
            {
                var line = dataReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                yield return line;
            }
        }

        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();

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

            var dates = GetDates();
            Console.WriteLine(string.Join("\r\n", dates));
        }
    }
}
