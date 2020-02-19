using DictationAssistant.Audio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace DictationAssistant
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            BASS.FreeAllPlugin();
            BASS.Free();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (BASS.Init(0, 44100, 0, IntPtr.Zero))
            {
                try
                {
                    DirectoryInfo pluginDir = new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "bass_plugin");
                    foreach (var File in pluginDir.GetFiles())
                        BASS.LoadPlugin(File.FullName);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
