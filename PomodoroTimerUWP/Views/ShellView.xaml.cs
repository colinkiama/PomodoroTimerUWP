using PomoLibrary.Helpers;
using PomoLibrary.Services;
using PomoLibrary.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PomodoroTimerUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView : Page
    {
        bool _isMenuOpen = false;
        private string _appName = Package.Current.DisplayName;
        public ShellView()
        {
            this.InitializeComponent();
            AnimationHelper.FrameSlideOutAnimationCompleted += AnimationHelper_FrameSlideOutAnimationCompleted;
        }

        private void AnimationHelper_FrameSlideOutAnimationCompleted(object sender, EventArgs e)
        {
            MenuBackgroundArea.Visibility = Visibility.Collapsed;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainFrame.Navigate(typeof(MainView));
            MenuFrame.Navigate(typeof(MenuView));
            NavService.Instance.LoadFrame(MenuFrame);

            MenuBackgroundArea.Visibility = Visibility.Collapsed;
            await ReviewHelper.TryRequestReviewAsync();
        }

        private async void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuButtonService.Instance.CheckIfAtMenuRoot())
            {
                if (_isMenuOpen)
                {
                    MenuButtonService.Instance.CloseMenu();
                    await AnimationHelper.UIControlSlideOutAnimation(MenuBackgroundArea);
                   }
                else
                {
                    await AnimationHelper.FrameSlideInAnimation(MenuBackgroundArea);
                }
                _isMenuOpen = !_isMenuOpen; 
            }
            else
            {
                MenuButtonService.Instance.GoBack();
            }
        }
    }
}
