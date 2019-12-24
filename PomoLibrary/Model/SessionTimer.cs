using PomoLibrary.Services;
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
        private TimeSpan _timerInterval = TimeSpan.FromMilliseconds(250);
        DispatcherTimer timer;

        public double TimesTicked { get; set; } = 0;
        public double TimesToTick { get; set; } = 0;
        public DateTime LastTickTime { get; set; }

        public SessionTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = _timerInterval;
        }
        
        public SessionTimer(PomoSessionLength sessionTime)
        {
            timer = new DispatcherTimer();
            TimesTicked = 0;
            timer.Tick += Timer_Tick;
            timer.Interval = _timerInterval;
            double sessionTimeInSeconds = ConvertMillisecondsToSeconds(sessionTime.TimeInMilliseconds);
            TimesToTick = sessionTimeInSeconds / _timerInterval.TotalSeconds;
        }

        public void SetTimer(TimeSpan timeToRunFor)
        {   
            TimesTicked = 0;
            TimesToTick = timeToRunFor.TotalSeconds / _timerInterval.TotalSeconds;
        }

        // Note: Timer doesn't always tick on time
        private void Timer_Tick(object sender, object e)
        {

            // Calculates time since last tick and adds ticks based on the time passed
            TimeSpan timeSinceLastTick = DateTimeOffset.UtcNow.UtcDateTime - LastTickTime;
            double ticksToAdd = timeSinceLastTick.TotalSeconds / _timerInterval.TotalSeconds;
            TimesTicked += ticksToAdd;

            Debug.WriteLine($"Times ticked: {TimesTicked}/{TimesToTick}");
            LastTickTime = DateTimeOffset.UtcNow.UtcDateTime;
            if (TimesTicked >= TimesToTick)
            {
                timer.Stop();
                TimerEnded?.Invoke(sender, EventArgs.Empty);
            }
            else
            {    
                TimerTicked?.Invoke(sender, GetTimeLeft());
            }

        }

        public TimeSpan GetTimeLeft() => TimeSpan.FromSeconds(_timerInterval.TotalSeconds * (TimesToTick - TimesTicked));

        public bool StartTimer()
        {
            bool willStart = TimesToTick > TimesTicked;
            DebugService.AddToLog($"Times Ticked: {TimesTicked}, Times To tick: {TimesToTick}");
            
            if (willStart)
            {
                DebugService.AddToLog($"Session Start: {DateTimeOffset.UtcNow}");
                LastTickTime = DateTimeOffset.UtcNow.UtcDateTime;
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
            double ticksToAdd = timeSinceLastTick.TotalSeconds / _timerInterval.TotalSeconds;
            TimesTicked += ticksToAdd;
        }

        private double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return milliseconds / 1000;
        }

    }
}
