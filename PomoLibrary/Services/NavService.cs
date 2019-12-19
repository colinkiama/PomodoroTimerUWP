using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace PomoLibrary.Services
{
    public class NavService
    {
        private Frame _frame = null;

        // Singleton Pattern with "Lazy"
        private static Lazy<NavService> lazy =
            new Lazy<NavService>(() => new NavService());

        public static NavService Instance => lazy.Value;

        public void LoadFrame(Frame frame)
        {
            _frame = frame;

        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        public void GoForward()
        {
            if (_frame.CanGoForward)
            {
                _frame.GoForward();
            }
        }

        public bool Navigate(Type sourcePageType)
        {
            return _frame.Navigate(sourcePageType);
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            return _frame.Navigate(sourcePageType, parameter);
        }

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
        {
            return _frame.Navigate(sourcePageType, parameter, infoOverride);
        }

        public bool IsCurrentPageOfType(Type typeQuery)
        {
            return _frame.SourcePageType.Equals(typeQuery);
        }
    }
}
