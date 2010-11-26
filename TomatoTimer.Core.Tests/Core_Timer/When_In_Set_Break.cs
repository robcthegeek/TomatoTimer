using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_In_Set_Break : CoreTimer_Tests
    {
        public When_In_Set_Break()
        {
            StartSetBreak();
        }

        [Fact]
        public void State_Is_SetBreakRunningState()
        {
            Assert.True(timer.State is TomatoTimer.SetBreakRunningState);
        }

        [Fact]
        public void Running_Is_True()
        {
            Assert.True(timer.Running);
        }

        [Fact]
        public void StartTomato_Raises_StateChangeFailed()
        {
            timer.StartTomato();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is TomatoTimer.SetBreakRunningState);
            Assert.True(e.StateTo is TomatoTimer.TomatoRunningState);
        }

        [Fact]
        public void StartTomato_DoesNotRaise_TomatoStartedEvent()
        {
            timer.StartTomato();
            AssertTomatoStartedEventRaised(false);
        }

        [Fact]
        public void StartBreak_Raises_StateChangeFailed()
        {
            timer.StartBreak();
            var e = monitor.StateChangeFailedEventArgs;
            Assert.True(e.StateFrom is TomatoTimer.SetBreakRunningState);
            Assert.True(e.StateTo is TomatoTimer.BreakRunningState);
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
            Assert.True(e.StateFrom is TomatoTimer.SetBreakRunningState);
            Assert.True(e.StateTo is TomatoTimer.SetBreakRunningState);
        }

        [Fact]
        public void StartSetBreak_DoesNotRaise_SetBreakStartedEvent()
        {
            monitor.ClearEvents();
            timer.StartSetBreak();
            AssertSetBreakStartedEventRaised(false);
        }

        [Fact]
        public void State_AfterInterrupt_StoppedState()
        {
            timer.Interrupt();
            Assert.True(timer.State is TomatoTimer.StoppedState);
        }

        [Fact]
        public void Interrupt_Raises_InterruptedEvent()
        {
            timer.Interrupt();
            AssertInterruptedEventRaised(true);
        }

        [Fact]
        public void TimerComponent_RaisesTimerComponentStarted_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }

        [Fact]
        public void State_TimerComponentRaisesTimerComponentStopped_StoppedState()
        {
            RaiseTimerComponentStopped();
            Assert.True(timer.State is TomatoTimer.StoppedState);
        }

        [Fact]
        public void TimerComponent_RaisesTimerComponentStopped_RaisesSetBreakEndedEvent()
        {
            RaiseTimerComponentStopped();
            Assert.True(monitor.SetBreakEndedEventRaised);
        }

        [Fact]
        public void TimeRemaining_Is_ReturnedFromTimerComponent()
        {
            AssertTimeRemainingFromReturnedTimerComponent();
        }
    }
}