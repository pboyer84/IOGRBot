using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class HighScore
    {
        
        private IDictionary<string, string> EditableTimesByPerson;
        private char ScoreLineSeparator;
        private char TimePartsSeparator;

        public HighScore(char scoreLineSeparator = ' ', char timePartsSeparator = ':')
        {
            EditableTimesByPerson = new Dictionary<string, string>();
            ScoreLineSeparator = scoreLineSeparator;
            TimePartsSeparator = timePartsSeparator;
        }

        public async static Task<HighScore> CreateInstanceFromFileAsync(string scoresFilepath)
        {
            var instance = new HighScore();
            await instance.LoadFromFileAsync(scoresFilepath);
            return instance;
        }

        public IReadOnlyDictionary<string, string> TimesByPerson
        {
            get
            {
                return new ReadOnlyDictionary<string, string>(EditableTimesByPerson);
            }
        }

        public HighScoreAddResult TryAdd(string username, string time, bool overwrite = false)
        {
            string[] timeParts = time?.Split(TimePartsSeparator);
            if (!(timeParts?.Length == 3
                && int.TryParse(timeParts[0], out int hourPart)
                && int.TryParse(timeParts[1], out int minutePart)
                && int.TryParse(timeParts[2], out int secondPart)
                && hourPart >= 0
                && minutePart >= 0 && minutePart < 60
                && secondPart >= 0 && secondPart < 60))
            {
                return HighScoreAddResult.InvalidCommandString;
            }
            bool added = EditableTimesByPerson.TryAdd(username, time);
            
            if (added)
            {
                return HighScoreAddResult.Ok;
            }
            if (!overwrite)
            {
                return HighScoreAddResult.DuplicateEntryWithoutOverwrite;
            }
            else
            {
                EditableTimesByPerson[username] = time;
                return HighScoreAddResult.Ok;
            }

            
        }

        public void Reset()
        {
            EditableTimesByPerson = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            var sorted = EditableTimesByPerson.OrderBy(x => x.Value).ToList();
            var sb = new StringBuilder();
            foreach (var element in sorted)
            {
                sb.AppendLine($"{element.Key}{ScoreLineSeparator}{element.Value}");
            }
            return sb.ToString();
        }

        public string ToStringWithRankings()
        {
            var sorted = EditableTimesByPerson.OrderBy(x => x.Value).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("Current rankings:");
            for (int i=0;i<sorted.Count;i++)
            {
                sb.AppendLine($"{i+1}) {sorted[i].Key}{ScoreLineSeparator}{sorted[i].Value}");
            }

            return sb.ToString().Trim();
        }
        public async Task LoadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Could not find a file at the provided path", filePath);
            }

            var scores = await File.ReadAllLinesAsync(filePath);
            IDictionary<string, string> highscoresInFile = new Dictionary<string, string>();
            
            foreach (string score in scores)
            {
                string[] scoreParts = score.Split(ScoreLineSeparator);
                if (scoreParts.Length != 2)
                {
                    continue;
                }

                if (!highscoresInFile.TryAdd(scoreParts[0], scoreParts[1]))
                {
                    // There's a duplicate key in the high scores file.
                }
            }

           EditableTimesByPerson = highscoresInFile;
        }

        public async Task SaveToFileAsync(string filePath)
        {
            string ioFilepath = $"{filePath}.new";
            await File.WriteAllTextAsync(ioFilepath, ToString());
            File.Copy(ioFilepath, filePath, true);
            File.Delete(ioFilepath);
        }
    }

    public enum HighScoreAddResult
    {
        Ok = 1,
        DuplicateEntryWithoutOverwrite = 2,
        InvalidCommandString = 3
    }
}