using System;

namespace Leonis.TomatoTimer.Core
{
    public partial class CoreTimer : ITimer
    {
        private ITimerComponent TimerComponent { get; set; }
        
        #region Events
        /// <summary>
        /// Raised When a Tomato is Started.
        /// </summary>
        public event EventHandler TomatoStarted;
        private void OnTomatoStarted(EventArgs e)
        {
            if (TomatoStarted != null)
            {
                TomatoStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When a Tomato has Ended.
        /// </summary>
        public event EventHandler TomatoEnded;
        internal void OnTomatoEnded(EventArgs e)
        {
            if (TomatoEnded != null)
            {
                TomatoEnded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When a Break is Started.
        /// </summary>
        public event EventHandler BreakStarted;
        internal void OnBreakStarted(EventArgs e)
        {
            if (BreakStarted != null)
            {
                BreakStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When a Break has Ended.
        /// </summary>
        public event EventHandler BreakEnded;
        protected void OnBreakEnded(EventArgs e)
        {
            if (BreakEnded != null)
            {
                BreakEnded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When a Set (Long) Break is Started.
        /// </summary>
        public event EventHandler SetBreakStarted;
        internal void OnSetBreakStarted(EventArgs e)
        {
            if (SetBreakStarted != null)
            {
                SetBreakStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When a Set (Long) Break has Ended.
        /// </summary>
        public event EventHandler SetBreakEnded;
        protected void OnSetBreakEnded(EventArgs e)
        {
            if (SetBreakEnded != null)
            {
                SetBreakEnded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When the Timer has Been Interrupted/Cancelled.
        /// </summary>
        public event EventHandler Interrupted;
        internal void OnInterrupted(EventArgs e)
        {
            if (Interrupted != null)
            {
                Interrupted(this, EventArgs.Empty);
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
                Tick(this, e);
            }
        }
        #endregion

        #region States
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

        #endregion

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
            var args = new TickEventArgs(e.TimeElapsed, e.TimeRemaining);
            OnTick(args);
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