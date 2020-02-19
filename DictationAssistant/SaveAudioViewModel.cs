using DictationAssistant.Audio;
using DictationAssistant.Audio.Encoder;
using DictationAssistant.Lyric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DictationAssistant
{
    public class SaveAudioViewModel : INotifyPropertyChanged
    {
        public enum LyricEncoderType
        {
            Null,
            Lrc
        }

        public static Dictionary<string, byte> CommonChannel { get; }
            = new Dictionary<string, byte>() {
                {"Mono", 1 },
                {"Stereo", 2 }
            };
        public static ReadOnlyCollection<int> CommonFreq { get; }
            = Array.AsReadOnly(new int[] { 6000, 7333, 8000, 11025, 16000, 22050, 24000, 32000, 44100, 48000 });
        public static Dictionary<string, PcmSampleFormat> CommonSampleFormat { get; }
            = new Dictionary<string, PcmSampleFormat>() {
                {"Unsigned 8bit", PcmSampleFormat.U8 },
                {"Signed 16bit", PcmSampleFormat.S16 }
            };
        public static ReadOnlyCollection<AudioEncoderInfo> CommonEncoderInfo { get; }
            = new List<AudioEncoderInfo>() {
                WaveEncoder.EncoderInfo,
                new ExternalAudioEncoderInfo("MPEG Audio Layer 3", "mp3", 
                    System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "encoder", "lame.exe"), "--ta 自动默写 -V0 --ignorelength --quiet - \"{0}\""),
                new ExternalAudioEncoderInfo("Opus Audio", "opus",
                    System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "encoder", "opusenc.exe"), "--artist 自动默写 --ignorelength --quiet - \"{0}\"")
            }.AsReadOnly();
        public static Dictionary<string, LyricEncoderType> CommonLyricType { get; }
            = new Dictionary<string, LyricEncoderType>() {
                {"Dismiss", LyricEncoderType.Null },
                {"Lrc File", LyricEncoderType.Lrc },
            };

        public event PropertyChangedEventHandler PropertyChanged;

        private byte channels = 2;
        private int freq = 44100;
        private PcmSampleFormat sampleFormat = PcmSampleFormat.S16;
        private AudioEncoderInfo encoderInfo = CommonEncoderInfo[1];
        private LyricEncoderType lyricType = LyricEncoderType.Lrc;
        private string targetPath;

        public int Freq
        {
            get
            {
                return freq;
            }
            set
            {
                freq = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Freq)));
            }
        }

        public byte Channels
        {
            get
            {
                return channels;
            }
            set
            {
                channels = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Channels)));
            }
        }

        public PcmSampleFormat SampleFormat
        {
            get
            {
                return sampleFormat;
            }
            set
            {
                sampleFormat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleFormat)));
            }
        }

        public AudioEncoderInfo EncoderInfo
        {
            get
            {
                return encoderInfo;
            }
            set
            {
                encoderInfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EncoderInfo)));
            }
        }

        public LyricEncoderType LyricType
        {
            get
            {
                return lyricType;
            }
            set
            {
                lyricType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LyricType)));
            }
        }

        public string TargetPath
        {
            get
            {
                return targetPath;
            }
            set
            {
                targetPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetPath)));
            }
        }

        public PcmStreamWithInfo CreateEncoder()
        {
            return EncoderInfo.CreateEncoder(new PcmFormatInfo(Freq,Channels, SampleFormat), TargetPath);
        }

        public ILyricWriter CreateLyricWriter()
        {
            switch (LyricType)
            {
                case LyricEncoderType.Null:
                    return null;
                case LyricEncoderType.Lrc:
                    var lyric = new LyricWriter(System.IO.Path.ChangeExtension(TargetPath, "lrc"));
                    lyric.IdTag["ar"] = "自动默写";
                    lyric.IdTag["by"] = "自动默写";
                    return lyric;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
