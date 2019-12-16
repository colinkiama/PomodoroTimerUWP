using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PomoLibrary.Model
{
    public class SessionTimer
    {
        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler TimerEnded;
        DispatcherTimer timer;
        int timesTicked = 0;
        int timesToTick = 0;


        public SessionTimer(PomoSessionLength sessionTime)
        {
            timer = new DispatcherTimer();
            timesTicked = 0;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);

            timesToTick = (int)Math.Ceiling(sessionTime.TimeInMilliseconds / timer.Interval.TotalMilliseconds);
        }

        public void SetTimer(TimeSpan timeToRunFor)
        {
            timesTicked = 0;
            timesToTick = (int)Math.Ceiling(timeToRunFor.TotalMilliseconds / timer.Interval.TotalMilliseconds);
        }

        private void Timer_Tick(object sender, object e)
        {
            timesTicked++;
            Debug.WriteLine($"Times ticked: {timesTicked}/{timesToTick}");
            if (timesTicked > timesToTick)
            {
                timer.Stop();
                TimerEnded?.Invoke(sender, EventArgs.Empty);
            }
            else
            {
                TimeSpan timeElapsed = TimeSpan.FromSeconds(timer.Interval.TotalSeconds * timesTicked);
                TimerTicked?.Invoke(this, timeElapsed);
            }

        }

        public bool StartTimer()
        {
            bool willStart = timesToTick > timesTicked;
            if (willStart)
            {
                timer.Start();
            }

            return willStart;
        }

        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
