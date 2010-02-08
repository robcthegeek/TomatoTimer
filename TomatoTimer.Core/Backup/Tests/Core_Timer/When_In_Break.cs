using System;
using Leonis.TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_In_Break : CoreTimer_Test
    {
        public When_In_Break()
        {
            StartBreak();
        }

        [Fact]
        public void running_returns_true()
        {
            Assert.True(timer.Running);
        }

        [Fact]
        public void starttomato_throws_invalidopex()
        {
            Assert.Throws<InvalidOperationException>(() => StartTomato());
        }

        [Fact]
        public void starttomato_raises_statechangefailed()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
            Assert.True(e.StateTo is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void starttomato_does_not_raise_tomatostarted_event()
        {
            timer.StartTomato();
            AssertTomatoStartedEventRaised(false);
        }

        [Fact]
        public void startbreak_raises_statechangefailed()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
            Assert.True(e.StateTo is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void startbreak_does_not_raise_breakstarted_event()
        {
            monitor.ClearEvents();
            timer.StartBreak();
            AssertBreakStartedEventRaised(false);

        }

        [Fact]
        public void startsetbreak_throws_invalidopex()
        {
            Assert.Throws<InvalidOperationException>(() => StartSetBreak());
        }

        [Fact]
        public void startsetbreak_raises_statechangefailed()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
            Assert.True(e.StateTo is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void startsetbreak_does_not_raise_setbreakstarted_event()
        {
            monitor.ClearEvents();
            timer.StartSetBreak();
            AssertSetBreakStartedEventRaised(false);
        }

        [Fact]
        public void interrupt_moves_timer_to_timerstoppedstate()
        {
            timer.Interrupt();
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void interrupt_raises_interrupted_event()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(true);
        }

        [Fact]
        public void timercomponent_started_throws_invalidopex()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void timercomponentstopped_moves_timer_to_timerstoppedstate()
        {
            RaiseTimerComponentStopped();
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void timercomponentstopped_raises_breakendedevent()
        {
            RaiseTimerComponentStopped();
            Assert.True(monitor.BreakEndedEventRaised);
        }

        [Fact]
        public void timeremaining_returned_from_timercomponent()
        {
            AssertTimeRemainingFromReturnedTimerComponent();
        }
    }
}