﻿using PomoLibrary.Enums;
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
        public int SessionNumber { get; set; }
        public PomoSessionType CurrentSesionType { get; set; }
        public PomoSessionState CurrentSessionState { get; set; }
        public PomoSessionSettings SessionSettings { get; set; }
        public SessionTimer Timer { get; set; }


        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler SessionCompleted;
        public event EventHandler<PomoSessionState> StateChanged;

        public PomoSession()
        {
            var loadedSessionSettings = SettingsService.Instance.SessionSettings;
            SessionSettings = loadedSessionSettings;
            CurrentSessionState = PomoSessionState.Stopped;
            CurrentSesionType = PomoSessionType.Work;
            Timer = new SessionTimer(SessionSettings.WorkSessionLength);
            SessionNumber = 1;
        }


        internal void GetNextSessionData()
        {
            var nextSessionType = DetermineNextSessionType();
            var nextSessionState = DetermineNextSessionState();
            TimeSpan sessionLength = DetermineNextSessionLength(nextSessionState, nextSessionType);
        }

        private TimeSpan DetermineNextSessionLength(PomoSessionState nextSessionState, PomoSessionType nextSessionType)
        {
            TimeSpan lengthToReturn = TimeSpan.FromMilliseconds(0);
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
            switch (CurrentSesionType)
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
            throw new NotImplementedException();
        }

        internal void PauseSession()
        {
            throw new NotImplementedException();
        }

        internal void StopSession()
        {
            throw new NotImplementedException();
        }
    }
}
