using System;
using TimerState = Leonis.TomatoTimer.Core.CoreTimer.TimerState;

namespace Leonis.TomatoTimer.Core
{
    public class StateChangeFailedEventArgs : EventArgs
    {
        public TimerState StateFrom { get; private set; }
        public TimerState StateTo { get; private set; }
        public string Message { get; private set; }

        public StateChangeFailedEventArgs(TimerState stateFrom, TimerState stateTo, string message)
        {
            StateFrom = stateFrom;
            StateTo = stateTo;
            Message = message;
        }
    }
}
