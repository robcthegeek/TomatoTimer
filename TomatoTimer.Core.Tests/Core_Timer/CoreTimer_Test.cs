using System;
using Xunit;
using Moq;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public abstract class CoreTimer_Test
    {
        protected readonly ITimer timer;
        protected readonly Mock<ITimerComponent> timerComponent;
        protected readonly TomatoEventMonitor monitor;

        protected CoreTimer_Test()
        {
            timerComponent = new Mock<ITimerComponent>();
            timer = new CoreTimer(timerComponent.Object);
            monitor = new TomatoEventMonitor(timer);
        }

        #region Utility Methods for Tests
        protected void RaiseTimerComponentStarted()
        {
            timerComponent.Raise(x => x.TimerStarted += null, this, EventArgs.Empty);
        }

        protected void RaiseTimerComponentStopped()
        {
            timerComponent.Raise(x => x.TimerStopped += null, this, EventArgs.Empty);
        }

        protected void StartTomato()
        {
            timer.StartTomato();
            timerComponent.Setup(x => x.Start(timer.TomatoTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, EventArgs.Empty);
        }

        protected void StartBreak()
        {
            timer.StartBreak();
            timerComponent.Setup(x => x.Start(timer.BreakTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, EventArgs.Empty);
        }

        public void StartSetBreak()
        {
            timer.StartSetBreak();
            timerComponent.Setup(x => x.Start(timer.SetBreakTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, EventArgs.Empty);
        }

        protected void AssertTomatoStartedEventRaised(bool assert)
        {
            Assert.Equal(assert, monitor.TomatoStartedEventRaised);
        }

        protected void AssertTomatoEndedEventRaised(bool assert)
        {
            Assert.Equal(assert, monitor.TomatoEndedEventRaised);
        }

        protected void AssertBreakStartedEventRaised(bool assert)
        {
            Assert.Equal(assert, monitor.BreakStartedEventRaised);
        }

        protected void AssertSetBreakStartedEventRaised(bool assert)
        {
            Assert.Equal(assert, monitor.SetBreakStartedEventRaised);
        }

        protected void AssertInterruptedEventRaised(bool assert)
        {
            Assert.Equal(assert, monitor.InterruptedEventRaised);
        }

        public void AssertTimeRemainingIsZero()
        {
            var zero = new TimeSpan(0, 0, 0, 0);
            Assert.Equal(zero, timer.TimeRemaining);
        }

        [Fact]
        public void AssertTimeRemainingFromReturnedTimerComponent()
        {
            var expected = new TimeSpan(0, 0, 42, 0);
            timerComponent.Setup(x => x.Remaining).Returns(expected);
            Assert.Equal(expected, timer.TimeRemaining);
        }
        #endregion
    }
}