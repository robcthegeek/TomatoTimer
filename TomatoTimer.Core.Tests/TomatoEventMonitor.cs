using System;
using TomatoTimer.Core;

namespace TomatoTimer.Core.Tests
{
    /// <summary>
    /// Class for Tracking Events Raised from an ITimer.
    /// </summary>
    /// <seealso cref="ITimer"/>
    public class TomatoEventMonitor
    {
        private readonly ITimer timer;
        public bool TomatoStartedEventRaised { get; private set; }
        public bool TomatoEndedEventRaised { get; private set; }
        public bool BreakStartedEventRaised { get; private set; }
        public bool BreakEndedEventRaised { get; private set; }
        public bool SetBreakStartedEventRaised { get; private set; }
        public bool SetBreakEndedEventRaised { get; private set; }
        public bool InterruptedEventRaised { get; private set; }

        public TimerEventArgs TimerEventArgs { get; private set; }


        public StateChangeFailedEventArgs StateChangeFailedEventArgs { get; private set; }

        public TomatoEventMonitor(ITimer timerToMonitor)
        {
            timer = timerToMonitor;
            
            timer.TomatoStarted += TomatoStartedHandler;
            timer.TomatoEnded += TomatoEndedHandler;
            timer.BreakStarted += BreakStartedHandler;
            timer.BreakEnded += BreakEndedHandler;
            timer.SetBreakStarted += SetBreakStartedHandler;
            timer.SetBreakEnded += SetBreakEndedHandler;
            timer.Interrupted += InterruptedHandler;
            timer.StateChangeFailed += StateChangeFailedHandler;
        }

        void StateChangeFailedHandler(object sender, StateChangeFailedEventArgs e)
        {
            StateChangeFailedEventArgs = e;
        }

        public void ClearEvents()
        {
            TomatoStartedEventRaised = false;
            TomatoEndedEventRaised = false;
            BreakStartedEventRaised = false;
            BreakEndedEventRaised = false;
            SetBreakStartedEventRaised = false;
            SetBreakEndedEventRaised = false;
            InterruptedEventRaised = false;
        }

        private void TomatoEndedHandler(object sender, EventArgs e)
        {
            TomatoEndedEventRaised = true;
        }

        private void InterruptedHandler(object sender, EventArgs e)
        {
            InterruptedEventRaised = true;
        }

        private void SetBreakStartedHandler(object sender, EventArgs e)
        {
            SetBreakStartedEventRaised = true;
        }

        private void BreakStartedHandler(object sender, EventArgs e)
        {
            BreakStartedEventRaised = true;
        }

        private void SetBreakEndedHandler(object sender, EventArgs e)
        {
            SetBreakEndedEventRaised = true;
        }

        private void BreakEndedHandler(object sender, EventArgs e)
        {
            BreakEndedEventRaised = true;
        }

        private void TomatoStartedHandler(object sender, TimerEventArgs e)
        {
            TomatoStartedEventRaised = true;
            TimerEventArgs = e;
        }
    }
}