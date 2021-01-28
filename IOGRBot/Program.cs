using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var botConfig = configuration.GetSection("DiscordBot").Get<BotConfiguration>();
                    services.AddSingleton(botConfig);
                    
                    var discordClient = new DiscordSocketClient();
                    services.AddSingleton(discordClient);
                    
                    services.AddSingleton<IScheduler, Scheduler>();
                    services.AddSingleton<IBot, Bot>();
                    services.AddSingleton<IIOGRFetcher, IOGRFetcher>();
                    
                    services.AddHostedService<Worker>();
                });
    }
}
