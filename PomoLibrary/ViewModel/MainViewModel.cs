using PomoLibrary.Commands;
using PomoLibrary.Dialogs;
using PomoLibrary.Enums;
using PomoLibrary.Helpers;
using PomoLibrary.Model;
using PomoLibrary.Services;
using PomoLibrary.Structs;
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
    public class MainViewModel : Notifier
    {

        private RelayCommand _playPauseCommand;

        public RelayCommand PlayPauseCommand
        {
            get { return _playPauseCommand; }
            set
            {
                _playPauseCommand = value;
                NotifyPropertyChanged();
            }
        }


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


        private PomoSessionType _currentSessionType;

        public PomoSessionType CurrentSessionType
        {
            get { return _currentSessionType; }
            set
            {
                _currentSessionType = value;
                NotifyPropertyChanged();
            }
        }




        public MainViewModel()
        {
            CurrentSessionState = PomoSessionState.Stopped;
            PlayPauseCommand = new RelayCommand(PlayPauseCommandCalled);
        }

        private void PlayPauseCommandCalled()
        {
            switch (CurrentSessionState)
            {
                case PomoSessionState.InProgress:
                    Pause();
                    break;
                case PomoSessionState.Paused:
                    Resume();
                    break;
                case PomoSessionState.Stopped:
                    Start();
                    break;
            }
        }

        private void FillInDetailsFromSession()
        {
            CurrentSession.TimerTicked += CurrentSession_TimerTicked;
            CurrentSession.SessionCompleted += CurrentSession_SessionCompleted;
            CurrentSession.StateChanged += CurrentSession_StateChanged;
            CurrentSession.TypeChanged += CurrentSession_TypeChanged;

            // TODO: Adjust this to support the different Session Lengths for each session state
            _sessionLength = TimeSpan.FromMilliseconds(CurrentSession.SessionSettings.WorkSessionLength.TimeInMilliseconds);
            CurrentSessionTime = _sessionLength;
        }

        private void CurrentSession_TypeChanged(object sender, PomoSessionType newSessionType)
        {
            CurrentSessionType = newSessionType;
        }

        private async void CurrentSession_SessionCompleted(object sender, EventArgs e)
        {
            CurrentSessionTime = new TimeSpan(0);
            try
            {
                var sessionCompletedDialog = new SessionCompletedDialog(CurrentSession.CurrentSessionType);
                sessionCompletedDialog.PrimaryButtonClick += SessionCompletedDialog_PrimaryButtonClick;
                sessionCompletedDialog.CloseButtonClick += SessionCompletedDialog_CloseButtonClick;
                await sessionCompletedDialog.ShowAsync();
            }
            catch (Exception)
            {

            }

        }

        // This means that the user wants to stop the whole session
        private void SessionCompletedDialog_CloseButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
        {
            CreateNewSession();
        }

        // This means that the user wants to continue the session
        private void SessionCompletedDialog_PrimaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
        {
            ContinueSession();
        }

        private void ContinueSession()
        {
            NextSessionData nextSessionData = CurrentSession.GetNextSessionData();
            if (nextSessionData.NextSessionState == PomoSessionState.Stopped)
            {
                // Session is completely over
                CreateNewSession();
            }
            else
            {
                // Apply next session data and start the next stage
                _sessionLength = nextSessionData.NextSessionLength;
                Resume();
            }
        }

        private void CurrentSession_StateChanged(object sender, PomoSessionState newState)
        {
            CurrentSessionState = newState;
        }


        internal void Resume()
        {
            bool resumedSession = CurrentSession.StartSession();
            if (!resumedSession)
            {
                // Session has been completely done!
            }
        }

        internal void Start()
        {
            CreateNewSession();
            CurrentSession.StartSession();
        }

        private void CreateNewSession()
        {
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
        }

        private void CurrentSession_TimerTicked(object sender, TimeSpan timeElapsed)
        {
            CurrentSessionTime = _sessionLength - timeElapsed;
        }
    }
}
