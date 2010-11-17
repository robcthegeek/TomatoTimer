using System;

namespace TomatoTimer.Core
{
    public class TimerStoppedEventArgs : EventArgs
    {
        public DateTime TimeStopped { get; private set; }
        public TimeSpan Elapsed { get; private set; }

        public TimerStoppedEventArgs(DateTime timeStopped, TimeSpan elapsed)
        {
            TimeStopped = timeStopped;
            Elapsed = elapsed;
        }
    }
}
