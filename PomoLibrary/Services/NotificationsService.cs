using Microsoft.Toolkit.Uwp.Notifications;
using PomoLibrary.Enums;
using PomoLibrary.Helpers;
using PomoLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace PomoLibrary.Services
{
    public class NotificationsService
    {
        // Singleton Pattern with "Lazy"
        private static Lazy<NotificationsService> lazy =
            new Lazy<NotificationsService>(() => new NotificationsService());

        public static NotificationsService Instance => lazy.Value;

        public void ShowSessionStartToast()
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = "Work session has Started"
                },
                new AdaptiveText()
                {
                    Text = "Will end at 18:00"
                }
            }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
        {
            new ToastButton("Dismiss", "dismiss")
            {
                ActivationType = ToastActivationType.Foreground
            }
        }
                },
                Scenario = ToastScenario.Alarm
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public void ScheduleSessionEndToast(PomoSession session)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = $"{SessionStringHelper.GetSessionString(session.CurrentSessionType)} session has ended"
                },
                new AdaptiveText()
                {
                    Text = $"Work Sessions Completed: {session.SessionsCompleted}/{session.SessionSettings.NumberOfSessions}"
                }
            }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
        {
            new ToastButton("Dismiss", "dismiss")
            {
                ActivationType = ToastActivationType.Foreground
            }
        }
                },
                Scenario = ToastScenario.Alarm
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }


        public void ScheduleAllSessionCompletedToast()
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = "All Sessions Completed! 🎉"
                },
                new AdaptiveText()
                {
                    Text = "You've completed all of your sessions! 🥳"
                }
            }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
        {
            new ToastButton("Dismiss", "dismiss")
            {
                ActivationType = ToastActivationType.Foreground
            }
        }
                },
                Scenario = ToastScenario.Alarm
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}
