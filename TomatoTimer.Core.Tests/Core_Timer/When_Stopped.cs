using System;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Stopped : CoreTimer_Tests
    {
        public When_Stopped()
        {
            // Nothing to do as Stopped by Default
        }

        [Fact]
        public void State_Is_StoppedState()
        {
            Assert.True(timer.State is TomatoTimer.StoppedState);
        }

        [Fact]
        public void TimeRemaining_Is_Zero()
        {
            Assert.Equal(TimeSpan.Zero, timer.TimeRemaining);
        }

        [Fact]
        public void Running_Is_False()
        {
            Assert.False(timer.Running);
        }

        [Fact]
        public void State_WhenStarted_IsTomatoRunningState()
        {
            StartTomato();
            Assert.True(timer.State is TomatoTimer.TomatoRunningState);
        }

        [Fact]
        public void StartTomato_Raises_TomatoStartedEvent()
        {
            StartTomato();
            AssertTomatoStartedEventRaised(true);
        }

        [Fact]
        public void StartBreak_Raises_BreakStartedEvent()
        {
            StartBreak();
            AssertBreakStartedEventRaised(true);
        }

        [Fact]
        public void State_AfterStartSetBreak_IsSetBreakRunningState()
        {
            StartSetBreak();
            Assert.True(timer.State is TomatoTimer.SetBreakRunningState);
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
            Assert.True(e.StateFrom is TomatoTimer.StoppedState);
            Assert.True(e.StateTo is TomatoTimer.InterruptedState);
        }

        [Fact]
        public void Interrupt_DoesNotRaise_InterruptedEvent()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(false);
        }

        [Fact]
        public void TimerComponent_RaisesComponentStarted_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }
    }
}