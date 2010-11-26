using System;

namespace TomatoTimer.Core
{
    public partial class TomatoTimer
    {
        public class StoppedState : TransitionClassBase
        {
            public StoppedState(TomatoTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override string StateName
            {
                get { return "StoppedState"; }
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.timerStoppedState; }
            }
        }
    }
}