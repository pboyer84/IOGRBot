using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class Bot
    {
        private readonly string commandListeningChannelName = "general";
        private readonly string announcementChannelName = "general";
        private readonly string iogrHighScoresChannelName = "highscores";
        private readonly string highscoreFilename = "highscores.txt";

        private readonly DiscordSocketClient client;
        private readonly Scheduler scheduler;
        private HighScore currentScore;
        private ITextChannel announcementChannel;
        private ITextChannel commandListeningChannel;
        private ITextChannel highScoresChannel;
        private bool keepAlive = true;
        private bool ready = false;
        private IOGRFetcher iogrFetcher;
        private const string helpText =

 @"I only understand the following commands:
    !help: to display this message
    !ping: receive a test response
    !newseed: new IOGR permalink
    !sleep: (admin only) terminates my program execution";

        public Bot(Scheduler scheduler)
        {
            iogrFetcher = new IOGRFetcher();
            this.scheduler = scheduler;

            client = new DiscordSocketClient();

            client.Log += LogAsync;
            client.Ready += ReadyAsync;
            client.LoggedOut += LoggedOutAsync;
            client.MessageReceived += MessageReceivedAsync;
        }

        private async Task LoggedOutAsync()
        {
            ready = false;
            if (currentScore != null)
            {
                await currentScore.SaveToFileAsync(highscoreFilename);
            }
        }

        public async Task MainAsync()
        {

            if (await scheduler.TryInitWithSchedule(this, @"0 0 12 ? * FRI *"))
            {
                await scheduler.Start();
            }
            
            await InitHighScore();
            await client.LoginAsync(TokenType.Bot, "NzgwOTM5OTYyMDc3MDIwMTYy.X72ZBA.httAcL2rm90qTB4vfJGkKuk7qoI");
            await client.StartAsync();
            
            while (keepAlive)
            {
                await Task.Delay(10000);
            }

            await scheduler.Shutdown();
            await client.StopAsync();
            await client.LogoutAsync();
        }

        public async Task PostAnnouncement(string message)
        {
            if (ready)
            {
                await announcementChannel.SendMessageAsync(message);
            }
        }

        private async Task InitHighScore()
        {
            if (File.Exists(highscoreFilename))
            {
                currentScore = await HighScore.CreateInstanceFromFileAsync(highscoreFilename);
            }
            else
            {
                currentScore = new HighScore();
            }
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{client.CurrentUser} is connected!");
            foreach (SocketGuild g in client.Guilds)
            {
                foreach (ITextChannel t in g.TextChannels)
                {
                    if (t.Name == iogrHighScoresChannelName)
                    {
                        highScoresChannel = t;
                    }
                    if (t.Name == commandListeningChannelName)
                    {
                        commandListeningChannel = t;
                    }
                    if (t.Name == announcementChannelName)
                    {
                        announcementChannel = t;
                    }
                }
            }
            ready = true;
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == client.CurrentUser.Id ||
                message.Channel.Name != commandListeningChannelName ||
                !message.Content.StartsWith('!'))
            {
                return;
            }

            if (message.Author.Username == "Inno" && message.Content == "!reset")
            {
                currentScore.Reset();
                await message.Author.SendMessageAsync("Scores have been reset");
                return;
            }

                if (message.Author.Username == "Inno" && message.Content == "!sleep")
            {
                await message.Channel.SendMessageAsync("Goodnight!");
                keepAlive = false;
                return;
            }

            string[] instructionWithArguments = message.Content.ToLower().Split(' ');
            if (instructionWithArguments.Length > 0)
            {
                switch (instructionWithArguments[0])
                {
                    case "!ping":
                        await message.Channel.SendMessageAsync("pong!");
                        break;
                    case "!submit":
                        var time = instructionWithArguments.ElementAtOrDefault(1);
                        var switches = instructionWithArguments.Skip(2).ToArray();
                        await DoSubmit(message.Author, time, switches);
                        await message.DeleteAsync();
                        break;
                    case "!newsfeed":
                        var permalink = await iogrFetcher.GetNewSeedPermalink();
                        await message.Channel.SendMessageAsync(permalink);
                        break;
                    case "!help":
                    default:
                        await message.Channel.SendMessageAsync(helpText);
                        return;
                }
            }
        }

        private async Task DoSubmit(SocketUser user, string time, string[] switches = null)
        {
            bool overwrite = false;
            if (switches?.Length > 0)
            { 
                for (int i=0;i<switches.Length; i++)
                {
                    switch (switches[i])
                    {
                        case "-o": 
                            overwrite = true;
                            break;
                        default:
                            await user.SendMessageAsync("Unrecognized switches. Use hh:mm:ss");
                            break;
                    }
                }
            }
            
            var result = currentScore.TryAdd(user.Username, time, overwrite);

            switch (result)
            {
                case HighScoreAddResult.Ok:
                    await commandListeningChannel.SendMessageAsync($"GG {user.Username}!");
                    await highScoresChannel.SendMessageAsync(currentScore.ToStringWithRankings());
                    break;
                case HighScoreAddResult.InvalidCommandString:
                    await user.SendMessageAsync("Invalid time format. Use hh:mm:ss");
                    break;
                case HighScoreAddResult.DuplicateEntryWithoutOverwrite:
                    await user.SendMessageAsync("Duplicate entry. Use hh:mm:ss -o to overwrite old score.");
                    break;
                default: break;
            }
        }
    }
}