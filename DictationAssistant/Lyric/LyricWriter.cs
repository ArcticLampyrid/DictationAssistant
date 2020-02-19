using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DictationAssistant.Lyric
{
    public class LyricWriter : ILyricWriter
    {
        
        private readonly Dictionary<string, Dictionary<long, object>> Lyric = new Dictionary<string, Dictionary<long, object>>();
        public Dictionary<string, string> IdTag { get; } = new Dictionary<string, string>();
        public string FileName { get; }

        public LyricWriter(string fileName)
        {
            FileName = fileName;
        }


        public void Append(string text, long offest)
        {
            if (!Lyric.ContainsKey(text))
                Lyric.Add(text, new Dictionary<long, object>());
            Lyric[text].Add(offest, null);
        }
        public void Flush()
        {
            using (StreamWriter writer = new StreamWriter(File.Open(FileName, FileMode.Create), Encoding.Default))
            {
                foreach (var pair in IdTag)
                    writer.WriteLine($"[{pair.Key}:{pair.Value}]");
                foreach (var pair in Lyric)
                {
                    foreach (var offest in pair.Value.Keys)
                    {
                        var min = offest / 60000;
                        var sec = (offest % 60000) / (double)1000;
                        writer.Write($"[{min.ToString("D2")}:{sec.ToString("00.00")}]");
                    }
                    writer.WriteLine(pair.Key);
                }
            }
        }
    }
}
