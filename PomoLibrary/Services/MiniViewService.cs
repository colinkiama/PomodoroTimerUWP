using PomoLibrary.Commands;
using PomoLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace PomoLibrary.Services
{
    public class MiniViewService : Notifier
    {


        public bool IsMiniViewOptionAvailable = false;

        private ApplicationView _appView;

        public RelayCommand ToggleMiniViewCommand { get; set; }

        // Singleton Pattern with "Lazy"
        private static Lazy<MiniViewService> lazy =
            new Lazy<MiniViewService>(() => new MiniViewService());




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


        public static MiniViewService Instance => lazy.Value;
        private MiniViewService()
        {
            _appView = ApplicationView.GetForCurrentView();
            IsInMiniView = false;
            IsMiniViewOptionAvailable = DetermineIfMiniViewOptionIsVisible();
            ToggleMiniViewCommand = new RelayCommand(async () => await TryToggleMiniViewAsync());
        }

        private bool DetermineIfMiniViewOptionIsVisible()
        {
            bool wasMiniViewOptionAvailable = false;
            if (ApiInformation.IsEnumNamedValuePresent("Windows.UI.ViewManagement.ApplicationViewMode", "CompactOverlay"))
            {
                if (_appView.IsViewModeSupported(ApplicationViewMode.CompactOverlay))
                {
                    wasMiniViewOptionAvailable = true;
                }
            }

            return wasMiniViewOptionAvailable;
        }

        private async Task TryToggleMiniViewAsync()
        {
            if (IsMiniViewOptionAvailable)
            {
                await ToggleMiniViewAsync();
            }
        }


        private async Task ToggleMiniViewAsync()
        {
            switch (_appView.ViewMode)
            {
                case ApplicationViewMode.Default:
                    ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                    compactOptions.CustomSize = new Size(450, 450);
                    IsInMiniView = await _appView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                    break;
                case ApplicationViewMode.CompactOverlay:
                    IsInMiniView = !(await _appView.TryEnterViewModeAsync(ApplicationViewMode.Default));
                    break;

            }
        }

    }
}
