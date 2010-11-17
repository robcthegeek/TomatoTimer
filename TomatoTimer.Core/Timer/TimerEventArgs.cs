using System;

namespace TomatoTimer.Core
{
    /// <summary>
    /// Base Class for Timer Event Arguments.
    /// </summary>
    public class TimerEventArgs : EventArgs
    {
        public ITomatoTimer Timer { get; private set; }

        public TimerEventArgs(ITomatoTimer timer)
        {
            Timer = timer;
        }
    }
}