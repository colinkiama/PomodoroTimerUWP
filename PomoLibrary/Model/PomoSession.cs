using PomoLibrary.Enums;
using PomoLibrary.Services;
using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Model
{
    public class PomoSession
    {
        private NextSessionData _nextSessionCache;
        private SessionTimer _timer;

        public int SessionsCompleted { get; set; }
        public PomoSessionType CurrentSessionType { get; set; }
        public PomoSessionState CurrentSessionState { get; set; }
        public PomoSessionSettings SessionSettings { get; set; }
        public SessionTimer Timer
        {
            get => _timer;
            set
            {
                // Workaround to listen to timer events from a loaded SessionTimer
                _timer = value;
                ListenToTimerEvents(_timer);
            }
        }

        private void ListenToTimerEvents(SessionTimer timer)
        {
            timer.TimerTicked += Timer_TimerTicked;
            timer.TimerEnded += Timer_TimerEnded;
        }

        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler SessionCompleted;
        public event EventHandler<PomoSessionState> StateChanged;
        public event EventHandler<PomoSessionType> TypeChanged;

        public PomoSession()
        {
            var loadedSessionSettings = SettingsService.Instance.SessionSettings;
            SessionSettings = loadedSessionSettings;
            CurrentSessionState = PomoSessionState.Stopped;
            CurrentSessionType = PomoSessionType.Work;
            SessionsCompleted = 0;
            _timer = new SessionTimer(SessionSettings.WorkSessionLength);
            ListenToTimerEvents(_timer);
        }



        private void Timer_TimerEnded(object sender, EventArgs e)
        {
            StopSession();
            SessionCompleted?.Invoke(sender, e);
        }

        private void Timer_TimerTicked(object sender, TimeSpan e)
        {
            TimerTicked?.Invoke(sender, e);
        }

        internal NextSessionData GetNextSessionData()
        {
            var nextSessionType = DetermineNextSessionType();
            var nextSessionState = DetermineNextSessionState();
            TimeSpan nextSessionLength = DetermineNextSessionLength(nextSessionState, nextSessionType);
            _nextSessionCache = new NextSessionData { NextSessionType = nextSessionType, NextSessionState = nextSessionState, NextSessionLength = nextSessionLength };
            return _nextSessionCache;
        }

        private TimeSpan DetermineNextSessionLength(PomoSessionState nextSessionState, PomoSessionType nextSessionType)
        {
            TimeSpan lengthToReturn = TimeSpan.FromMilliseconds(0);
            if (nextSessionState != PomoSessionState.Stopped)
            {
                switch (nextSessionType)
                {
                    case PomoSessionType.Work:
                        lengthToReturn = TimeSpan.FromMilliseconds(SessionSettings.WorkSessionLength.TimeInMilliseconds);
                        break;
                    case PomoSessionType.Break:
                        lengthToReturn = TimeSpan.FromMilliseconds(SessionSettings.BreakSessionLength.TimeInMilliseconds);
                        break;
                    case PomoSessionType.LongBreak:
                        lengthToReturn = TimeSpan.FromMilliseconds(SessionSettings.LongBreakSessionLength.TimeInMilliseconds);
                        break;
                }
            }
            return lengthToReturn;
        }

        private PomoSessionState DetermineNextSessionState()
        {
            PomoSessionState stateToReturn = PomoSessionState.InProgress;
            if (SessionsCompleted == SessionSettings.NumberOfSessions)
            {
                stateToReturn = PomoSessionState.Stopped;
            }

            return stateToReturn;
        }

        private PomoSessionType DetermineNextSessionType()
        {
            PomoSessionType sessionTypeToReturn = PomoSessionType.Work;
            switch (CurrentSessionType)
            {
                case PomoSessionType.Work:
                    if (SessionsCompleted + 1 == SessionSettings.NumberOfSessions)
                    {
                        sessionTypeToReturn = PomoSessionType.LongBreak;
                    }
                    else
                    {
                        sessionTypeToReturn = PomoSessionType.Break;
                    }
                    break;
                case PomoSessionType.Break:
                    sessionTypeToReturn = PomoSessionType.Work;
                    break;
                case PomoSessionType.LongBreak:
                    sessionTypeToReturn = PomoSessionType.Work;
                    break;
            }

            return sessionTypeToReturn;
        }

        internal bool StartSession()
        {
            bool hasStarted = false;

            if (_nextSessionCache.NextSessionLength != TimeSpan.FromMilliseconds(0))
            {
                // Only increments the session number if the last session was a work session
                if (CurrentSessionType == PomoSessionType.Work)
                {
                    SessionsCompleted++;
                }
                CurrentSessionState = _nextSessionCache.NextSessionState;
                StateChanged?.Invoke(this, CurrentSessionState);
                CurrentSessionType = _nextSessionCache.NextSessionType;
                TypeChanged?.Invoke(this, CurrentSessionType);
                Timer.SetTimer(_nextSessionCache.NextSessionLength);
                hasStarted = Timer.StartTimer();
                ClearNextSessionCache();
            }
            else
            {
                hasStarted = Timer.StartTimer();
                if (hasStarted)
                {
                    CurrentSessionState = PomoSessionState.InProgress;
                    StateChanged?.Invoke(this, CurrentSessionState);
                }
            }

            return hasStarted;
        }

        internal void TimerCatchup()
        {
            Timer.CatchUp();
        }

        private void ClearNextSessionCache()
        {
            _nextSessionCache = default;
        }

        public void PauseSession()
        {
            Timer.StopTimer();
            CurrentSessionState = PomoSessionState.Paused;
            StateChanged?.Invoke(this, CurrentSessionState);
        }

        public void StopSession()
        {
            Timer.StopTimer();
            CurrentSessionState = PomoSessionState.Stopped;
            StateChanged?.Invoke(this, CurrentSessionState);
        }

        public bool ResumeSession()
        {
            bool hasStarted = Timer.StartTimer();
            if (hasStarted)
            {
                CurrentSessionState = PomoSessionState.InProgress;
                StateChanged?.Invoke(this, CurrentSessionState);
            }
            return hasStarted;
        }
    }
}
