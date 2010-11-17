using System;

namespace TomatoTimer.Core
{
    public partial class CoreTimer : ITomatoTimer
    {
        private ITimerComponent TimerComponent { get; set; }
        
        /// <summary>
        /// Raised When a Tomato is Started.
        /// </summary>
        public event EventHandler<TimerEventArgs> TomatoStarted;
        private void OnTomatoStarted()
        {
            if (TomatoStarted != null)
            {
                TomatoStarted(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When a Tomato has Ended.
        /// </summary>
        public event EventHandler<TimerEventArgs> TomatoEnded;
        internal void OnTomatoEnded()
        {
            if (TomatoEnded != null)
            {
                TomatoEnded(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When a Break is Started.
        /// </summary>
        public event EventHandler<TimerEventArgs> BreakStarted;
        internal void OnBreakStarted()
        {
            if (BreakStarted != null)
            {
                BreakStarted(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When a Break has Ended.
        /// </summary>
        public event EventHandler<TimerEventArgs> BreakEnded;
        protected void OnBreakEnded()
        {
            if (BreakEnded != null)
            {
                BreakEnded(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When a Set (Long) Break is Started.
        /// </summary>
        public event EventHandler<TimerEventArgs> SetBreakStarted;
        internal void OnSetBreakStarted()
        {
            if (SetBreakStarted != null)
            {
                SetBreakStarted(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When a Set (Long) Break has Ended.
        /// </summary>
        public event EventHandler<TimerEventArgs> SetBreakEnded;
        protected void OnSetBreakEnded()
        {
            if (SetBreakEnded != null)
            {
                SetBreakEnded(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When the Timer has Been Interrupted/Cancelled.
        /// </summary>
        public event EventHandler<TimerEventArgs> Interrupted;
        internal void OnInterrupted()
        {
            if (Interrupted != null)
            {
                Interrupted(this, new TimerEventArgs(this));
            }
        }

        /// <summary>
        /// Raised When the Timer is Asked to Transition to an Invalid State.<para/>
        /// (e.g. Tomato Running > Break Running - The Tomato Should be Completed OR Interrupted)
        /// </summary>
        public event EventHandler<StateChangeFailedEventArgs> StateChangeFailed;
        private void OnStateChangeFailed(StateChangeFailedEventArgs e)
        {
            if (StateChangeFailed != null)
            {
                StateChangeFailed(this, e);
            }
        }

        /// <summary>
        /// Raised when the internal Timer Component Ticks.
        /// </summary>
        public event EventHandler<TickEventArgs> Tick;
        protected void OnTick(TickEventArgs e)
        {
            if (Tick != null)
            {
                Tick(this, new TickEventArgs(this, e.TimeElapsed, e.TimeRemaining));
            }
        }

        internal TimerState timerStoppedState;
        internal TimerState tomatoRunningState;
        internal TimerState breakRunningState;
        internal TimerState setBreakRunningState;
        internal TimerState interruptedState;
        internal TimerState tomatoCompletedState;

        private TimerState state;
        public TimerState State
        {
            get { return state; }
            internal set
            {
                var wasRunning = state == null ? false : state.Running || state is TransitionClassBase;
                // Rewire the Events to the Appropriate Listener
                if (state != null)
                    state.UnwireFromTimerComponent(TimerComponent);

                state = value;
                state.WireUpToTimerComponent(TimerComponent);
                
                if (wasRunning) /* We are Transitioning States */
                    state.OnTransitionTo();
            }
        }

        public TimeSpan TomatoTimeSpan { get; set; }
        public TimeSpan BreakTimeSpan { get; set; }
        public TimeSpan SetBreakTimeSpan { get; set; }

        public bool Running
        {
            get { return state.Running; }
        }
        public TimeSpan TimeRemaining
        {
            get { return TimerComponent.Remaining; }
        }

        public CoreTimer(ITimerComponent timerComponent)
        {
            RegisterTimerComponent(timerComponent);
            SpinUpStates();
            InitDefaultValues();
        }

        private void InitDefaultValues()
        {
            TomatoTimeSpan = new TimeSpan(0, 0, 25, 0);
            BreakTimeSpan = new TimeSpan(0, 0, 5, 0);
            SetBreakTimeSpan = new TimeSpan(0, 0, 30, 0);
            State = timerStoppedState;
        }

        private void SpinUpStates()
        {
            timerStoppedState = new StoppedState(this);
            tomatoRunningState = new TomatoRunningState(this);
            breakRunningState = new BreakRunningState(this);
            setBreakRunningState = new SetBreakRunningState(this);
            interruptedState = new InterruptedState(this);
            tomatoCompletedState = new TomatoCompletedState(this);
        }

        private void RegisterTimerComponent(ITimerComponent timer)
        {
            TimerComponent = timer;
            TimerComponent.Tick += TimerComponent_Tick;
        }

        void TimerComponent_Tick(object sender, TickEventArgs e)
        {
            OnTick(e);
        }

        public void StartTomato()
        {
            State.StartTomato();
        }

        public void StartBreak()
        {
            State.StartBreak();
        }

        public void StartSetBreak()
        {
            State.StartSetBreak();
        }

        public void Interrupt()
        {
            State.Interrupt();
        }

        internal void StartTimerWithTimeSpan(TimeSpan timeSpan)
        {
            TimerComponent.Start(timeSpan);
        }
    }
}