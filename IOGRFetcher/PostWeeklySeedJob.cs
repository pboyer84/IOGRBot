using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IOGRFetcher
{
    public class PostWeeklySeedJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var iogrFetcher = new IOGRFetcher();
            string url = await iogrFetcher.GetNewSeedPermalink();

            var howdysFriend = new HowdysFriend();
            await howdysFriend.TellHowdyNewSeed(url);
        }
    }
}
