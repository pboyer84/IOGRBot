using Quartz;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class PostWeeklySeedJob : IJob, IPostWeeklySeedJob
    {
        private IIOGRFetcher iogrFetcher;

        public PostWeeklySeedJob(IIOGRFetcher iogrFetcher)
        {
            this.iogrFetcher = iogrFetcher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string url = await iogrFetcher.GetNewSeedPermalink();
            await Scheduler.Owner.PostAnnouncement(url);
        }
    }
}