using System;

namespace TomatoTimer.Core
{
    public interface ITomatoTimer
    {
        event EventHandler<TimerEventArgs> TomatoStarted;
        event EventHandler<TimerEventArgs> TomatoEnded;
        event EventHandler<TimerEventArgs> BreakStarted;
        event EventHandler<TimerEventArgs> BreakEnded;
        event EventHandler<TimerEventArgs> SetBreakStarted;
        event EventHandler<TimerEventArgs> SetBreakEnded;
        event EventHandler<TimerEventArgs> Interrupted;
        event EventHandler<StateChangeFailedEventArgs> StateChangeFailed;
        event EventHandler<TickEventArgs> Tick;

        TimeSpan TomatoTimeSpan { get; set; }
        TimeSpan BreakTimeSpan { get; set; }
        TimeSpan SetBreakTimeSpan { get; set; }
        TimeSpan TimeRemaining { get; }
        CoreTimer.TimerState State { get; }
        
        bool Running { get; }
        void StartTomato();
        void StartBreak();
        void StartSetBreak();
        void Interrupt();
    }
}