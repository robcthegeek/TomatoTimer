using System;
using Xunit;
using Moq;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    /// <summary>
    /// Base Class for Testing of the Timer State Machine.
    /// </summary>
    public abstract class CoreTimer_Tests
    {
        protected readonly ITomatoTimer timer;
        protected readonly Mock<ITimerComponent> timerComponent;
        protected readonly TomatoEventMonitor monitor;

        protected CoreTimer_Tests()
        {
            timerComponent = new Mock<ITimerComponent>();
            timer = new TomatoTimer(timerComponent.Object);
            monitor = new TomatoEventMonitor(timer);
        }

        protected void RaiseTimerComponentStarted()
        {
            timerComponent.Raise(x => x.TimerStarted += null, this, new TimerStartedEventArgs(DateTime.Now));
        }

        protected void RaiseTimerComponentStopped()
        {
            timerComponent.Raise(x => x.TimerStopped += null, this, new TimerStoppedEventArgs(DateTime.Now, TimeSpan.MinValue));
        }

        protected void StartTomato()
        {
            timer.StartTomato();
            timerComponent.Setup(x => x.Start(timer.TomatoTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, new TimerStartedEventArgs(DateTime.Now));
        }

        protected void StartBreak()
        {
            timer.StartBreak();
            timerComponent.Setup(x => x.Start(timer.BreakTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, new TimerStartedEventArgs(DateTime.Now));
        }

        public void StartSetBreak()
        {
            timer.StartSetBreak();
            timerComponent.Setup(x => x.Start(timer.SetBreakTimeSpan));
            timerComponent.Raise(x => x.TimerStarted += null, this, new TimerStartedEventArgs(DateTime.Now));
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

        [Fact]
        public void AssertTimeRemainingFromReturnedTimerComponent()
        {
            var expected = new TimeSpan(0, 0, 42, 0);
            timerComponent.Setup(x => x.Remaining).Returns(expected);
            Assert.Equal(expected, timer.TimeRemaining);
        }
    }
}