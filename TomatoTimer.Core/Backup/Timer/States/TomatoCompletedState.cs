using System;

namespace Leonis.TomatoTimer.Core
{
    public partial class CoreTimer
    {
        public class TomatoCompletedState : TransitionClassBase
        {
            public TomatoCompletedState(CoreTimer coreTimer)
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
                CoreTimer.TimerComponent.Stop();
                CoreTimer.OnTomatoEnded(EventArgs.Empty);
            }
        }
    }
}