using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class IOGRFetcher
    {
        private Random rng = new Random();

        private int GenerateSeed()
        {
            int value = (int)Math.Floor(int.MaxValue * rng.NextDouble());
            Console.WriteLine("Next seed: " + value);

            return value;
        }

        public async Task<string> GetNewSeedPermalink()
        {
            var client = new HttpClient();
            string baseUrl = @"https://iogr-api-prod.azurewebsites.net/v1/seed/generate";
            string input = File.ReadAllText("iogr.json");
            JObject i = JObject.Parse(input);
            var seedProperty = i.Property("seed");
            seedProperty.Value = GenerateSeed();
            input = i.ToString(Newtonsoft.Json.Formatting.None);

            HttpContent f = new StringContent(input, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl, f);
            var data = await response?.Content?.ReadAsStringAsync();

            JObject o = JObject.Parse(data);
            var permalinkProperty = o.Property("permalink_id");

            return $"https://www.iogr.app/permalink/{permalinkProperty.Value}";
        }
    }
}
