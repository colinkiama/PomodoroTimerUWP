﻿using PomoLibrary.Commands;
using PomoLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

namespace PomoLibrary.Services
{
    public class MiniViewService : Notifier
    {
        private ApplicationView _appView = ApplicationView.GetForCurrentView();

        // Singleton Pattern with "Lazy"
        private static Lazy<MiniViewService> lazy =
            new Lazy<MiniViewService>(() => new MiniViewService());

        public static MiniViewService Instance => lazy.Value;
        private MiniViewService()
        {
            ToggleMiniViewCommand = new RelayCommand(async () => await TryToggleMiniViewAsync());
        }

        private async Task TryToggleMiniViewAsync()
        {
            if (ApiInformation.IsEnumNamedValuePresent("Windows.UI.ViewManagement.ApplicationViewMode", "CompactOverlay"))
            {
                if (_appView.IsViewModeSupported(ApplicationViewMode.CompactOverlay))
                {
                    await ToggleMiniViewAsync();
                }
            }
        }

        private async Task ToggleMiniViewAsync()
        {
            switch (_appView.ViewMode)
            {
                case ApplicationViewMode.Default:
                    await _appView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                    break;
                case ApplicationViewMode.CompactOverlay:
                    await _appView.TryEnterViewModeAsync(ApplicationViewMode.Default);
                    break;

            }
        }

        private bool _isInMiniView;

        public bool IsInMiniView
        {
            get { return _isInMiniView; }
            set
            {
                _isInMiniView = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand ToggleMiniViewCommand { get; set; }

    }
}