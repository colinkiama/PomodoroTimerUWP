﻿using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PomoLibrary.Services
{
    public class SettingsService
    {
        public PomoSessionSettings SessionSettings { get => _sessionSettings; set => _sessionSettings = value; }

        private PomoSessionSettings _sessionSettings;

        public event EventHandler<PomoSessionSettings> SessionSettingsUpdated;

        // Singleton Pattern with "Lazy"
        private static Lazy<SettingsService> lazy =
            new Lazy<SettingsService>(() => new SettingsService());

        public static SettingsService Instance => lazy.Value;

        private SettingsService()
        {
            Application.Current.EnteredBackground += Current_EnteredBackground;
        }

        private async void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await SaveSettingsAsync();
            deferral.Complete();
        }

        public void UpdateSessionSettings(PomoSessionSettings sessionSettings)
        {
            if (sessionSettings != this._sessionSettings)
            {
                this._sessionSettings = sessionSettings;
                SessionSettingsUpdated?.Invoke(this, sessionSettings);
            }
        }


        public async Task LoadSettingsAsync()
        {
            var sessionSettingsLoad = await FileIOService.Instance.LoadSessionSettings();
            if (sessionSettingsLoad != null)
            {
                SessionSettings = (PomoSessionSettings)sessionSettingsLoad;
            }
            else
            {
                // TODO: Load from file
                SessionSettings = new PomoSessionSettings
                {

                    WorkSessionLength = new PomoSessionLength
                    {
                        Length = 25,
                        UnitOfLength = Enums.TimeUnit.Minutes
                    },
                    BreakSessionLength = new PomoSessionLength
                    {
                        Length = 5,
                        UnitOfLength = Enums.TimeUnit.Minutes
                    },
                    LongBreakSessionLength = new PomoSessionLength
                    {
                        Length = 20,
                        UnitOfLength = Enums.TimeUnit.Minutes
                    },
                    NumberOfSessions = 4
                };

            }

        }

        public async Task SaveSettingsAsync()
        {
            await FileIOService.Instance.SaveSessionSettingsAsync(_sessionSettings);
        }

    }
}
