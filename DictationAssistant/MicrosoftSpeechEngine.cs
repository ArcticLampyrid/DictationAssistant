using System.Collections.Generic;
using System;
using SpeechLib;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace DictationAssistant
{
    public class MicrosoftSpeechEngine : ISpeakEngine
    {
        private static ReadOnlyCollection<MicrosoftSpeechEngineInfo> AllNativeEngines;
        static MicrosoftSpeechEngine()
        {
            var NativeEngineList = new List<MicrosoftSpeechEngineInfo>();
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
                        var engine = ObjectTokens.Item(i);
                        string engineName = engine.GetAttribute("name");
                        CultureInfo engineCulture = new CultureInfo(int.Parse(engine.GetAttribute("Language").Split(';')[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture), false);
                        NativeEngineList.Add(new MicrosoftSpeechEngineInfo(engineName, engineCulture, engine.Id));
                        i += 1;
                    }
                }
                catch (Exception)
                {
                }
            }
            AllNativeEngines = NativeEngineList.AsReadOnly();
        }
        public static ReadOnlyCollection<MicrosoftSpeechEngineInfo> GetAllNativeEngines()
        {
            return AllNativeEngines;
        }


        public CultureInfo Culture => NativeEngineInfo.Culture;
        public string Name => NativeEngineInfo.Name;
        private readonly MicrosoftSpeechEngineInfo NativeEngineInfo;
        public MicrosoftSpeechEngine(MicrosoftSpeechEngineInfo nativeEngineInfo)
        {
            NativeEngineInfo = nativeEngineInfo;
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
                var engine = new SpObjectToken();
                engine.SetId(NativeEngineInfo.TokenId);
                voice.Voice = engine;
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
