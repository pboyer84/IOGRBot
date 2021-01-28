using Microsoft.Extensions.Configuration;

namespace IOGRBot
{
    public class BotConfiguration
    {
        public string AnnouncementChannel { get; set; }
        public string BotToken { get; set; }
        public string CommandListeningChannel { get; set; }
        public string HighScoreChannel { get; set; }
        public string HighScoreFilename { get; set; }
        public string RecurringSeedCronSchedule { get; set; }
    }
}