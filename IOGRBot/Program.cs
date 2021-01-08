using System;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var scheduler = new Scheduler();
            var bot = new Bot(scheduler);
            await bot.MainAsync();

            
        }
    }
}