using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Notifications
{
    public class NotificationsService
    {
        // Singleton Pattern with "Lazy"
        private static Lazy<NotificationsService> lazy =
            new Lazy<NotificationsService>(() => new NotificationsService());

        public static NotificationsService Instance => lazy.Value;

        public void ShowSessionStartToast()
        {

        }

        public void ShowSessionEndToast()
        {

        }

        public void ShowAllSessionsCompletedToast()
        {

        }
    }
}
