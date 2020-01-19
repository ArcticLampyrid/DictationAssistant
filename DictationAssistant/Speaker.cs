using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections;
using System;
using DictationAssistant;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DictationAssistant
{
    public class Speaker : ISpeaker
    {
        public byte Volume { get; set; }
        public sbyte Rate { get; set; }
        public ISpeakEngine Engine { get; set; }
        public ISpeakStateControler Speak(string text)
        {
            var pcm = Engine.Speak(text, new SpeakParam() { Rate = Rate });
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
