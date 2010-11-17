using System;

namespace TomatoTimer.Core
{
    public class TimerStartedEventArgs : EventArgs
    {
        public DateTime TimeStarted { get; private set; }

        public TimerStartedEventArgs(DateTime timeStarted)
        {
            TimeStarted = timeStarted;
        }
    }
}
