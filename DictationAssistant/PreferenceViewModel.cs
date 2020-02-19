using DictationAssistant.Speech;
using QIQI.WpfFontPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DictationAssistant
{
    public class PreferenceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
		private PackedFontInfo editorFontInfo;
		private string defaultChineseVoiceName;
		private string defaultEnglishVoiceName;
		private string pathForImprovedResource;
		private ICollection<IVoice> voices;

		public PackedFontInfo EditorFontInfo
		{
			get 
			{ 
				return editorFontInfo;
			}
			set 
			{ 
				editorFontInfo = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditorFontInfo)));
			}
		}

		public string DefaultChineseVoiceName
		{
			get
			{
				return defaultChineseVoiceName;
			}
			set
			{
				defaultChineseVoiceName = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultChineseVoiceName)));
			}
		}

		public string DefaultEnglishVoiceName
		{
			get
			{
				return defaultEnglishVoiceName;
			}
			set
			{
				defaultEnglishVoiceName = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultEnglishVoiceName)));
			}
		}

		public string PathForImprovedResource
		{
			get
			{
				return pathForImprovedResource;
			}
			set
			{
				pathForImprovedResource = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathForImprovedResource)));
			}
		}

		public ICollection<IVoice> Voices
		{
			get
			{
				return voices;
			}
			set
			{
				voices = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Voices)));
			}
		}
	}
}
