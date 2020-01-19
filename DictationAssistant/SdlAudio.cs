using System.Runtime.InteropServices;

namespace DictationAssistant
{
    public class SdlAudio
    {
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_Init(uint flags);

        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int SDL_SetHintWithPriority(string name, string value, int priority);

        private const uint SDL_INIT_AUDIO = 0x10U;
        static SdlAudio()
        {
            SDL_SetHintWithPriority("SDL_WINDOWS_DISABLE_THREAD_NAMING", "1", 2);
            SDL_Init(SDL_INIT_AUDIO);
        }
        public static void Use()
        {
            return;
        }
    }
}
