using PomoLibrary.Dialogs;
using PomoLibrary.Enums;
using PomoLibrary.Helpers;
using PomoLibrary.Model;
using PomoLibrary.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PomoLibrary.ViewModel
{
    class MainViewModel : Notifier
    {

        private TimeSpan _sessionLength;

        private TimeSpan _currentSessionTime;

        public TimeSpan CurrentSessionTime
        {
            get { return _currentSessionTime; }
            set
            {
                _currentSessionTime = value;
                NotifyPropertyChanged();
            }
        }

        private PomoSessionState _currentSessionState;

        public PomoSessionState CurrentSessionState
        {
            get { return _currentSessionState; }
            set
            {
                _currentSessionState = value;
                SessionService.Instance.UpdateSessionState(_currentSessionState);
                NotifyPropertyChanged();
            }
        }

        private PomoSession _session;

        public PomoSession CurrentSession
        {
            get { return _session; }
            set
            {
                _session = value;
                NotifyPropertyChanged();
            }
        }

        private string _sessionText;

        public string SessionText
        {
            get { return _sessionText; }
            set
            {
                if (_sessionText != value)
                {
                    _sessionText = value;
                    NotifyPropertyChanged();

                }
            }
        }

      
        private double _sessionInverseProgress;

        public double SessionInverseProgress
        {
            get { return _sessionInverseProgress; }
            set
            {
                _sessionInverseProgress = value;
                NotifyPropertyChanged();
            }
        }

        private int _wordCount;

        public int WordCount
        {
            get { return _wordCount; }
            set
            {
                _wordCount = value;
                NotifyPropertyChanged();
            }
        }


        public MainViewModel()
        {
            CurrentSessionState = PomoSessionState.Stopped;
            
            SessionText = "";
            ResetSessionInverseProgress();
        }

        private void ResetSessionInverseProgress()
        {
            SessionInverseProgress = 100;
        }

        private void FillInDetailsFromSession()
        {
            CurrentSession.TimerTicked += CurrentSession_TimerTicked;
            CurrentSession.SessionCompleted += CurrentSession_SessionCompleted;
            CurrentSession.StateChanged += CurrentSession_StateChanged;

            // TODO: Adjust this to support the different Session Lengths for each session state
            //_sessionLength = CalculateSessionLength()
            //_sessionLength = TimeSpan.FromMilliseconds(CurrentSession.SessionSettings.SessionLength.TimeInMilliseconds);
            //CurrentSessionTime = _sessionLength;
        }

        private async void CurrentSession_SessionCompleted(object sender, EventArgs e)
        {
            //CurrentSessionTime = new TimeSpan(0);
            try
            {
                //var sessionCompletedDialog = new SessionCompletedDialog(SessionText);
                //await sessionCompletedDialog.ShowAsync();
            }
            catch (Exception)
            {

            }

        }

        private void CurrentSession_StateChanged(object sender, PomoSessionState newState)
        {
            CurrentSessionState = newState;
        }


        internal void Resume()
        {
            CurrentSession.StartSession();
        }

        internal void Start()
        {
            SessionText = "";
            CurrentSession = new PomoSession();
            FillInDetailsFromSession();
        }

        internal void Pause()
        {
            CurrentSession.PauseSession();
        }

        internal void Stop()
        {
            CurrentSession.StopSession();
            ResetSessionInverseProgress();
        }

        private void CurrentSession_TimerTicked(object sender, TimeSpan timeElapsed)
        {
            CurrentSessionTime = _sessionLength - timeElapsed;
        }
    }
}
