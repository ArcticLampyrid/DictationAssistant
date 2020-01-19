using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DictationAssistant
{
    public class LyricWriter
    {
        private Dictionary<string, Dictionary<long, object>> Lyric = new Dictionary<string, Dictionary<long, object>>();
        public readonly Dictionary<string, string> IdTag = new Dictionary<string, string>();
        public void Append(string text, long offest)
        {
            if (!Lyric.ContainsKey(text))
                Lyric.Add(text, new Dictionary<long, object>());
            Lyric[text].Add(offest, null);
        }
        public void WriteTo(Stream output)
        {
            using (StreamWriter writer = new StreamWriter(output, Encoding.Default))
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
