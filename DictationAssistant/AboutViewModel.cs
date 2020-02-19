using System.Reflection;

namespace DictationAssistant
{
    public class AboutViewModel
    {
        public string Version { get; }
        public string Title { get; }
        public string CopyrightNotice => Properties.Resources.CopyrightNotice;
        public AboutViewModel()
        {

            Assembly assembly = Assembly.GetEntryAssembly();
            Version = assembly.GetName().Version.ToString();
            Title = assembly.GetName().Name;
        }
    }
}
