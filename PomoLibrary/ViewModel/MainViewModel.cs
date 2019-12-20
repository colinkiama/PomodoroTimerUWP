using PomoLibrary.Commands;
using PomoLibrary.Common;
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
        public RelayCommand PlayPauseCommand { get; private set; }

        public RelayCommand ResetCommand { get; private set; }

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


        private int[] _totalSessionProgress;

        public int[] TotalSessionProgress
        {
            get { return _totalSessionProgress; }
            set
            {
                _totalSessionProgress = value;
                NotifyPropertyChanged();
            }
        }


        public MainViewModel()
        {
            TotalSessionProgress = new int[] { 0, 0 };
            CurrentSessionState = PomoSessionState.Stopped;
            PlayPauseCommand = new RelayCommand(PlayPauseCommandCalled);
            ResetCommand = new RelayCommand(ResetCommandCalled);
            SettingsService.Instance.SessionSettingsUpdated += Instance_SessionSettingsUpdated;
            // Code beyond this point needs to be adjusted
            // to support instances where data from an unfinished session
            // should be used
            bool sessionLoadedFromFile = TryGetLoadedSession();
            if (!sessionLoadedFromFile)
            {
                CreateNewSession();
            }
            Application.Current.EnteredBackground += Current_EnteredBackground;
        }

        private bool TryGetLoadedSession()
        {
            
            PomoSession loadedSession = FileIOService.Instance.GetLoadedCurrentSessionData();
            bool wasSessionLoaded = loadedSession != null;
            if (wasSessionLoaded)
            {
                CurrentSession = loadedSession;
                FillInDetailsFromSession();
            }
            return wasSessionLoaded;
        }


        private async void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            bool hasSessionNotStarted = CurrentSessionState == PomoSessionState.Stopped && CurrentSession.SessionsCompleted == 0;
            if (!hasSessionNotStarted)
            {
                await FileIOService.Instance.SaveCurrentSessionDataAsync(CurrentSession);
            }
            deferral.Complete();
        }

        private void Instance_SessionSettingsUpdated(object sender, PomoSessionSettings e)
        {
            // Creating a new session is easier than updating the session with changes
            CreateNewSession();
        }

        private void ResetCommandCalled()
        {
            Stop();
            CreateNewSession();
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
            UpdateTotalSessionProgress(CurrentSession.SessionsCompleted, CurrentSession.SessionSettings.NumberOfSessions);
            CurrentSessionState = CurrentSession.CurrentSessionState;
            CurrentSessionType = CurrentSession.CurrentSessionType;
            // TODO: Adjust this to support the different Session Lengths for each session state
            _sessionLength = PickLengthFromType(_currentSessionType);
            UpdateSessionTime(CurrentSession.Timer.GetTimeElapsed());
            
        }

        private TimeSpan PickLengthFromType(PomoSessionType currentSessionType)
        {
            var timeSpanToReturn = TimeSpan.FromSeconds(0);
            switch (currentSessionType)
            {
                case PomoSessionType.Work:
                    timeSpanToReturn = TimeSpan.FromMilliseconds(CurrentSession.SessionSettings.WorkSessionLength.TimeInMilliseconds);
                    break;
                case PomoSessionType.Break:
                    timeSpanToReturn = TimeSpan.FromMilliseconds(CurrentSession.SessionSettings.BreakSessionLength.TimeInMilliseconds);
                    break;
                case PomoSessionType.LongBreak:
                    timeSpanToReturn = TimeSpan.FromMilliseconds(CurrentSession.SessionSettings.LongBreakSessionLength.TimeInMilliseconds);
                    break;
            }
            return timeSpanToReturn;
        }

        private void UpdateTotalSessionProgress(int sessionsCompleted, int numberOfSessions)
        {
            TotalSessionProgress = new int[] { sessionsCompleted, numberOfSessions };
        }

        private void CurrentSession_TypeChanged(object sender, PomoSessionType newSessionType)
        {
            CurrentSessionType = newSessionType;

            // If session is not of a work type, a work session was previously completed
            // So get the session progress changes
            if (newSessionType != PomoSessionType.Work)
            {
                UpdateTotalSessionProgress(CurrentSession.SessionsCompleted, CurrentSession.SessionSettings.NumberOfSessions);
            }
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
        private async void SessionCompletedDialog_CloseButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
        {
            await FileIOService.Instance.RemoveCurrentSessionData();
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
                CurrentSessionTime = _sessionLength;
                CurrentSessionType = nextSessionData.NextSessionType;
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
            NotificationsService.Instance.ShowSessionStartToast(CurrentSessionTime, CurrentSessionType);
            NotificationsService.Instance.ScheduleSessionEndToast(CurrentSessionTime, CurrentSessionType, CurrentSession);
            if (!resumedSession)
            {
                // Session has been completely done!
            }
        }

        internal void Start()
        {
            CreateNewSession();
            CurrentSession.StartSession();
            NotificationsService.Instance.ShowSessionStartToast(CurrentSessionTime, CurrentSessionType);
            NotificationsService.Instance.ScheduleSessionEndToast(CurrentSessionTime, CurrentSessionType, CurrentSession);
        }

        private void CreateNewSession()
        {
            CurrentSession = new PomoSession();
            FillInDetailsFromSession();
        }

        internal void Pause()
        {
            CurrentSession.PauseSession();
            NotificationsService.Instance.ClearAllNotifications();
        }

        internal void Stop()
        {
            CurrentSession.StopSession();
            NotificationsService.Instance.ClearAllNotifications();
        }

        private void CurrentSession_TimerTicked(object sender, TimeSpan timeElapsed)
        {
            UpdateSessionTime(timeElapsed);
        }

        private void UpdateSessionTime(TimeSpan timeElapsed)
        {
            CurrentSessionTime = _sessionLength - timeElapsed;
        }
    }
}
