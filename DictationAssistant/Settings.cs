using QIQI.WpfFontPicker;
using System.Configuration;

namespace DictationAssistant.Properties {
    internal sealed partial class Settings {
        public Settings() {

        }


        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public PackedFontInfo FontInfo
        {
            get => (PackedFontInfo)this[nameof(FontInfo)];
            set => this[nameof(FontInfo)] = value;
        }
    }
}
