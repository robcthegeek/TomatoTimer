using System;

namespace TomatoTimer.Core
{
    public partial class TomatoTimer
    {
        public class TomatoCompletedState : TransitionClassBase
        {
            public TomatoCompletedState(TomatoTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override string StateName
            {
                get { return "TomatoCompletedState"; }
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.tomatoCompletedState; }
            }

            public override void OnTransitionTo()
            {
                CoreTimer.OnTomatoEnded();
            }
        }
    }
}