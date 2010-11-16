using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_In_TomatoCompletedState : CoreTimer_Tests
    {
        public When_In_TomatoCompletedState()
        {
            StartTomato();
            RaiseTimerComponentStopped();
        }

        [Fact]
        public void State_Is_TomatoCompletedState()
        {
            Assert.True(timer.State is CoreTimer.TomatoCompletedState);
        }

        [Fact]
        public void TomatoEnded_Event_Raised()
        {
            AssertTomatoEndedEventRaised(true);
        }

        [Fact]
        public void TomatoEnded_EventArgs_Set()
        {
            Assert.NotNull(monitor.TimerEventArgs);
        }

        [Fact]
        public void Running_Returns_False()
        {
            Assert.False(timer.Running);
        }

        [Fact]
        public void StartTomato_Sets_TomatoRunningState()
        {
            StartTomato();
            Assert.True(timer.State is CoreTimer.TomatoRunningState);
        }

        [Fact]
        public void StartTomato_Raises_TomatoStarted()
        {
            StartTomato();
            AssertTomatoStartedEventRaised(true);
        }

        [Fact]
        public void StartTomato_Sets_TimerEventArgs()
        {
            StartTomato();
            Assert.NotNull(monitor.TimerEventArgs);
        }

        [Fact]
        public void StartBreak_Raises_BreakStarted()
        {
            StartBreak();
            AssertBreakStartedEventRaised(true);
        }

        [Fact]
        public void StartBreak_Sets_TimerEventArgs()
        {
            StartBreak();
            Assert.NotNull(monitor.TimerEventArgs);
        }

        [Fact]
        public void StartSetBreak_Sets_SetBreakRunningState()
        {
            StartSetBreak();
            Assert.True(timer.State is CoreTimer.SetBreakRunningState);
        }

        [Fact]
        public void StartSetBreak_Sets_TimerEventArgs()
        {
            StartSetBreak();
            Assert.NotNull(monitor.TimerEventArgs);
        }

        [Fact]
        public void StartSetBreak_Raises_SetBreakStarted()
        {
            StartSetBreak();
            AssertSetBreakStartedEventRaised(true);
        }

        [Fact]
        public void Interrupt_Raises_StateChangeFailed()
        {
            timer.Interrupt();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.NotNull(e);
            Assert.True(e.StateFrom is CoreTimer.TomatoCompletedState);
            Assert.True(e.StateTo is CoreTimer.InterruptedState);
        }

        [Fact]
        public void Interrupt_Doesnt_Raise_Interrupted()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(false);
        }

        [Fact]
        public void TimerComponent_Started_Throws_InvalidOpEx()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void TimeRemaining_Returns_Zero()
        {
            Assert.Equal(TimeSpan.Zero, timer.TimeRemaining);
        }
    }
}