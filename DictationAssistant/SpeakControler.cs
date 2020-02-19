using DictationAssistant.Audio;
using DictationAssistant.Lyric;
using DictationAssistant.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DictationAssistant
{
    public class SpeakControler : INotifyPropertyChanged
    {
        private System.Timers.Timer TimerForAutoSpeaking;
        private readonly object lockObject = new object(); // 约定：RaiseEvent前必须unlock
        private int ElapsedSeconds = 0; // 已过秒数

        public SpeakControler()
        {
            TimerForAutoSpeaking = new System.Timers.Timer() { Enabled = false, Interval = 1000 };
            TimerForAutoSpeaking.Elapsed += TimerForAutoSpeaking_Elapsed;
        }


        private ISpeaker _speaker;
        public ISpeaker Speaker
        {
            get { return _speaker; }
            set
            {
                _speaker = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speaker)));
            }
        }

        private IWordListAdapter _wordListSource;
        public IWordListAdapter WordListSource
        {
            get { return _wordListSource; }
            set
            {
                _wordListSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WordListSource)));
            }
        }

        private ISpeakStateControler _speakStateControler;
        private ISpeakStateControler SpeakStateControler
        {
            get
            {
                return _speakStateControler;
            }

            set
            {
                lock (lockObject)
                {
                    var old = _speakStateControler;
                    if (old != null)
                        _speakStateControler.PlayCompleted -= SpeakStateControler_PlayCompleted;
                    _speakStateControler = value;
                    if (value != null)
                    {
                        value.PlayCompleted += SpeakStateControler_PlayCompleted;
                    }
                    if ((old == null) ^ (value == null))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSpeaking)));
                    }
                }
            }
        }
        public bool IsSpeaking => SpeakStateControler != null;

        private int _nextWordIndex = 0;
        public int NextWordIndex
        {
            get { return _nextWordIndex; }
            private set
            {
                _nextWordIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextWordIndex)));
            }
        }

        private int _elapsedTimes = 0;
        public int ElapsedTimes
        {
            get
            {
                return _elapsedTimes;
            }
            private set
            {
                _elapsedTimes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ElapsedTimes)));
            }
        }

        public bool AutoMode { get; private set; } = false;

        private IWaitingTimeCalculator _currentWaitingTimeCalculator = new FixedWaitingTime(3);
        public IWaitingTimeCalculator CurrentWaitingTimeCalculator
        {
            get { return _currentWaitingTimeCalculator; }
            set
            {
                _currentWaitingTimeCalculator = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentWaitingTimeCalculator)));
            }
        }

        private int _autoMode_TimesPerWord = 2;

        public int AutoMode_TimesPerWord
        {
            get { return _autoMode_TimesPerWord; }
            set
            {
                _autoMode_TimesPerWord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoMode_TimesPerWord)));
            }
        }

        private bool isPausedAutoMode = false;
        public bool IsPausedAutoMode
        {
            get { return isPausedAutoMode; }
            set
            {
                isPausedAutoMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPausedAutoMode)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        // 警告：任何事件都可能在一个全新的线程被触发！

        public event UpdateWaitingTimeInfoEventHandler UpdateWaitingTimeInfo;

        public delegate void UpdateWaitingTimeInfoEventHandler(int i, int remind);

        private int GetWaitingTime(string word)
        {
            return CurrentWaitingTimeCalculator.CalculateWaitingTime(word);
        }
        private int GetWaitingTime(int i)
        {
            return GetWaitingTime(WordListSource[i]);
        }
        private void TimerForAutoSpeaking_Elapsed(object sender, EventArgs e)
        {
            bool needToWait = false;
            bool completedAuto = false;
            int waitToSpeakWord = 0;
            int remind = 0;
            var numberOfWords = WordListSource.Length;
            int localNextWordIndex;
            lock (lockObject)
                localNextWordIndex = NextWordIndex;
            var waitingTime = GetWaitingTime(localNextWordIndex - 1);
            lock (lockObject)
            {
                ElapsedSeconds += 1; // 加一秒
                if (localNextWordIndex <= numberOfWords)
                {
                    remind = waitingTime - ElapsedSeconds;
                    needToWait = remind > 0;
                    if (ElapsedTimes >= AutoMode_TimesPerWord)
                    {
                        waitToSpeakWord = localNextWordIndex;
                        if (needToWait == false && localNextWordIndex == numberOfWords)
                            completedAuto = true;
                    }
                    else
                        waitToSpeakWord = localNextWordIndex - 1;
                }
                else
                    completedAuto = true;
                if (completedAuto)
                {
                    AutoMode = false;
                    TimerForAutoSpeaking.Stop();
                }
            }
            if (completedAuto)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoMode)));
            }
            else
            {
                TimerForAutoSpeaking.Start();
                if (needToWait)
                    UpdateWaitingTimeInfo?.Invoke(waitToSpeakWord, remind);
                else
                    Speak(waitToSpeakWord);
            }
        }

        public void Speak(int Index)
        {
            ISpeakStateControler LocalSpeakStateControler;
            string word = WordListSource[Index];
            lock (lockObject)
            {
                TimerForAutoSpeaking.Stop();
                ElapsedSeconds = 0;
                if (SpeakStateControler != null)
                {
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
                if (NextWordIndex != Index + 1)
                {
                    ElapsedTimes = 0;
                    NextWordIndex = Index + 1;
                }
                if (IsPausedAutoMode)
                    IsPausedAutoMode = false;
                LocalSpeakStateControler = Speaker.Speak(word);
            }
            SpeakStateControler = LocalSpeakStateControler;
        }
        private void SpeakStateControler_PlayCompleted(object sender, EventArgs e)
        {
            var localAutoMode = false;
            lock (lockObject)
            {
                localAutoMode = AutoMode;
                SpeakStateControler = null;
                ElapsedTimes += 1;
            }
            if (localAutoMode)
                TimerForAutoSpeaking_Elapsed(sender, e);
        }

        /// <summary>
        /// 移动位置到第一个前
        /// </summary>
        /// <remarks></remarks>
        public void ResetProgress()
        {
            lock (lockObject)
            {
                NextWordIndex = 0;
                ElapsedTimes = 0;
            }
        }
        /// <summary>
        /// 播报下一个并移动位置
        /// </summary>
        /// <remarks></remarks>
        public void SpeakNext()
        {
            int index;
            lock (lockObject)
                index = NextWordIndex;
            Speak(index);
        }
        /// <summary>
        /// 播报当前词语
        /// </summary>
        /// <remarks></remarks>
        public void SpeakAgain()
        {
            int index;
            lock (lockObject)
                index = NextWordIndex - 1;
            Speak(index);
        }
        /// <summary>
        /// 播报上一个并移动位置
        /// </summary>
        /// <remarks></remarks>
        public void SpeakPrevious()
        {
            int index;
            lock (lockObject)
                index = NextWordIndex - 2;
            Speak(index);
        }

        public void StartAuto(int index = 0)
        {
            lock (lockObject)
            {
                if (IsPausedAutoMode)
                    IsPausedAutoMode = false;
                AutoMode = true;
                ElapsedSeconds = 0;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoMode)));
            Speak(index);
        }
        public void StopAuto()
        {
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                if (IsPausedAutoMode)
                    IsPausedAutoMode = false;
                AutoMode = false;
                TimerForAutoSpeaking.Stop();
                if (SpeakStateControler != null)
                {
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoMode)));
        }

        public void PauseAuto()
        {
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                TimerForAutoSpeaking.Stop();
                if (SpeakStateControler != null)
                {
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
                IsPausedAutoMode = true;
            }
        }

        public void ResumeAuto()
        {
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                TimerForAutoSpeaking.Start();
                IsPausedAutoMode = false;
            }
        }

        public void StopThis()
        {
            lock (lockObject)
            {
                if (SpeakStateControler != null)
                {
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
            }
        }

        public bool SaveAudio(
            IVoice voice,
            SpeakParam speakParam,
            PcmWriter pcmWriter,
            ILyricWriter lrcWriter,
            Action<double> reportProgress = null,
            Func<bool> isCancellationPending = null)
        {
            bool complete = true;

            reportProgress?.Invoke(0);
            long byteOffest = 0;
            int length = WordListSource.Length;
            string[] wordList = new string[length];
            for (int i = 0; i < length; i++)
            {
                wordList[i] = WordListSource[i];
            }

            PcmStreamWithInfo speech;
            for (int iWord = 0; iWord < length; iWord++)
            {
                if (isCancellationPending != null && isCancellationPending())
                {
                    complete = false;
                    break;
                }
                reportProgress?.Invoke((double)iWord / length);
                string word = wordList[iWord];

                if (AutoMode_TimesPerWord >= 1)
                {
                    speech = voice.Speak(word, speakParam);
                    lrcWriter?.Append(word, pcmWriter.BytesToMilliseconds(byteOffest));
                    byteOffest += pcmWriter.Write(speech);
                    byteOffest += pcmWriter.WriteDelay(GetWaitingTime(word) * 1000);
                    for (var i = 2; i <= AutoMode_TimesPerWord; i++)
                    {
                        if (speech.PcmStream.CanSeek)
                        {
                            speech.PcmStream.Seek(0, SeekOrigin.Begin);
                        }
                        else
                        {
                            speech.Dispose();
                            speech = voice.Speak(word, speakParam);
                        }
                        lrcWriter?.Append(word, pcmWriter.BytesToMilliseconds(byteOffest));
                        byteOffest += pcmWriter.Write(speech);
                        byteOffest += pcmWriter.WriteDelay(GetWaitingTime(word) * 1000);
                    }
                    speech.Dispose();
                }

                lrcWriter?.Append(" ", pcmWriter.BytesToMilliseconds(byteOffest) - 10);
            }
            lrcWriter?.Append("本文件由 自动默写 程序自动生成", pcmWriter.BytesToMilliseconds(byteOffest));
            reportProgress?.Invoke(1);

            return complete;
        }
    }
}
