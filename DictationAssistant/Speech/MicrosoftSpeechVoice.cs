using System.Collections.Generic;
using System;
using SpeechLib;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using DictationAssistant.Audio;

namespace DictationAssistant.Speech
{
    public class MicrosoftSpeechVoice : IVoice
    {
        public static ReadOnlyCollection<MicrosoftSpeechVoiceInfo> AllNativeVoices { get; }
        static MicrosoftSpeechVoice()
        {
            var NativeVoiceList = new List<MicrosoftSpeechVoiceInfo>();
            foreach (var categoryId in new[] {
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices" })
            {
                try
                {
                    SpObjectTokenCategory ObjectTokenCategory = new SpObjectTokenCategory();
                    ObjectTokenCategory.SetId(categoryId, false);
                    ISpeechObjectTokens ObjectTokens = ObjectTokenCategory.EnumerateTokens();
                    int i = 0;
                    int Count = ObjectTokens.Count;
                    while (i < Count)
                    {
                        var voice = ObjectTokens.Item(i);
                        string voiceName = voice.GetAttribute("name");
                        CultureInfo voiceCulture = new CultureInfo(int.Parse(voice.GetAttribute("Language").Split(';')[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture), false);
                        NativeVoiceList.Add(new MicrosoftSpeechVoiceInfo(voiceName, voiceCulture, voice.Id));
                        i += 1;
                    }
                }
                catch (Exception)
                {
                }
            }
            AllNativeVoices = NativeVoiceList.AsReadOnly();
        }


        public CultureInfo Culture => NativeVoiceInfo.Culture;
        public string Name => NativeVoiceInfo.Name;
        private readonly MicrosoftSpeechVoiceInfo NativeVoiceInfo;
        public MicrosoftSpeechVoice(MicrosoftSpeechVoiceInfo nativeVoiceInfo)
        {
            NativeVoiceInfo = nativeVoiceInfo;
        }
        public PcmStreamWithInfo Speak(string text, SpeakParam param)
        {
            byte[] data;
            PcmFormatInfo pcmFormat = new PcmFormatInfo(44100, 2, PcmSampleFormat.S16);
            {
                SpVoice voice = new SpVoice();
                SpMemoryStream sps = new SpMemoryStream();
                sps.Format.Type = SpeechAudioFormatType.SAFT44kHz16BitStereo;
                voice.AudioOutputStream = sps;
                //注：在某些特殊坏境（如XP），必须重新创建ObjectToken对象，才能在多线程等环境下避免未知错误
                var voiceToken = new SpObjectToken();
                voiceToken.SetId(NativeVoiceInfo.TokenId);
                voice.Voice = voiceToken;
                voice.Rate = param.Rate;
                voice.Speak(text, SpeechVoiceSpeakFlags.SVSFDefault);
                data = sps.GetData() as byte[];
                voice.Voice = null;
                sps = null;
                voice = null;
            }

            var bytes_200ms = (pcmFormat.Freq / 5) * pcmFormat.BlockAlign;
            int i;

            i = 0;
            while (i < data.Length)
            {
                if (!PcmSampleFormatHelper.IsEmptyAudio(pcmFormat.SampleFormat, data, i, pcmFormat.BlockAlign))
                    break;
                i += pcmFormat.BlockAlign;
            }
            if (i > bytes_200ms)
            {
                var start = i - bytes_200ms;
                var length = data.Length - start;
                byte[] temp = new byte[length];
                Array.Copy(data, start, temp, 0, length);
                data = temp;
            }

            i = data.Length;
            while (i > 0)
            {
                if (!PcmSampleFormatHelper.IsEmptyAudio(pcmFormat.SampleFormat, data, i - pcmFormat.BlockAlign, pcmFormat.BlockAlign))
                    break;
                i -= pcmFormat.BlockAlign;
            }
            if (data.Length - i > bytes_200ms)
            {
                var length = i + bytes_200ms;
                byte[] temp = new byte[length];
                Array.Copy(data, 0, temp, 0, length);
                data = temp;
            }

            return new PcmStreamWithInfo(new MemoryStream(data, false), pcmFormat);
        }
    }
}
