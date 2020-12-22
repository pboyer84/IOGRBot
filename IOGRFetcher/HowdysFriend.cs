using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IOGRFetcher
{
    public class HowdysFriend
    {
        // Make a webhook in Discord and get the url endpoints
        // Removed because you don't source control secret
        private string HowdysFriendWebhookEndpoint = @"xxx";

        public async Task TellHowdyNewSeed(string permalink)
        {
            Console.WriteLine("Talking to Howdy: NewSeed");
            var client = new HttpClient();
            var payload = File.ReadAllText("newseed.json");
            var s = $"{{\"content\":\"{permalink}\"}}";
            HttpContent f = new StringContent(s, Encoding.UTF8, "application/json");

            await client.PostAsync(HowdysFriendWebhookEndpoint, f);
        }
    }
}
