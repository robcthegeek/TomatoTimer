using System;

namespace Leonis.TomatoTimer.Core
{
    public partial class CoreTimer
    {
        public class TomatoRunningState : TimerState
        {
            public TomatoRunningState(CoreTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.tomatoRunningState; }
            }

            public override void StartTomato()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.tomatoRunningState, "Cannot Start a Tomato - One is Already Running.");
            }

            public override void StartBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.breakRunningState, "Cannot Start Break - A Tomato is Currently Running.");
            }

            public override void StartSetBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.setBreakRunningState, "Cannot Start Set Break - A Tomato is Currently Running.");
            }

            public override void Interrupt()
            {
                CoreTimer.State = CoreTimer.interruptedState;
                CoreTimer.State.OnTransitionTo();
            }

            public override bool Running
            {
                get { return true; }
            }

            protected override void OnTimerComponentStarted()
            {
                throw new InvalidOperationException(
                    "Timer Component Has Reported Starting When Should Already Be Running.");
            }

            protected override void OnTimerComponentStopped()
            {
                CoreTimer.State = CoreTimer.tomatoCompletedState;
            }

            public override void OnTransitionTo()
            {
                CoreTimer.OnTomatoStarted(EventArgs.Empty);
            }
        }
    }
}