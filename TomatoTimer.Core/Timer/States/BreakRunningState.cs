using System;

namespace TomatoTimer.Core
{
    public partial class TomatoTimer
    {
        public class BreakRunningState : TimerState
        {
            public BreakRunningState(TomatoTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.breakRunningState; }
            }

            public override void StartTomato()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.tomatoRunningState, "Cannot Start Tomato While on a Break");
            }

            public override void StartBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.breakRunningState, "Break is Already Running.");
            }

            public override void StartSetBreak()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.setBreakRunningState, "Unable to Start Set Break - A Break is Already Running.");
            }

            public override void Interrupt()
            {
                CoreTimer.State = CoreTimer.timerStoppedState;
                CoreTimer.OnInterrupted();
            }

            public override bool Running
            {
                get
                {
                    return true;
                }
            }

            protected override void OnTimerComponentStarted()
            {
                throw new InvalidOperationException(
                    "Timer Component Has Reported Starting When It Should Already Be Running for a Break.");
            }

            protected override void OnTimerComponentStopped()
            {
                CoreTimer.OnBreakEnded();
                CoreTimer.State = CoreTimer.timerStoppedState;
            }

            public override void OnTransitionTo()
            {
                CoreTimer.OnBreakStarted();
            }
        }
    }
}
