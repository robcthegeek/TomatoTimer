using System;

namespace TomatoTimer.Core
{
    public partial class TomatoTimer
    {
        public class SetBreakRunningState : TimerState
        {
            public SetBreakRunningState(TomatoTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.setBreakRunningState; }
            }

            public override void StartTomato()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.tomatoRunningState, "Cannot Start a Tomato While a Set Break is Still Running.");
            }

            public override void StartBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.breakRunningState, "Unable to Start a Break - A Set Break is Already Running.");
            }

            public override void StartSetBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.setBreakRunningState, "Unable to Start Set Break - One is Already Running.");
            }

            public override void Interrupt()
            {
                CoreTimer.State = CoreTimer.timerStoppedState;
                CoreTimer.OnInterrupted();
            }

            public override bool Running
            {
                get { return true; }
            }

            protected override void OnTimerComponentStarted()
            {
                throw new InvalidOperationException(
                    "Timer Component Has Reported Starting When It Should Already Be Running for a Set Break.");
            }

            protected override void OnTimerComponentStopped()
            {
                CoreTimer.OnSetBreakEnded();
                CoreTimer.State = CoreTimer.timerStoppedState;
            }

            public override void OnTransitionTo()
            {
                CoreTimer.OnSetBreakStarted();
            }
        }
    }
}