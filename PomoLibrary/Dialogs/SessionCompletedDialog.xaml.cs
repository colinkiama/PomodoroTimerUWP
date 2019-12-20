using PomoLibrary.Enums;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PomoLibrary.Dialogs
{
    public sealed partial class SessionCompletedDialog : ContentDialog
    {
       

        public SessionCompletedDialog(PomoSessionType sessionType)
        {
            this.InitializeComponent();
            string sessionTypeTitle = "";
            switch (sessionType)
            {
                case PomoSessionType.Work:
                    sessionTypeTitle = "Work Session";
                    break;
                case PomoSessionType.Break:
                    sessionTypeTitle = "Break Session";
                    break;
                case PomoSessionType.LongBreak:
                    sessionTypeTitle = "Long Break Session";
                    break;
            }

            this.Title = sessionTypeTitle;
            this.Content = $"{sessionTypeTitle} has ended. Please select what to do next.";
            this.PrimaryButtonText = "Continue";
            this.CloseButtonText = "Stop";
        }

        
    }
}
