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
        public const string emailValue = "colinkiama@gmail.com";
        public const string StoreID = "9PJSR2QK1V1V";
        public static readonly string ReviewString = $"ms-windows-store://review/?ProductId={StoreID}";
        public static readonly string FeedbackString;
        
        static string appDisplayName = Package.Current.DisplayName;

        static ReviewHelper()
        {
            FeedbackString = $"mailto:{emailValue}?subject={appDisplayName}%20Feedback&body=<Write%20your%20feedback%20here>";
        }
      

    }
}
