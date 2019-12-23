using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PomoLibrary.Model
{
    public class SessionTimer
    {
        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler TimerEnded;
        const int TimerIntervalInMilliseconds = 250;
        const double TimerIntervalInSeconds = 0.25;
        DispatcherTimer timer;

        public int TimesTicked { get; set; } = 0;
        public int TimesToTick { get; set; } = 0;
        public DateTime LastTickTime { get; set; }

        public SessionTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, TimerIntervalInMilliseconds);
        }
        
        public SessionTimer(PomoSessionLength sessionTime)
        {
            timer = new DispatcherTimer();
            TimesTicked = 0;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, TimerIntervalInMilliseconds);
            double sessionTimeInSeconds = ConvertMillisecondsToSeconds(sessionTime.TimeInMilliseconds);
            TimesToTick = (int)Math.Round(sessionTimeInSeconds / TimerIntervalInSeconds, MidpointRounding.AwayFromZero);
        }

        public void SetTimer(TimeSpan timeToRunFor)
        {   
            TimesTicked = 0;
            TimesToTick = (int)Math.Round(timeToRunFor.TotalSeconds / TimerIntervalInSeconds, MidpointRounding.AwayFromZero);
        }

        private void Timer_Tick(object sender, object e)
        {
            TimesTicked++;
            Debug.WriteLine($"Times ticked: {TimesTicked}/{TimesToTick}");
            LastTickTime = DateTimeOffset.UtcNow.UtcDateTime;
            if (TimesTicked > TimesToTick)
            {
                timer.Stop();
                TimerEnded?.Invoke(sender, EventArgs.Empty);
            }
            else
            {
                TimeSpan timeElapsed = GetTimeElapsed();    
                TimerTicked?.Invoke(this, timeElapsed);
            }

        }

        public TimeSpan GetTimeElapsed() => TimeSpan.FromMilliseconds(TimerIntervalInMilliseconds * TimesTicked);

        public bool StartTimer()
        {
            bool willStart = TimesToTick > TimesTicked;
            if (willStart)
            {
                timer.Start();
            }
            else
            {
                TimerEnded?.Invoke(this, EventArgs.Empty);
            }

            return willStart;
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        internal void CatchUp()
        {
            TimeSpan timeSinceLastTick = DateTimeOffset.UtcNow.UtcDateTime - LastTickTime;
            int ticksToAdd = (int)Math.Round(timeSinceLastTick.TotalSeconds / TimerIntervalInSeconds, MidpointRounding.AwayFromZero);
            TimesTicked += ticksToAdd;
        }

        private double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return milliseconds / 1000;
        }

    }
}
