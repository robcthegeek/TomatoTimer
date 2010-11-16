using System;
using System.Windows.Threading;

namespace TomatoTimer.Core
{
    public class TimerComponent : ITimerComponent
    {
        // TODO (RC): Extract Dependency on DispatcherTimer

        private readonly DispatcherTimer timer;
        private DateTime startTime;
        private DateTime stopTime;
        private DateTime stoppedTime;
        public TimeSpan Interval = new TimeSpan(0, 0, 0, 1);

        #region Events
        public event EventHandler TimerStarted;
        protected void OnTimerStarted()
        {
            if (TimerStarted != null)
            {
                TimerStarted(this, EventArgs.Empty);
            }
        }

        public event EventHandler TimerStopped;
        protected void OnTimerStopped()
        {
            if (TimerStopped != null)
            {
                TimerStopped(this, EventArgs.Empty);
            }
        }

        public event EventHandler<TickEventArgs> Tick;
        protected void OnTick()
        {
            if (Tick != null)
            {
                Tick(this, new TickEventArgs(null, Elapsed, Remaining));
            }
        }
        #endregion

        public TimerComponent()
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = Interval;
            timer.Tick += timer_Tick;

            ResetTimes();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            OnTick();
            if (DateTime.Now >= stopTime)
                Stop();
        }

        public TimeSpan Remaining
        {
            get
            {
                if (!timer.IsEnabled)
                    return TimeSpan.Zero;

                return DateTime.Now.TimeSpanBetween(stopTime);
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                if (!timer.IsEnabled)
                    return startTime.TimeSpanBetween(stoppedTime);
                return startTime.TimeSpanBetween(DateTime.Now);
            }
        }

        public void Start(TimeSpan timeSpan)
        {
            startTime = DateTime.Now;
            stopTime = DateTime.Now.Add(timeSpan);            
            timer.Start();
            timer.Interval = Interval;
            OnTimerStarted();
        }

        public void Stop()
        {
            timer.Interval = TimeSpan.Zero;
            ResetTimes();
            timer.Stop();
            stoppedTime = DateTime.Now;
            OnTimerStopped();
        }

        /// <summary>
        /// Resets the 'nextTickTime' and 'stopTime' Variables to DateTime.MinValue.
        /// </summary>
        private void ResetTimes()
        {
            stopTime = DateTime.MinValue;
        }
    }
}