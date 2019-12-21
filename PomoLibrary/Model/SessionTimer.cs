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

        DispatcherTimer timer;

        public int TimesTicked { get; set; } = 0;
        public int TimesToTick { get; set; } = 0;
        public DateTime LastTickTime { get; set; }

        public SessionTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
        }
        
        public SessionTimer(PomoSessionLength sessionTime)
        {
            timer = new DispatcherTimer();
            TimesTicked = 0;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);

            TimesToTick = (int)Math.Ceiling(sessionTime.TimeInMilliseconds / timer.Interval.TotalMilliseconds);
        }

        public void SetTimer(TimeSpan timeToRunFor)
        {
            TimesTicked = 0;
            TimesToTick = (int)Math.Ceiling(timeToRunFor.TotalMilliseconds / timer.Interval.TotalMilliseconds);
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

        public TimeSpan GetTimeElapsed() => TimeSpan.FromSeconds(timer.Interval.TotalSeconds * TimesTicked);

        public bool StartTimer()
        {
            bool willStart = TimesToTick > TimesTicked;
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

        internal void CatchUp()
        {
            TimeSpan timeSinceLastTick = DateTimeOffset.UtcNow.UtcDateTime - LastTickTime;
            int ticksToAdd = (int)(timeSinceLastTick.TotalSeconds / timer.Interval.TotalSeconds);
            TimesTicked += ticksToAdd;
        }
    }
}
