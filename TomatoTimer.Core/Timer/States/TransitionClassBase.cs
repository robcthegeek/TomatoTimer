using System;

namespace TomatoTimer.Core
{
    public partial class TomatoTimer
    {
        public abstract class TransitionClassBase : TimerState
        {
            private TimerState newStateOnStart;

            protected abstract string StateName { get; }

            public TransitionClassBase(TomatoTimer coreTimer)
                : base(coreTimer)
            {
            }

            public override bool Running
            {
                get { return false; }
            }

            public override void StartTomato()
            {
                newStateOnStart = CoreTimer.tomatoRunningState;
                CoreTimer.StartTimerWithTimeSpan(CoreTimer.TomatoTimeSpan);
            }

            public override void StartBreak()
            {
                newStateOnStart = CoreTimer.breakRunningState;
                CoreTimer.StartTimerWithTimeSpan(CoreTimer.BreakTimeSpan);
            }

            public override void StartSetBreak()
            {
                newStateOnStart = CoreTimer.setBreakRunningState;
                CoreTimer.StartTimerWithTimeSpan(CoreTimer.SetBreakTimeSpan);
            }

            public override void Interrupt()
            {
                RaiseStateChangeFailedEvent(
                    CoreTimer.interruptedState, string.Format(
                    "Unable to Interrupt {0} - No Timer is Running in this State", StateName));
            }

            protected override void OnTimerComponentStarted()
            {
                if (newStateOnStart == null)
                    throw new InvalidOperationException(string.Format(
                        "TimerComponent Raised Started Event to {0} - But No Transition State Has Been Set.", StateName));

                CoreTimer.State = newStateOnStart;
            }

            protected override void OnTimerComponentStopped()
            {
                // Timer Can Be Stopped in Transition
                //throw new InvalidOperationException(string.Format(
                //    "Timer Component was Stopped in {0} When It Should Not Have Been Running.", StateName));
            }
        }
    }
}