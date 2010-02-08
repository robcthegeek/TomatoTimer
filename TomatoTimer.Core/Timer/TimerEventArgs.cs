using System;

namespace TomatoTimer.Core
{
    /// <summary>
    /// Base Class for Timer Event Arguments.
    /// </summary>
    public class TimerEventArgs : EventArgs
    {
        public ITimer Timer { get; private set; }

        public TimerEventArgs(ITimer timer)
        {
            Timer = timer;
        }
    }
}