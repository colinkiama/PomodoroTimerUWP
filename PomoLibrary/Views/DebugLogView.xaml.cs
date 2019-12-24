using PomoLibrary.Services;
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
    public sealed partial class DebugLogView : Page
    {
        public DebugLogView()
        {
            this.InitializeComponent();
            MenuButtonService.Instance.BackButtonClicked += Instance_BackButtonClicked;
        }

        private void Instance_BackButtonClicked(object sender, EventArgs e)
        {
            NavService.Instance.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MenuButtonService.Instance.NavigatedToMenuChild();
            DebugLogTextBlock.Text = DebugService.GetLogString();
        }
    }
}
