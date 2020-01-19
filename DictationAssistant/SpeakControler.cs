using System;
using System.Runtime.CompilerServices;

namespace DictationAssistant
{
    public class SpeakControler
    {
        public SpeakControler()
        {
            TimerForAutoSpeaking = new System.Timers.Timer() { Enabled = false, Interval = 1000 };
            TimerForAutoSpeaking.Elapsed += TimerForAutoSpeaking_Elapsed;
        }

        private readonly object lockObject = new object(); // 约定：RaiseEvent前必须unlock

        public ISpeaker Speaker { get; set; }
        public IWordListAdapter WordListSource { get; set; }
        private ISpeakStateControler _SpeakStateControler;

        private ISpeakStateControler SpeakStateControler
        {
            get
            {
                lock (lockObject)
                {
                    return _SpeakStateControler;
                }
            }

            set
            {
                lock (lockObject)
                {
                    if (_SpeakStateControler != null)
                        _SpeakStateControler.PlayCompleted -= SpeakStateControler_PlayCompleted;
                    _SpeakStateControler = value;
                    if (_SpeakStateControler != null)
                    {
                        _SpeakStateControler.PlayCompleted += SpeakStateControler_PlayCompleted;
                    }
                }
            }
        }

        public int NextWordIndex { get; private set; } = 0; // 下一个播报词语索引
        private int ElapsedSeconds = 0; // 已过秒数
        public int ElapsedTimes { get; private set; } = 0; // 已报次数

        private bool AutoMode = false; // （是否）自动播报

        public IWaitingTimeCalculator CurrentWaitingTimeCalculator { get; set; } = new FixedWaitingTime(3);
        public int AutoMode_TimesPerWord { get; set; } = 2; // 每词遍数

        private System.Timers.Timer TimerForAutoSpeaking;


        // 警告：任何事件都可能在一个全新的线程被触发！


        public event AutoSpeakingCompletedEventHandler AutoSpeakingCompleted;

        public delegate void AutoSpeakingCompletedEventHandler(bool completed);

        public event UpdateWaitingTimeInfoEventHandler UpdateWaitingTimeInfo;

        public delegate void UpdateWaitingTimeInfoEventHandler(int i, int remind);

        public event SpeakingEventHandler Speaking;

        public delegate void SpeakingEventHandler(int i);

        public event StoppedThisEventHandler StoppedThis;

        public delegate void StoppedThisEventHandler(int i, bool completed);

        private int GetWaitingTime(int i)
        {
            return CurrentWaitingTimeCalculator.CalculateWaitingTime(WordListSource[i]);
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
                ElapsedSeconds = ElapsedSeconds + 1; // 加一秒
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
                AutoSpeakingCompleted?.Invoke(true);
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
            bool raiseStoppedThis = false;
            int lastIndex = -1;
            string word = WordListSource[Index];
            lock (lockObject)
            {
                TimerForAutoSpeaking.Stop();
                ElapsedSeconds = 0;
                if (SpeakStateControler != null)
                {
                    raiseStoppedThis = true;
                    lastIndex = NextWordIndex - 1;
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
                if (NextWordIndex != Index + 1)
                {
                    ElapsedTimes = 0;
                    NextWordIndex = Index + 1;
                }
                LocalSpeakStateControler = Speaker.Speak(word);
            }
            if (raiseStoppedThis)
                StoppedThis?.Invoke(lastIndex, false);
            Speaking?.Invoke(Index);
            SpeakStateControler = LocalSpeakStateControler;
        }
        private void SpeakStateControler_PlayCompleted(object sender, EventArgs e)
        {
            var localAutoMode = false;
            int lastIndex = -1;
            lock (lockObject)
            {
                localAutoMode = AutoMode;
                lastIndex = NextWordIndex - 1;
                SpeakStateControler = null;
                ElapsedTimes += 1;
            }

            StoppedThis?.Invoke(lastIndex, true);
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
        public void SpeakLast()
        {
            int index;
            lock (lockObject)
                index = NextWordIndex - 2;
            Speak(index);
        }
        public bool IsAutoMode
        {
            get
            {
                lock (lockObject)
                    return AutoMode;
            }
        }
        public bool IsSpeaking
        {
            get
            {
                lock (lockObject)
                    return SpeakStateControler != null;
            }
        }

        public void StartAuto(int index = 0)
        {
            lock (lockObject)
            {
                AutoMode = true;
                ElapsedSeconds = 0;
            }
            Speak(index);
        }
        public void StopAuto()
        {
            bool raiseStoppedThis = false;
            int lastIndex = -1;
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                AutoMode = false;
                TimerForAutoSpeaking.Stop();
                if (SpeakStateControler != null)
                {
                    raiseStoppedThis = true;
                    lastIndex = NextWordIndex - 1;
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
            }
            if (raiseStoppedThis)
                StoppedThis?.Invoke(lastIndex, false);
            AutoSpeakingCompleted?.Invoke(false);
        }
        public void PauseAuto()
        {
            bool raiseStoppedThis = false;
            int lastIndex = -1;
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                TimerForAutoSpeaking.Stop();
                if (SpeakStateControler != null)
                {
                    raiseStoppedThis = true;
                    lastIndex = NextWordIndex - 1;
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
            }
            if (raiseStoppedThis)
                StoppedThis?.Invoke(lastIndex, false);
        }
        public void ResumeAuto()
        {
            lock (lockObject)
            {
                if (AutoMode == false)
                    return;
                TimerForAutoSpeaking.Start();
            }
        }

        public void StopThis()
        {
            bool raiseStoppedThis = false;
            int lastIndex = -1;
            lock (lockObject)
            {
                if (SpeakStateControler != null)
                {
                    raiseStoppedThis = true;
                    lastIndex = NextWordIndex - 1;
                    SpeakStateControler.StopSpeak();
                    SpeakStateControler = null;
                }
            }
            if (raiseStoppedThis)
                StoppedThis?.Invoke(lastIndex, false);
        }
    }
}
