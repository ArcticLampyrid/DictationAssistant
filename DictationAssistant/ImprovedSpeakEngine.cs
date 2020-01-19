using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.IO;

namespace DictationAssistant
{
    public class ImprovedSpeakEngine : ISpeakEngine
    {
        private static readonly string[] AudioFileExtensions = "wav;flac;ape;m4a;opus;acc;mp3;mp2;mp1;ogg;wma;aif;mp4".Split(';');

        private ISpeakEngine EngineForNoFile;
        private string ResForImproving;
        public ImprovedSpeakEngine(ISpeakEngine engineForNoFile, string resForImproving)
        {
            this.EngineForNoFile = engineForNoFile;
            this.ResForImproving = resForImproving;
        }

        public string Name
        {
            get
            {
                return "[Improved] " + EngineForNoFile.Name;
            }
        }
        public CultureInfo Culture
        {
            get
            {
                return EngineForNoFile.Culture;
            }
        }


        private string FindFile(string text)
        {
            string filePath;
            filePath = text;
            filePath = filePath.Replace(@"\", "");
            filePath = filePath.Replace("/", "");
            filePath = filePath.Replace(":", "");
            filePath = filePath.Replace("*", "");
            filePath = filePath.Replace("?", "");
            filePath = filePath.Replace("\"", "");
            filePath = filePath.Replace("<", "");
            filePath = filePath.Replace(">", "");
            filePath = filePath.Replace("|", "");
            filePath = Path.Combine(ResForImproving, filePath + ".");
            foreach (var extension in AudioFileExtensions)
            {
                if (File.Exists(filePath + extension))
                    return filePath + extension;
            }
            throw new FileNotFoundException($"未找到词组{text}对应的音频文件");
        }
        public PcmStreamWithInfo Speak(string text, SpeakParam param)
        {
            try
            {
                return AudioFileDecodeStream.Create(FindFile(text));
            }
            catch (Exception)
            {
                if (EngineForNoFile == null)
                    return PcmStreamWithInfo.Empty;
                return EngineForNoFile.Speak(text, param);
            }
        }
    }
}
