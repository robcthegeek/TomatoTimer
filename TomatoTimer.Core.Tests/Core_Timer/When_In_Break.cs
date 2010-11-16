using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_In_Break : CoreTimer_Tests
    {
        public When_In_Break()
        {
            StartBreak();
        }

        [Fact]
        public void Running_Returns_True()
        {
            Assert.True(timer.Running);
        }

        [Fact]
        public void StartTomato_Throws_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => StartTomato());
        }

        [Fact]
        public void StartTomato_Raises_StageChangeFailed()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
        }

        [Fact]
        public void StartTomato_StateChangeFailed_StateFrom_BreakRunningState()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void StartTomato_StateChangeFailed_StateTo_BreakRunningState()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateTo is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void StartTomato_Doesnt_Raise_TomatoStarted()
        {
            timer.StartTomato();
            AssertTomatoStartedEventRaised(false);
        }

        [Fact]
        public void StartBreak_Raises_StateChangeFailed()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
        }

        [Fact]
        public void StartBreak_StateChangeFailed_StateFrom_BreakRunningState()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void StartBreak_StateChangeFailed_StateTo_BreakRunningState()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateTo is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void StartBreak_Does_Not_Raise_BreakStarted_Event()
        {
            monitor.ClearEvents();
            timer.StartBreak();
            AssertBreakStartedEventRaised(false);
        }

        [Fact]
        public void StartSetBreak_Throws_InvalidOpEx()
        {
            Assert.Throws<InvalidOperationException>(() => StartSetBreak());
        }

        [Fact]
        public void StartSetBreak_Raises_StateChangeFailed()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
        }

        [Fact]
        public void StartSetBreak_StateChangeFailed_StateFrom_BreakRunningState()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is CoreTimer.BreakRunningState);
        }

        [Fact]
        public void StartSetBreak_StateChangeFailed_StateTo_SetBreakRunningState()
        {
            timer.StartSetBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateTo is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void StartSetBreak_Doesnt_Raise_SetBreakStarted()
        {
            monitor.ClearEvents();
            timer.StartSetBreak();
            AssertSetBreakStartedEventRaised(false);
        }

        [Fact]
        public void Interrupt_Sets_TimerStoppedState()
        {
            timer.Interrupt();
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void Interrupt_Raises_Interrupted()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(true);
        }

        [Fact]
        public void TimerComponent_Started_Throws_InvalidOpEx()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void TimerComponentStopped_Sets_TimerStoppedState()
        {
            RaiseTimerComponentStopped();
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void TimerComponentStopped_Raises_BreakEnded()
        {
            RaiseTimerComponentStopped();
            Assert.True(monitor.BreakEndedEventRaised);
        }

        [Fact]
        public void TimeRemaining_Returned_From_TimerComponent()
        {
            AssertTimeRemainingFromReturnedTimerComponent();
        }
    }
}