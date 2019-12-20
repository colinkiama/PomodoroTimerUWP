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


        const string ScheduledNotificationID = "scheduled";
        const string SessionStartNotificationID = "sessionStart";

        private ToastNotifier _toastNotifier;
        // Singleton Pattern with "Lazy"
        private static Lazy<NotificationsService> lazy =
            new Lazy<NotificationsService>(() => new NotificationsService());

        private NotificationsService()
        {
            _toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }
        public static NotificationsService Instance => lazy.Value;

        public void ShowSessionStartToast(TimeSpan timeToPass, PomoSessionType sessionType)
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
                    Text = $"{SessionStringHelper.GetSessionString(sessionType)} session has started"
                },
                new AdaptiveText()
                {
                    Text = $"Will end at {DateTime.Now.Add(timeToPass)}"
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
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());
            toastNotif.Tag = SessionStartNotificationID;
            // And send the notification
            _toastNotifier.Show(toastNotif);
        }

        public void ScheduleSessionEndToast(TimeSpan timeToPass, PomoSessionType sessionType, PomoSession session)
        {
            if (sessionType == PomoSessionType.LongBreak)
            {
                ScheduleAllSessionCompletedToast(timeToPass, session);
            }
            else
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
                    Text = $"{SessionStringHelper.GetSessionString(sessionType)} session has ended"
                },
                new AdaptiveText()
                {
                    Text = $"Work Sessions Completed: {session.SessionsCompleted}/{session.SessionSettings.NumberOfSessions}"
                },


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
                var scheduledToast = new ScheduledToastNotification(toastContent.GetXml(), DateTimeOffset.UtcNow.Add(timeToPass));
                scheduledToast.Tag = ScheduledNotificationID;
                scheduledToast.Id = ScheduledNotificationID;

                // And send the notification
                _toastNotifier.AddToSchedule(scheduledToast);
            }

        }


        private void ScheduleAllSessionCompletedToast(TimeSpan timeToPass, PomoSession session)
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
                },
                new AdaptiveText()
                {
                    Text = $"Work Sessions Completed: {session.SessionsCompleted}/{session.SessionSettings.NumberOfSessions}"
                },

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
            var scheduledToast = new ScheduledToastNotification(toastContent.GetXml(), DateTimeOffset.UtcNow.Add(timeToPass));
            scheduledToast.Tag = ScheduledNotificationID; 
            scheduledToast.Id = ScheduledNotificationID;

            // And send the notification
            _toastNotifier.AddToSchedule(scheduledToast);
        }

        public void ClearScheduledNotifications()
        {
            var scheduledToasts = _toastNotifier.GetScheduledToastNotifications();
            List<ScheduledToastNotification> scheduledToastsToRemove = new List<ScheduledToastNotification>();
            foreach (var toast in scheduledToasts)
            {
                scheduledToastsToRemove.Add(toast);
            }

            for (int i = scheduledToastsToRemove.Count - 1; i >= 0; i--)
            {
                _toastNotifier.RemoveFromSchedule(scheduledToastsToRemove[i]);
            }
         }
    }
}
