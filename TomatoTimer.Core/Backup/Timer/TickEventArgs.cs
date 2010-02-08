using System;

namespace Leonis.TomatoTimer.Core
{
    public class TickEventArgs : EventArgs
    {
        public TimeSpan TimeElapsed { get; set; }
        public TimeSpan TimeRemaining { get; set; }

        public TickEventArgs(TimeSpan timeElapsed, TimeSpan timeRemaining)
        {
            TimeElapsed = timeElapsed;
            TimeRemaining = timeRemaining;
        }
    }
}