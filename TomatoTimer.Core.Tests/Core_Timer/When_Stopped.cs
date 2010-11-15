using System;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Stopped : CoreTimer_Test
    {
        public When_Stopped()
        {
            // Nothing to do as Stopped by Default
        }

        [Fact]
        public void state_is_timerstoppedstate()
        {
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void timerremaining_returns_zero()
        {
            AssertTimeRemainingIsZero();
        }

        [Fact]
        public void running_returns_false()
        {
            Assert.False(timer.Running);
        }

        [Fact]
        public void starttomato_moves_timer_to_tomatorunningstate()
        {
            StartTomato();
            Assert.True(timer.State is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void starttomato_raises_tomatostarted_event()
        {
            StartTomato();
            AssertTomatoStartedEventRaised(true);
        }

        [Fact]
        public void startbreak_raises_breakstarted_event()
        {
            StartBreak();
            AssertBreakStartedEventRaised(true);
        }

        [Fact]
        public void startsetbreak_moves_timer_to_setbreakrunningstate()
        {
            StartSetBreak();
            Assert.True(timer.State is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void startsetbreak_raises_setbreakstarted_event()
        {
            StartSetBreak();
            AssertSetBreakStartedEventRaised(true);
        }

        [Fact]
        public void interrupt_raises_statechangefailed()
        {
            timer.Interrupt();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.StoppedState);
            Assert.True(e.StateTo is CoreTimer.InterruptedState);
        }

        [Fact]
        public void interrupt_does_not_raise_interrupted_event()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(false);
        }

        [Fact]
        public void timercomponentstarted_with_no_state_throws_invalidopex()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }
    }
}