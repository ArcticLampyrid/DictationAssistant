using System.Collections.Generic;
using System;
using System.Threading;
using DictationAssistant.Audio;

namespace DictationAssistant.Speech
{
    public class Speaker : ISpeaker
    {
        public byte Volume { get; set; }
        public sbyte Rate { get; set; }
        public IVoice Voice { get; set; }
        public ISpeakStateControler Speak(string text)
        {
            PcmStreamWithInfo pcm;
            try
            {
                pcm = Voice.Speak(text, new SpeakParam() { Rate = Rate });
            }
            catch (Exception)
            {
                pcm = PcmStreamWithInfo.Empty;
            }
            var player = new PcmPlayer(pcm, Volume);
            player.Play();
            return new SpeakStateControler(player);
        }

        private class SpeakStateControler : ISpeakStateControler
        {
            private readonly object lockObject = new object();
            private PcmPlayer PcmPlayer;

            public SpeakStateControler(PcmPlayer pcmPlayer)
            {
                this.PcmPlayer = pcmPlayer;
                pcmPlayer.PlayCompleted += PcmPlayer_PlayCompleted;
            }

            private List<EventHandler> PlayCompletedEventHandlerList = new List<EventHandler>();
            public event EventHandler PlayCompleted
            {
                add
                {
                    bool invokeNow = false;
                    lock (lockObject)
                    {
                        PlayCompletedEventHandlerList.Add(value);
                        invokeNow = PcmPlayer.State == PcmPlayer.AudioPlayState.Stopped;
                    }
                    if (invokeNow) {
                        Thread t = new Thread(() =>
                        {
                            value.Invoke(this, EventArgs.Empty);
                        });
                        t.Start();
                    }
                }
                remove
                {
                    lock (lockObject)
                        PlayCompletedEventHandlerList.Remove(value);
                }
            }
            void OnPlayCompleted()
            {
                EventHandler[] tempList;
                lock (lockObject)
                {
                    tempList = new EventHandler[PlayCompletedEventHandlerList.Count];
                    PlayCompletedEventHandlerList.CopyTo(tempList);
                }
                foreach (EventHandler handler in tempList)
                    handler.Invoke(this, EventArgs.Empty);
            }
            private void PcmPlayer_PlayCompleted(object sender, EventArgs e)
            {
                OnPlayCompleted();
            }
            public void StopSpeak()
            {
                PcmPlayer.Dispose();
            }
        }
    }
}
