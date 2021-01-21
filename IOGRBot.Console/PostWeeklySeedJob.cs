using Quartz;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class PostWeeklySeedJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var iogrFetcher = new IOGRFetcher();
            string url = await iogrFetcher.GetNewSeedPermalink();
            await Scheduler.Owner.PostAnnouncement(url);
        }
    }
}