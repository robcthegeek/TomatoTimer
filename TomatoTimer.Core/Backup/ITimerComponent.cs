using System;

namespace Leonis.TomatoTimer.Core
{
    public interface ITimerComponent
    {
        event EventHandler TimerStarted;
        event EventHandler TimerStopped;
        event EventHandler<TickEventArgs> Tick;

        TimeSpan Remaining { get; }
        TimeSpan Elapsed { get; }

        void Start(TimeSpan timeSpan);
        void Stop();
    }
}
