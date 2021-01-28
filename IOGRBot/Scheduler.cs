using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Scheduler : IScheduler
    {
        public static IBot Owner { get; private set; }

        private Quartz.IScheduler scheduler;

        public async Task<bool> TryInitWithSchedule(IBot owner, string cronExpression)
        {
            Owner = owner;
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            var factory = new StdSchedulerFactory();
            scheduler = await factory.GetScheduler();
            bool ok = await TrySetSchedule(cronExpression, scheduler);
            return ok;
        }

        private async Task<bool> TrySetSchedule(string cronExpression, Quartz.IScheduler scheduler)
        {
            if (!CronExpression.IsValidExpression(cronExpression))
            {
                return false;
            }

            IJobDetail job = JobBuilder.Create<PostWeeklySeedJob>()
                .WithIdentity("WeeklySeedJob", "Group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("schedule", "Group1")
                .StartNow()
                .WithCronSchedule(cronExpression)
                .Build();

            await scheduler.Clear();
            await scheduler.ScheduleJob(job, trigger);
            return true;
        }

        public async Task Start()
        {
            if (scheduler?.InStandbyMode ?? false)
            {
                await scheduler.Start();
            }
        }

        public async Task Shutdown()
        {
            if (scheduler?.IsStarted ?? false)
            {
                await scheduler.Shutdown();
            }
        }
    }
}