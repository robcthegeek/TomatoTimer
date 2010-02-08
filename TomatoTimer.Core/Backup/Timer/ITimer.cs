using System;

namespace Leonis.TomatoTimer.Core
{
    public interface ITimer
    {
        event EventHandler TomatoStarted;
        event EventHandler TomatoEnded;
        event EventHandler BreakStarted;
        event EventHandler BreakEnded;
        event EventHandler SetBreakStarted;
        event EventHandler SetBreakEnded;
        event EventHandler Interrupted;
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