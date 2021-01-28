using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IBot bot;
        public Worker(ILogger<Worker> logger, IBot bot)
        {
            this.logger = logger;
            this.bot = bot;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            // TODO Get this value from a secret vault
            string loginToken = string.Empty;
            await bot.StartAsync(loginToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }

            await bot.ShutdownAsync();
        }
    }
}