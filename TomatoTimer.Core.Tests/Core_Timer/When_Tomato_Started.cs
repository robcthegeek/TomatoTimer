using System;
using TomatoTimer.Core;
using Rhino.Mocks;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Tomato_Started : CoreTimer_Test
    {
        public When_Tomato_Started()
        {
            StartTomato();
        }

        [Fact]
        void running_returns_true()
        {
            Assert.True(timer.Running);
        }

        [Fact]
        public void starttomato_raises_statechangefailed()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void starttomato_does_not_raise_tomatostarted_event()
        {
            monitor.ClearEvents();
            timer.StartTomato();
            AssertTomatoStartedEventRaised(false);
        }

        [Fact]
        public void startbreak_raises_statechangefailed()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void startbreak_does_not_raise_breakstarted_event()
        {
            timer.StartBreak();
            AssertBreakStartedEventRaised(false);
        }

        [Fact]
        public void startsetbreak_raises_statechangefailed()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void startsetbreak_does_not_raise_setbreakstarted_event()
        {
            timer.StartSetBreak();
            AssertSetBreakStartedEventRaised(false);
        }

        [Fact]
        public void TomatoStarted_Is_Raised()
        {
            AssertTomatoStartedEventRaised(true);
        }

        [Fact]
        public void TomatoStarted_Sets_TimerEventArgs()
        {
            Assert.NotNull(monitor.TimerEventArgs);
        }

        [Fact]
        public void TomatoStarted_Sets_TimerEventArgs_Timer()
        {
            Assert.NotNull(monitor.TimerEventArgs.Timer);
        }

        [Fact]
        public void interrupt_moves_timers_to_interruptedstate()
        {
            timer.Interrupt();
            Assert.True(timer.State is CoreTimer.InterruptedState);
        }

        [Fact]
        public void interrupt_raises_interrupted_event()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(true);
        }

        [Fact]
        public void timercomponentstarted_throws_invalidopex()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void timercomponentstopped_moves_timer_to_tomatocompletedstate()
        {
            RaiseTimerComponentStopped();
            Assert.True(timer.State is CoreTimer.TomatoCompletedState);
        }

        [Fact]
        public void When_TimerComponent_Raises_Tick_Timer_Raises_Tick()
        {
            var raised = false;
            timer.Tick += (sender, args) => raised = true;
            timerComponent.Raise(tc => tc.Tick += null, this, new TickEventArgs(null, TimeSpan.MinValue, TimeSpan.MinValue));
            Assert.True(raised);
        }

        [Fact]
        public void TickArgs_Timer_Is_Not_Null()
        {
            ITimer actual = null;
            timer.Tick += (sender, args) => actual = args.Timer;
            timerComponent.Raise(tc => tc.Tick += null, this, new TickEventArgs(null, TimeSpan.MinValue, TimeSpan.MinValue));
            Assert.NotNull(actual);
        }

        [Fact]
        public void TickArgs_Timer_Is_Timer()
        {
            ITimer actual = null;
            timer.Tick += (sender, args) => actual = args.Timer;
            timerComponent.Raise(tc => tc.Tick += null, this, new TickEventArgs(null, TimeSpan.MinValue, TimeSpan.MinValue));
            Assert.Equal(actual, timer);
        }

        [Fact]
        public void Tick_Returns_Elapsed_Value_From_TimerComponent()
        {
            var actual = TimeSpan.MinValue;
            timer.Tick += (sender, args) => actual = args.TimeElapsed;

            var expected = new TimeSpan(0, 0, 42, 0);
            var expectedArgs = new TickEventArgs(timer, expected, TimeSpan.MinValue);
            timerComponent.Raise(tc => tc.Tick += null, this, expectedArgs);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Tick_Returns_Remaining_Value_From_TimerComponent()
        {
            var actual = TimeSpan.MinValue;
            timer.Tick += (sender, args) => actual = args.TimeRemaining;

            var expected = new TimeSpan(0, 0, 42, 0);
            var expectedArgs = new TickEventArgs(timer, TimeSpan.MinValue, expected);
            timerComponent.Raise(tc => tc.Tick += null, this, expectedArgs);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Tick_Returns_Positive_TimeRemaining_Value()
        {
            var actual = TimeSpan.Zero;
            timer.Tick += (sender, args) => actual = args.TimeRemaining;
            var expected = new TimeSpan(0, 0, 2, 0);
            var expArgs = new TickEventArgs(timer, TimeSpan.MinValue, expected);
            timerComponent.Raise(tc => tc.Tick += null, this, expArgs);
            Assert.True(actual > TimeSpan.Zero,
                        string.Format(
                            "Tick Has Not Raised Expected Positive TimeRemaining Value.\r\nTimeSpan Returned: {0}",
                            actual));
        }

        [Fact]
        public void timeremaining_returned_from_timercomponent()
        {
            AssertTimeRemainingFromReturnedTimerComponent();
        }
    }
}