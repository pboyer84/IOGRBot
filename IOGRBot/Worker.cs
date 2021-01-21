using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var scheduler = new Scheduler();
            var bot = new Bot(scheduler);
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