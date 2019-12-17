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
        public int SessionNumber { get; set; }
        public PomoSessionType CurrentSessionType { get; set; }
        public PomoSessionState CurrentSessionState { get; set; }
        public PomoSessionSettings SessionSettings { get; set; }
        public SessionTimer Timer { get; set; }



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
            SessionNumber = 1;
            Timer = new SessionTimer(SessionSettings.WorkSessionLength);
            Timer.TimerTicked += Timer_TimerTicked;
            Timer.TimerEnded += Timer_TimerEnded;
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
            if (SessionNumber + 1 > SessionSettings.NumberOfSessions)
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
                    if (SessionNumber + 1 == SessionSettings.NumberOfSessions)
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
                CurrentSessionState = _nextSessionCache.NextSessionState;
                StateChanged?.Invoke(this, CurrentSessionState);
                CurrentSessionType = _nextSessionCache.NextSessionType;
                TypeChanged?.Invoke(this, CurrentSessionType);
                Timer.SetTimer(_nextSessionCache.NextSessionLength);
                hasStarted = Timer.StartTimer();
                SessionNumber++;
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
