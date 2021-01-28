using IOGRBot;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IOGR_Discord_Tests
{
    public class HighScoreTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestResetOk()
        {
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "01:23:45");
            string before = sut.ToString();
            sut.Reset();
            string after = sut.ToString();
            Assert.AreEqual(after, string.Empty);
            Assert.AreNotEqual(before, after);
        }

        [Test]
        [TestCase("Inno", "01:23:45")]
        public void TestAddValidTime(string username, string time)
        {
            HighScore sut = new HighScore();
            sut.TryAdd(username, time);
            var scores = sut.TimesByPerson;
            Assert.IsTrue(scores[username] == time);
        }

        [Test]
        [TestCase("Inno", "abcdef")]
        [TestCase("Inno", "-1:00:00")]
        [TestCase("Inno", "99:99:99")]
        [TestCase("Inno", "00:60:00")]
        [TestCase("Inno", "00:00:60")]

        public void TestAddInvalidTime(string username, string time)
        {
            HighScore sut = new HighScore();
            var addResult = sut.TryAdd(username, time);
            Assert.IsTrue(addResult == HighScoreAddResult.InvalidCommandString);
        }

        [Test]
        public void TestAddDuplicateTimeWithOverwrite()
        {
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "3:33:33");
            var addResult = sut.TryAdd("Inno", "0:00:33");
            Assert.AreEqual(addResult, HighScoreAddResult.DuplicateEntryWithoutOverwrite);
        }

        [Test]
        public void TestAddDuplicatTimeWithoutOverwrite()
        {
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "3:33:33");
            var addResult = sut.TryAdd("Inno", "0:00:33", true);
            var scores = sut.TimesByPerson;
            Assert.AreEqual(addResult, HighScoreAddResult.Ok);
            Assert.AreEqual(scores["Inno"], "0:00:33");
        }

        [Test]
        public void TestTimesToStringOk()
        {
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "3:33:33");
            sut.TryAdd("Exen", "0:33:33");
            var actual = sut.ToString();
            string expected = "Exen 0:33:33\r\nInno 3:33:33\r\n";
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void TestTimesAreRankedOk()
        {
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "3:33:33");
            sut.TryAdd("Exen", "0:33:33");
            var actual = sut.ToStringWithRankings();
            string expected = "Current rankings:\r\n1) Exen 0:33:33\r\n2) Inno 3:33:33";
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public async Task TestLoadFromFileOk()
        {
            string fileContent = "Inno 1:23:45\r\nExen 00:23:45\r\n";
            string fut = @"LoadFromFile.txt";
            await File.WriteAllTextAsync(fut, fileContent);

            HighScore sut = new HighScore();
            await sut.LoadFromFileAsync(fut);
            string actual = sut.ToString();
            string expected = "Exen 00:23:45\r\nInno 1:23:45\r\n";
            Assert.AreEqual(actual, expected);
            File.Delete(fut);
        }

        [Test]
        public async Task TestSaveToFileOk()
        {
            string fut = @"TestSaveToFileOk.txt";
            HighScore sut = new HighScore();
            sut.TryAdd("Inno", "1:23:45");
            await sut.SaveToFileAsync(fut);
            sut.Reset();
            await sut.LoadFromFileAsync(fut);
            var scores = sut.TimesByPerson;
            Assert.AreEqual(scores["Inno"], "1:23:45");
            File.Delete(fut);
        }

        [Test]
        public void TestSkipToArray()
        {
            string[] sut = { "a" };
            var skip = sut.Skip(1).ToArray();
            Assert.IsNotNull(skip);
        }
    }
}