using DictationAssistant.Audio;
using DictationAssistant.Audio.Decoder;
using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.IO;

namespace DictationAssistant.Speech
{
    public class ImprovedVoice : IVoice
    {
        private static readonly string[] AudioFileExtensions = "wav;flac;ape;m4a;opus;aac;mp3;mp2;mp1;ogg;wma;aif;mp4".Split(';');

        private IVoice VoiceForNoFile;
        private string ResForImproving;
        public ImprovedVoice(IVoice voiceForNoFile, string resForImproving)
        {
            this.VoiceForNoFile = voiceForNoFile;
            this.ResForImproving = resForImproving;
        }

        public string Name
        {
            get
            {
                return "[Improved] " + VoiceForNoFile.Name;
            }
        }
        public CultureInfo Culture
        {
            get
            {
                return VoiceForNoFile.Culture;
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
                if (VoiceForNoFile == null)
                    return PcmStreamWithInfo.Empty;
                return VoiceForNoFile.Speak(text, param);
            }
        }
    }
}
