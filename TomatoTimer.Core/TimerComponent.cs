using System;

namespace TomatoTimer.Core
{
    public class TimerComponent : ITimerComponent
    {
        private readonly ITimer timer;
        private readonly ICurrentTimeProvider time;

        private DateTime startTime;
        private DateTime stopTime;
        private DateTime stoppedTime;
        private TimeSpan interval = new TimeSpan(0, 0, 0, 1);

        #region Events
        public event EventHandler<TimerStartedEventArgs> TimerStarted;
        protected void OnTimerStarted()
        {
            if (TimerStarted != null)
            {
                var args = new TimerStartedEventArgs(startTime);
                TimerStarted(this, args);
            }
        }

        public event EventHandler<TimerStoppedEventArgs> TimerStopped;
        protected void OnTimerStopped()
        {
            if (TimerStopped != null)
            {
                var args = new TimerStoppedEventArgs(
                    stoppedTime, stoppedTime.Subtract(startTime));
                TimerStopped(this, args);
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
            this.timer.Interval = interval;
            this.timer.Tick += timer_Tick;

            ResetTimes();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            OnTick();
            // TODO (RC): Remove DateTime.Now From Here
            if (DateTime.Now >= stopTime)
                Stop();
        }

        public TimeSpan Remaining
        {
            get
            {
                return timer.IsEnabled ?
                    time.Now.TimeSpanBetween(stopTime) :
                    TimeSpan.Zero;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return timer.IsEnabled ?
                    startTime.TimeSpanBetween(time.Now) :
                    startTime.TimeSpanBetween(stoppedTime);
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

            startTime = time.Now;
            stopTime = time.Now.Add(timeSpan);            
            timer.Start();
            timer.Interval = interval;
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
            if (!timer.IsEnabled)
                throw new InvalidOperationException(
                    "Timer component is not running. Please Start before calling Stop.");

            timer.Interval = TimeSpan.Zero;
            ResetTimes();
            timer.Stop();
            stoppedTime = time.Now;
            OnTimerStopped();
        }

        private void ResetTimes()
        {
            stopTime = DateTime.MinValue;
        }
    }
}