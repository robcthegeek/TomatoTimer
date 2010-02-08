using System;

namespace TomatoTimer.Core
{
    public class TickEventArgs : TimerEventArgs
    {
        public TimeSpan TimeElapsed { get; set; }
        public TimeSpan TimeRemaining { get; set; }

        public TickEventArgs(ITimer timer, TimeSpan timeElapsed, TimeSpan timeRemaining) : base(timer)
        {
            TimeElapsed = timeElapsed;
            TimeRemaining = timeRemaining;
        }
    }
}