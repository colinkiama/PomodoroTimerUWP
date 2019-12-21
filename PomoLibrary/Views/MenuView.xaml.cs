using PomoLibrary.Dialogs;
using PomoLibrary.Model;
using PomoLibrary.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace PomoLibrary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuView : Page
    {
        private AboutDialog _aboutDialog = new AboutDialog();
        private List<MenuItem> _menuSettings { get; set; } = new List<MenuItem>
        {
            new MenuItem{ Title= "Settings" ,IconGlyph = "\xE713" },
            //new MenuItem{ Title= "Statistics" ,IconGlyph = "\xE9D9" },
            new MenuItem{ Title= "About" ,IconGlyph = "\xE897" },
        };


        public MenuView()
        {
            this.InitializeComponent();

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
            }
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
