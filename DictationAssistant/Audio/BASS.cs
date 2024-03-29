using System;
using System.Runtime.InteropServices;

namespace DictationAssistant.Audio
{
    public static class BASS
    {
        [DllImport("bass.dll")]
        private static extern int BASS_Init(int device, int freq, int flags, IntPtr win, IntPtr clsid);

        [DllImport("bass.dll")]
        private static extern int BASS_Free();

        private const int BASS_UNICODE = unchecked((int)0x80000000);

        [DllImport("bass.dll", CharSet = CharSet.Unicode)]
        private static extern int BASS_PluginLoad(string file, int flags);

        [DllImport("bass.dll")]
        private static extern int BASS_PluginFree(int handle);

        public static bool Init(int device, int freq, int flags, IntPtr win)
        {
            try
            {
                return BASS_Init(device, freq, flags, win, default(IntPtr)) != 0;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static int LoadPlugin(string file)
        {
            try
            {
                return BASS_PluginLoad(file, BASS_UNICODE);
            }
            catch (Exception)
            {
            }
            return 0;
        }
        public static bool FreePlugin(int handle)
        {
            try
            {
                return BASS_PluginFree(handle) != 0;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool FreeAllPlugin()
        {
            return FreePlugin(0);
        }
        public static bool Free()
        {
            try
            {
                return BASS_Free() != 0;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
