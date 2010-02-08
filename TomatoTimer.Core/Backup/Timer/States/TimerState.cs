namespace Leonis.TomatoTimer.Core
{
    public partial class CoreTimer
    {
        public abstract class TimerState
        {
            protected CoreTimer CoreTimer { get; private set; }

            protected TimerState(CoreTimer coreTimer)
            {
                CoreTimer = coreTimer;
            }

            internal void WireUpToTimerComponent(ITimerComponent timerComponent)
            {
                timerComponent.TimerStarted += TimerComponent_TimerStarted;
                timerComponent.TimerStopped += TimerComponent_TimerStopped;
            }

            internal void UnwireFromTimerComponent(ITimerComponent timerComponent)
            {
                timerComponent.TimerStarted -= TimerComponent_TimerStarted;
                timerComponent.TimerStopped -= TimerComponent_TimerStopped;
            }

            void TimerComponent_TimerStopped(object sender, System.EventArgs e)
            {
                OnTimerComponentStopped();
            }

            void TimerComponent_TimerStarted(object sender, System.EventArgs e)
            {
                OnTimerComponentStarted();
            }

            protected abstract TimerState CurrentState { get; }
            protected void RaiseStateChangeFailedEvent(TimerState transitioningTo, string message)
            {
                CoreTimer.OnStateChangeFailed(
                    new StateChangeFailedEventArgs(CurrentState, transitioningTo, message));
            }

            // Generic Timer Actions
            public abstract void StartTomato();
            public abstract void StartBreak();
            public abstract void StartSetBreak();
            public abstract void Interrupt();
            public abstract bool Running { get; }

            // Methods Used by Timer for Events Raising
            public virtual void OnTransitionTo()
            {
                // This Method is Called When the State is Set in TransitionClassBase.
            }

            // Actions in Response to TimerComponent Events.
            protected abstract void OnTimerComponentStarted();
            protected abstract void OnTimerComponentStopped();
        }
    }
}
