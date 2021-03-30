using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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
            iogrAppPermalinkBaseUrl = iOGRFetcherConfiguration.IogrAppPermalinkBaseUrl;
        }

        private int GenerateSeed()
        {
            int value = (int)Math.Floor(int.MaxValue * rng.NextDouble());
            Console.WriteLine("Next seed: " + value);

            return value;
        }

        public async Task<string> GetNewSeedPermalink()
        {
            string input = File.ReadAllText(inputJsonFile);
            var randomizerFlags = JsonSerializer.Deserialize<RandomizerFlags>(input);
            randomizerFlags.seed = GenerateSeed();

            string bodyContent = JsonSerializer.Serialize<RandomizerFlags>(randomizerFlags);
            HttpContent body = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(iogrApiGenerateEndpoint, body);
            var responseContent = await response?.Content?.ReadAsStringAsync();

            var info = JsonSerializer.Deserialize<RandomizationInfo>(responseContent);

            return $"{iogrAppPermalinkBaseUrl}{info.permalink_id}";
        }
    }
}