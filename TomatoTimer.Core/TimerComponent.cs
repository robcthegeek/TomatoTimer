using System;
using System.Windows.Threading;

namespace TomatoTimer.Core
{
    public class TimerComponent : ITimerComponent
    {
        // TODO (RC): Extract Dependency on DispatcherTimer

        private readonly ITimer timer;
        private readonly ICurrentTimeProvider time;

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

        public TimerComponent(ITimer timer, ICurrentTimeProvider timeProvider)
        {
            if (timer == null)
                throw new ArgumentNullException(
                    "timer", "Timer Implementation is Required");

            if (timeProvider == null)
                throw new ArgumentNullException(
                    "timeProvider", "CurrentTimeProvider Implementation is Required");

            this.timer = timer;
            this.time = timeProvider;
            this.timer.Interval = Interval;
            this.timer.Tick += timer_Tick;

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
            if (TimeSpanInvalid(timeSpan))
                throw new ArgumentOutOfRangeException(
                    "timeSpan", "Invalid TimeSpan - Please use a value greater than TimeSpan.Zero, less than TimeSpan.MaxValue which when added to the current time does not exceed DateTime.MaxValue");

            if (timer.IsEnabled)
                throw new InvalidOperationException(
                    "Timer component is already running. Please Stop before calling Start again.");

            startTime = DateTime.Now;
            stopTime = DateTime.Now.Add(timeSpan);            
            timer.Start();
            timer.Interval = Interval;
            OnTimerStarted();
        }

        private bool TimeSpanInvalid(TimeSpan value)
        {
            if (value == TimeSpan.Zero)
                return true;
            if (!(value > TimeSpan.MinValue && value < TimeSpan.MaxValue))
                return true;
            if (DateTime.MaxValue.Subtract(value) < time.Now)
                return true;
            return false;
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