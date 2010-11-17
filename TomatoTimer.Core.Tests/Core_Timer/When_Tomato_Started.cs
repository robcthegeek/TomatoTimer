using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Tomato_Started : CoreTimer_Tests
    {
        public When_Tomato_Started()
        {
            StartTomato();
        }

        [Fact]
        void Running_True()
        {
            Assert.True(timer.Running);
        }

        [Fact]
        public void StartTomato_Raises_StateChangeFailed()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void StartTomato_DoesNotRaise_TomatoStartedEvent()
        {
            monitor.ClearEvents();
            timer.StartTomato();
            AssertTomatoStartedEventRaised(false);
        }

        [Fact]
        public void StartBreak_Raises_StateChangeFailed()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void StartBreak_DoesNotRaise_BreakStartedEvent()
        {
            timer.StartBreak();
            AssertBreakStartedEventRaised(false);
        }

        [Fact]
        public void StartSetBreak_Raises_StateChangeFailed()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.TomatoRunningState);
            Assert.True(e.StateTo is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void StartSetBreak_DoesNotRaise_SetBreakStartedEvent()
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
        public void State_AfterInterrupt_InterruptedState()
        {
            timer.Interrupt();
            Assert.True(timer.State is CoreTimer.InterruptedState);
        }

        [Fact]
        public void Interrupt_Raises_InterruptedEvent()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(true);
        }

        [Fact]
        public void TimerComponentRaises_Throws_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void State_TimerComponentRaisesTimerComponentStopped_TomatoCompletedState()
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
            ITomatoTimer actual = null;
            timer.Tick += (sender, args) => actual = args.Timer;
            timerComponent.Raise(tc => tc.Tick += null, this, new TickEventArgs(null, TimeSpan.MinValue, TimeSpan.MinValue));
            Assert.NotNull(actual);
        }

        [Fact]
        public void TickArgsTimer_Is_TimerInstance()
        {
            ITomatoTimer actual = null;
            timer.Tick += (sender, args) => actual = args.Timer;
            timerComponent.Raise(tc => tc.Tick += null, this, new TickEventArgs(null, TimeSpan.MinValue, TimeSpan.MinValue));
            Assert.Equal(actual, timer);
        }

        [Fact]
        public void Tick_Returns_ElapsedFromTimerComponent()
        {
            var actual = TimeSpan.MinValue;
            timer.Tick += (sender, args) => actual = args.TimeElapsed;
        
            var expected = new TimeSpan(0, 0, 42, 0);
            var expectedArgs = new TickEventArgs(timer, expected, TimeSpan.MinValue);
            timerComponent.Raise(tc => tc.Tick += null, this, expectedArgs);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Tick_Returns_TimeRemainingFromTimerComponent()
        {
            var actual = TimeSpan.MinValue;
            timer.Tick += (sender, args) => actual = args.TimeRemaining;

            var expected = new TimeSpan(0, 0, 42, 0);
            var expectedArgs = new TickEventArgs(timer, TimeSpan.MinValue, expected);
            timerComponent.Raise(tc => tc.Tick += null, this, expectedArgs);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeRemaining_Is_ReturnedFromTimerComponent()
        {
            AssertTimeRemainingFromReturnedTimerComponent();
        }
    }
}