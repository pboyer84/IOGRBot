using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class IOGRFetcher : BaseConfigurableObject, IIOGRFetcher
    {
        private Random rng = new Random();
        private readonly HttpClient httpClient;
        private string inputJsonFile;
        private string iogrApiGenerateEndpoint;
        private string iogrAppPermalinkBaseUrl;
        public IOGRFetcher(HttpClient httpClient, IOGRFetcherConfiguration iOGRFetcherConfiguration) : base(iOGRFetcherConfiguration)
        {
            this.httpClient = httpClient;
            Configure(iOGRFetcherConfiguration);
        }

        private void Configure(IOGRFetcherConfiguration iOGRFetcherConfiguration)
        {
            inputJsonFile = iOGRFetcherConfiguration.InputJsonFile;
            iogrApiGenerateEndpoint = iOGRFetcherConfiguration.IogrApiGenerateEndpoint;
            
        }

        private int GenerateSeed()
        {
            int value = (int)Math.Floor(int.MaxValue * rng.NextDouble());
            Console.WriteLine("Next seed: " + value);

            return value;
        }

        public async Task<string> GetNewSeedPermalink()
        {;
            string input = File.ReadAllText(inputJsonFile);
            JObject i = JObject.Parse(input);
            var seedProperty = i.Property("seed");
            seedProperty.Value = GenerateSeed();
            input = i.ToString(Newtonsoft.Json.Formatting.None);

            HttpContent body = new StringContent(input, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(iogrApiGenerateEndpoint, body);
            var data = await response?.Content?.ReadAsStringAsync();

            JObject o = JObject.Parse(data);
            var permalinkProperty = o.Property("permalink_id");

            return $"{iogrAppPermalinkBaseUrl}{permalinkProperty.Value}";
        }
    }
}
