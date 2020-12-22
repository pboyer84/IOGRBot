using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IOGRFetcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await InitScheduler();
        }

        private async static Task InitScheduler()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            var factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<PostWeeklySeedJob>()
                .WithIdentity("WeeklySeedJob", "Group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("FridayNoonSchedule", "Group1")
                .StartNow()
                .WithCronSchedule(@"0 0 12 ? * FRI *")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            
            Console.WriteLine("Jobs will continue. Press any key to close the application");
            Console.ReadKey();

            await scheduler.Shutdown();
        }
    }
}
