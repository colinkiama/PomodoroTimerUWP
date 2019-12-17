using PomoLibrary.Enums;
using PomoLibrary.Services;
using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.ViewModel
{
    public class SettingsViewModel : Notifier
    {

        private int _workSessionLength;

        public int WorkSessionLength
        {
            get { return _workSessionLength; }
            set
            {
                if (_workSessionLength != value)
                {
                    _workSessionLength = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TimeUnit _workSessionTimeUnit;

        public TimeUnit WorkSessionTimeUnit
        {
            get { return _workSessionTimeUnit; }
            set
            {
                if (_workSessionTimeUnit != value)
                {
                    _workSessionTimeUnit = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _breakSessionLength;

        public int BreakSessionLength
        {
            get { return _breakSessionLength; }
            set
            {
                if (_breakSessionLength != value)
                {
                    _breakSessionLength = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TimeUnit _breakSessionTimeUnit;

        public TimeUnit BreakSessionTimeUnit
        {
            get { return _breakSessionTimeUnit; }
            set
            {
                if (_breakSessionTimeUnit != value)
                {
                    _breakSessionTimeUnit = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _longBreakSessionLength;

        public int LongBreakSessionLength
        {
            get { return _longBreakSessionLength; }
            set
            {
                if (_longBreakSessionLength != value)
                {
                    _longBreakSessionLength = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TimeUnit _longBreakSessionTimeUnit;

        public TimeUnit LongBreakSessionTimeUnit
        {
            get { return _longBreakSessionTimeUnit; }
            set
            {
                if (_longBreakSessionTimeUnit != value)
                {
                    _longBreakSessionTimeUnit = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Beware of double negative!
        // This was done for easier xaml binding
        private bool _isSessionNotInProgress;

        public bool IsSessionNotInProgress
        {
            get { return _isSessionNotInProgress; }
            set
            {
                _isSessionNotInProgress = value;
                NotifyPropertyChanged();
            }
        }


        public List<double> FontList { get; set; } = new List<double>()
        {
            8,9,10,11,12,14,15,18,24,30,36,48,60,72,96
        };

        public List<TimeUnit> TimeUnits { get; set; } = new List<TimeUnit>()
        {
            TimeUnit.Seconds, TimeUnit.Minutes, TimeUnit.Hours
        };


        public SettingsViewModel()
        {
            MenuButtonService.Instance.MenuClosed += Instance_MenuClosed;
            SessionService.Instance.SessionStateChanged += Instance_SessionStateChanged;
            IsSessionNotInProgress = true;
        }

        private void Instance_SessionStateChanged(object sender, PomoSessionState sessionState)
        {
            switch (sessionState)
            {
                case PomoSessionState.Stopped:
                    IsSessionNotInProgress = true;
                    break;
                default:
                    IsSessionNotInProgress = false;
                    break;
            }
        }

        private PomoSessionSettings GetSessionSettingsFromViewModel() => new PomoSessionSettings
        {
            WorkSessionLength = new PomoSessionLength { Length = _workSessionLength, UnitOfLength = _workSessionTimeUnit },
            BreakSessionLength = new PomoSessionLength { Length = _breakSessionLength, UnitOfLength = _breakSessionTimeUnit },
            LongBreakSessionLength = new PomoSessionLength { Length = _longBreakSessionLength, UnitOfLength = _longBreakSessionTimeUnit },
        };

        private void Instance_MenuClosed(object sender, EventArgs e)
        {
            SetSettings();
        }

        public void GetSettings()
        {
            var sessionSettings = SettingsService.Instance.SessionSettings;
            AddSessionSettingsToViewModel(sessionSettings);
        }

        public void SetSettings()
        {
            if (_isSessionNotInProgress)
            {
                var sessionSettings = GetSessionSettingsFromViewModel();
                SettingsService.Instance.UpdateSessionSettings(sessionSettings);
            }
        }


        private void AddSessionSettingsToViewModel(PomoSessionSettings sessionSettings)
        {
            WorkSessionLength = sessionSettings.WorkSessionLength.Length;
            WorkSessionTimeUnit = sessionSettings.WorkSessionLength.UnitOfLength;

            BreakSessionLength = sessionSettings.BreakSessionLength.Length;
            BreakSessionTimeUnit = sessionSettings.BreakSessionLength.UnitOfLength;

            LongBreakSessionLength = sessionSettings.LongBreakSessionLength.Length;
            LongBreakSessionTimeUnit = sessionSettings.LongBreakSessionLength.UnitOfLength;
        }
    }
}
