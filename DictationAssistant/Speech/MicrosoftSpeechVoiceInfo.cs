using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DictationAssistant.Speech
{
    public class MicrosoftSpeechVoiceInfo
    {
        public MicrosoftSpeechVoiceInfo(string name, CultureInfo culture, string tokenId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            TokenId = tokenId ?? throw new ArgumentNullException(nameof(tokenId));
        }

        public string Name { get; }
        public CultureInfo Culture { get; }
        public string TokenId { get; }
    }
}
