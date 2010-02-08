using System;

namespace TomatoTimer.Core
{
    public partial class CoreTimer
    {
        public class InterruptedState : TimerState
        {
            public InterruptedState(CoreTimer coreTimer)
                : base(coreTimer)
            {
            }

            protected override TimerState CurrentState
            {
                get { return CoreTimer.interruptedState; }
            }

            public override void StartTomato()
            {
                ResetTimerState();
                CoreTimer.StartTomato();
            }

            public override void StartBreak()
            {
                ResetTimerState();
                CoreTimer.StartBreak();
            }

            public override void StartSetBreak()
            {
                ResetTimerState();
                CoreTimer.StartSetBreak();
            }

            public override void Interrupt()
            {
                ResetTimerState();
                CoreTimer.Interrupt();
            }

            public override bool Running
            {
                get { return false; }
            }

            protected override void OnTimerComponentStarted()
            {
                throw new InvalidOperationException(
                    "Timer Component Has Been Started When in Interrupted State.");
            }

            protected override void OnTimerComponentStopped()
            {
                // Do Nothing - Timer Component will be Stopped as Interrupted.
            }

            private void ResetTimerState()
            {
                CoreTimer.State = CoreTimer.timerStoppedState;
            }

            public override void OnTransitionTo()
            {
                CoreTimer.TimerComponent.Stop();
                CoreTimer.OnInterrupted();
            }
        }
    }
}