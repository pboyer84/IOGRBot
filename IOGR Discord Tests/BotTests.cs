using Discord.WebSocket;
using IOGRBot;
using Moq;
using NUnit.Framework;
using System;

namespace IOGR_Discord_Tests
{
    public class BotTests
    {
        private Mock<DiscordSocketClient> mockDiscordClient = new Mock<DiscordSocketClient>();
        private Mock<IOGRFetcher> mockIogrClient = new Mock<IOGRFetcher>();
        private Mock<Scheduler> mockScheduler = new Mock<Scheduler>();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMissingConfigThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(null));
            Assert.AreEqual(ex.Message, "Missing configuration value: AdminUsername. Cannot start application.");
        }

        [Test]
        public void TestMissingAdminUsernameThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: AdminUsername. Cannot start application.");
        }

        [Test]
        public void TestMissingAnnouncementChannelThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: AnnouncementChannel. Cannot start application.");
        }

        [Test]
        public void TestMissingCommandListeningChannelThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = "some value",
                CommandListeningChannel = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: CommandListeningChannel. Cannot start application.");
        }

        [Test]
        public void TestMissingHighScoreChannelThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = "some value",
                CommandListeningChannel = "some value",
                HighScoreChannel = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: HighScoreChannel. Cannot start application.");
        }

        [Test]
        public void TestMissingHighScoreFilenameThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = "some value",
                CommandListeningChannel = "some value",
                HighScoreChannel = "some value",
                HighScoreFilename = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: HighScoreFilename. Cannot start application.");
        }

        [Test]
        public void TestMissingLoginTokenThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = "some value",
                CommandListeningChannel = "some value",
                HighScoreChannel = "some value",
                HighScoreFilename = "some value",
                LoginToken = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: LoginToken. Cannot start application.");
        }

        [Test]
        public void TestMissingRecurringSeedCronScheduleThrowsException()
        {
            BotConfiguration testInput = new BotConfiguration()
            {
                AdminUsername = "some value",
                AnnouncementChannel = "some value",
                CommandListeningChannel = "some value",
                HighScoreChannel = "some value",
                HighScoreFilename = "some value",
                LoginToken = "some value",
                RecurringSeedCronSchedule = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: RecurringSeedCronSchedule. Cannot start application.");
        }

        private void CallFailingConstructor(BotConfiguration input)
        {
            var sut = new Bot(mockDiscordClient.Object, mockIogrClient.Object, mockScheduler.Object, input);
        }
    }
}
