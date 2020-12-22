using System;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.MainAsync();
        }
    }
}