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

        public long CurrentTickSum { get; set; } = 0;
        public long FinalTickSum { get; set; } = 0;
        public DateTime LastTickTime { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public TimeSpan SessionTime { get; set; }

        public SessionTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = _timerInterval;
        }

        public SessionTimer(PomoSessionLength sessionTime)
        {
            timer = new DispatcherTimer();
            CurrentTickSum = 0;
            FinalTickSum = 0;
            timer.Tick += Timer_Tick;
            timer.Interval = _timerInterval;
            SessionTime = TimeSpan.FromMilliseconds(sessionTime.TimeInMilliseconds);
        }

        public void SetTimer(TimeSpan timeToRunFor)
        {
            CurrentTickSum = 0;
            FinalTickSum = 0;
            SessionTime = timeToRunFor;
        }

        // Note: Timer doesn't always tick on time
        private void Timer_Tick(object sender, object e)
        {

            // Calculates time since last tick and adds ticks based on the time passed
            TimeSpan timePassedSinceLastTick = DateTime.UtcNow - LastTickTime;
            CurrentTickSum += timePassedSinceLastTick.Ticks;


            Debug.WriteLine($"Timer Progress: {CurrentTickSum}/{FinalTickSum}");
            LastTickTime = DateTime.UtcNow;
            if (CurrentTickSum >= FinalTickSum)
            {
                timer.Stop();
                TimerEnded?.Invoke(sender, EventArgs.Empty);
            }
            else
            {
                TimerTicked?.Invoke(sender, GetTimeLeft());
            }

        }

        public TimeSpan GetTimeLeft()
        {
            TimeSpan timeToReturn = SessionTime;
            if (CurrentTickSum > 0)
            {

                timeToReturn = TimeSpan.FromTicks(FinalTickSum - CurrentTickSum);
            }

            return timeToReturn;
        }

        public bool StartTimer()
        {
            SessionStartTime = DateTime.UtcNow;

            // Session ends when CurrentTickSum > FinalTickSum

            if (FinalTickSum <= 0)
            {
                SessionEndTime = SessionStartTime.Add(SessionTime);
                FinalTickSum = SessionEndTime.Ticks - SessionStartTime.Ticks;
            }
            else
            {
                SessionEndTime = SessionStartTime.Add(TimeSpan.FromTicks(FinalTickSum - CurrentTickSum));
            }



            DebugService.AddToLog($"Session Start Time: {SessionStartTime}");
            DebugService.AddToLog($"Session End Time: {SessionEndTime}");
            DebugService.AddToLog($"Timer Progress: {CurrentTickSum}, Times To tick: {FinalTickSum}");

            LastTickTime = DateTime.UtcNow;

            bool willStart = FinalTickSum > CurrentTickSum;
            if (willStart)
            {
                DebugService.AddToLog($"Session Start: {DateTimeOffset.UtcNow}");

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
            TimeSpan timePassedSinceLastTick = DateTime.UtcNow - LastTickTime;
            CurrentTickSum += timePassedSinceLastTick.Ticks;
        }


    }
}
