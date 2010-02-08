using System;

namespace TomatoTimer.Core
{
    public partial class CoreTimer
    {
        public class StoppedState : TransitionClassBase
        {
            public StoppedState(CoreTimer coreTimer)
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

            public override void OnTransitionTo()
            {
                CoreTimer.TimerComponent.Stop();
            }
        }
    }
}