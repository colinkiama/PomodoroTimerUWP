using PomoLibrary.Dialogs;
using PomoLibrary.Model;
using PomoLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PomoLibrary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuView : Page
    {
        private AboutDialog _aboutDialog = new AboutDialog();
        private List<MenuItem> _menuSettings { get; set; }


        public MenuView()
        {
            this.InitializeComponent();
#if DEBUG
            CreateMenuWithDebugLog();
#else
            CreateMenuForRelease();
#endif

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MenuButtonService.Instance.NavigatedToMenu();
        }

        private async void MenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem;
            if (_menuSettings.Contains(clickedItem))
            {
                var clickedMenuItem = (MenuItem)clickedItem;

#if DEBUG
                switch (_menuSettings.IndexOf(clickedMenuItem))
                {
                    case 0:
                        Frame.Navigate(typeof(SettingsView));
                        break;
                   
                    case 1:
                        await ShowAboutDialogAsync();
                        break;
                    case 2:
                        Frame.Navigate(typeof(DebugLogView));
                        break;
                }
#else

                switch (_menuSettings.IndexOf(clickedMenuItem))
                {
                    case 0:
                        Frame.Navigate(typeof(SettingsView));
                        break;
                    //case 1:
                    //    //Frame.Navigate(typeof(StatisticsView))
                    //    break;
                    case 1:
                        await ShowAboutDialogAsync();
                        break;
                }
#endif


            }

        }

        private void CreateMenuWithDebugLog()
        {
            _menuSettings = new List<MenuItem> {
            new MenuItem { Title = "Settings", IconGlyph = "\xE713" },
            //new MenuItem{ Title= "Statistics" ,IconGlyph = "\xE9D9" },
            new MenuItem { Title = "About", IconGlyph = "\xE897" },
            new MenuItem { Title = "Debug", IconGlyph = "\xE1DE" },
            };
        }

        private void CreateMenuForRelease()
        {
            _menuSettings = new List<MenuItem> {
            new MenuItem { Title = "Settings", IconGlyph = "\xE713" },
            //new MenuItem{ Title= "Statistics" ,IconGlyph = "\xE9D9" },
            new MenuItem { Title = "About", IconGlyph = "\xE897" },
            };
        }

        private async Task ShowAboutDialogAsync()
        {
            try
            {
                await _aboutDialog.ShowAsync();
            }
            catch (Exception)
            {


            }
        }
    }
}
