using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace PomoLibrary.Helpers
{
    public class AppStartupHelper
    {
        public static async Task AppStartupAsync()
        {
            var _appView = ApplicationView.GetForCurrentView();
            _appView.SetPreferredMinSize(new Size(192,192));
        }
    }
}
