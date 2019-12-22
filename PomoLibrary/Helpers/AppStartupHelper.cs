using PomoLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.Services.Store.Engagement;

namespace PomoLibrary.Helpers
{
    public static class AppStartupHelper
    {
        public static async Task AppStartupAsync()
        {
            var _appView = ApplicationView.GetForCurrentView();
            ExtendViewIntoTitleBar();
            _appView.SetPreferredMinSize(new Size(192,192));
            await SettingsService.Instance.LoadSettingsAsync();
            await FileIOService.Instance.LoadCurrentSessionDataAsync();
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        private static void ExtendViewIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
