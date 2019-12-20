using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace PomoLibrary.Helpers
{
    public static class ReviewHelper
    {
        const string launchCountSettingsValue = "launchCount";
        const string noMorePromptsSettingsValue = "noMorePrompts";
        
        public const string emailValue = "colinkiama@gmail.com";
        public const string StoreID = "9PJSR2QK1V1V";
        public static readonly string ReviewString = $"ms-windows-store://review/?ProductId={StoreID}";
        static string appDisplayName = Package.Current.DisplayName;
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        static ReviewHelper()
        {
            TryUpdateLaunchCount();
        }
      

        private static void TryUpdateLaunchCount()
        {
            if (localSettings.Values[noMorePromptsSettingsValue] == null)
            {
                localSettings.Values[noMorePromptsSettingsValue] = false;
                localSettings.Values[launchCountSettingsValue] = (byte)1;
            }

            else if ((bool)localSettings.Values[noMorePromptsSettingsValue] == false)
            {
                {
                    byte oldValue = (byte)localSettings.Values[launchCountSettingsValue];
                    localSettings.Values[launchCountSettingsValue] = (byte)(oldValue + 1);
                }
            }
        }

        // Recommended: Run this when you navigate app's "Home Page" or "Shell".
        public static async Task TryRequestReviewAsync()
        {

            if (CheckIfTimeForReview())
            {
                var reviewDialog = new ContentDialog()
                {
                    Title = "Enjoying the app?",
                    Content = "It would be lovely if you left a review! 😊",
                    PrimaryButtonText = "Review",
                    CloseButtonText = "Later"

                };

                reviewDialog.Closed += ReviewDialog_Closed;
                await reviewDialog.ShowAsync();
            }
        }

        private static async void ReviewDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                RemovePromptsForever();
                await Launcher.LaunchUriAsync(new Uri(ReviewString));
            }
            else
            {
                await ShowFeedbackDialog();
            }
        }

        private static void RemovePromptsForever()
        {
            localSettings.Values[noMorePromptsSettingsValue] = true;
        }

        private async static Task ShowFeedbackDialog()
        {
            try
            {

                var feedbackDialog = new ContentDialog()
                {
                    Title = "Feedback",
                    Content = "Your feedback is valuable. It improves your experience!",
                    PrimaryButtonText = "Give Feedback",
                    CloseButtonText = "Later",
                };

                feedbackDialog.Closed += FeedbackDialog_Closed;
                await feedbackDialog.ShowAsync();
            }
            catch (Exception)
            {
            }

        }

        private async static void FeedbackDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                await Launcher.LaunchUriAsync(new Uri($"mailto:{emailValue}?subject={appDisplayName}%20Feedback&body=<Write%20your%20feedback%20here>"));
            }
        }

        private static bool CheckIfTimeForReview()
        {
            bool isTimeToReview = false;
            if ((bool)localSettings.Values[noMorePromptsSettingsValue] == false)
            {
                if ((byte)localSettings.Values[launchCountSettingsValue] == (byte)2)
                {
                    isTimeToReview = true;
                    localSettings.Values[launchCountSettingsValue] = (byte)0;
                }
            }

            return isTimeToReview;
        }
    }
}
