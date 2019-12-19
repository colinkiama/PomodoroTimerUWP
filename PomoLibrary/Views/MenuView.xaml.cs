using PomoLibrary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private List<MenuSetting> _menuSettings { get; set; } = new List<MenuSetting>
        {
            new MenuSetting{ Title= "Settings" ,IconGlyph = "\xE713" },
            new MenuSetting{ Title= "Statistics" ,IconGlyph = "\xE9D9" },
            new MenuSetting{ Title= "About" ,IconGlyph = "\xE897" },
        };


        public MenuView()
        {
            this.InitializeComponent();

        }

        private void SettingsListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
