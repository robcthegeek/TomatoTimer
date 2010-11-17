using System;

namespace TomatoTimer.Core
{
    public interface ITimerComponent
    {
        event EventHandler<TimerStartedEventArgs> TimerStarted;
        event EventHandler<TimerStoppedEventArgs> TimerStopped;
        event EventHandler<TickEventArgs> Tick;

        TimeSpan Remaining { get; }
        TimeSpan Elapsed { get; }

        void Start(TimeSpan timeSpan);
        void Stop();
    }
}
