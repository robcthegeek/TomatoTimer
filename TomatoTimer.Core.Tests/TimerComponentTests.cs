using System;
using TomatoTimer.Core.Helpers;
using TomatoTimer.Core.Tests.Factories;
using Xunit;

namespace TomatoTimer.Core.Tests.Timer_Component
{
    public class TimerComponentTests
    {
        static TimeSpan TimerLength { get { return new TimeSpan(0, 0, 0, 42); } }

        [Fact]
        public void Ctor_NullTimer_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new TimerComponent(null, Create.TimeProvider.ThatWorks()));
            Assert.Contains("Timer Implementation is Required", ex.Message);
            Assert.Equal("timer", ex.ParamName);
        }

        [Fact]
        public void Ctor_NullCurrentTimeProvider_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new TimerComponent(Create.Timer.ThatWorks(), null));
            Assert.Contains("CurrentTimeProvider Implementation is Required", ex.Message);
            Assert.Equal("timeProvider", ex.ParamName);
        }

        [Fact]
        public void Remaining_OnCtor_TimeSpanZero()
        {
            var component = Create.TimerComponent.ThatWorks();
            Assert.Equal(TimeSpan.Zero, component.Remaining);
        }

        [Fact]
        public void Elapsed_OnCtor_TimeSpanZero()
        {
            var component = Create.TimerComponent.ThatWorks();
            Assert.Equal(TimeSpan.Zero, component.Elapsed);
        }

        [Fact]
        public void Start_TimeSpanIsZero_ThrowsArgumentOutOfRangeException()
        {
            var component = Create.TimerComponent.ThatWorks();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                component.Start(TimeSpan.Zero));
            Assert.Contains("Invalid TimeSpan - Please use a value greater than TimeSpan.Zero, less than TimeSpan.MaxValue which when added to the current time does not exceed DateTime.MaxValue", ex.Message);
            Assert.Equal("timeSpan", ex.ParamName);
        }

        [Fact]
        public void Start_TimeSpanIsMinValue_ThrowsArgumentOutOfRangeException()
        {
            var component = Create.TimerComponent.ThatWorks();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                component.Start(TimeSpan.MinValue));
            Assert.Contains("Invalid TimeSpan - Please use a value greater than TimeSpan.Zero, less than TimeSpan.MaxValue which when added to the current time does not exceed DateTime.MaxValue", ex.Message);
            Assert.Equal("timeSpan", ex.ParamName);
        }

        [Fact]
        public void Start_TimeSpanIsMaxValue_ThrowsArgumentOutOfRangeException()
        {
            var component = Create.TimerComponent.ThatWorks();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                component.Start(TimeSpan.MaxValue));
            Assert.Contains("Invalid TimeSpan - Please use a value greater than TimeSpan.Zero, less than TimeSpan.MaxValue which when added to the current time does not exceed DateTime.MaxValue", ex.Message);
            Assert.Equal("timeSpan", ex.ParamName);
        }

        [Fact]
        public void Start_TimeSpanGreaterThanMaxDateTimeSubtractNow_ThrowsArgumentOutOfRangeException()
        {
            var currentTime = new DateTime(2010, 11, 16, 18, 51, 0);
            var maxMinusCurrent = DateTime.MaxValue.Subtract(currentTime);
            var tooBigTimeSpan = maxMinusCurrent.Add(10.Minutes());
            var time = Create.TimeProvider.ThatReturns(currentTime);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), time);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                component.Start(tooBigTimeSpan));
            Assert.Contains("Invalid TimeSpan - Please use a value greater than TimeSpan.Zero, less than TimeSpan.MaxValue which when added to the current time does not exceed DateTime.MaxValue", ex.Message);
            Assert.Equal("timeSpan", ex.ParamName);
        }

        [Fact]
        public void Start_IsStopped_RaisesTimerStartedEvent()
        {
            var raised = false;
            var raisedTimeStarted = DateTime.MinValue;
            var expectedTimeStarted = new DateTime(2010, 11, 17, 6, 34, 0);
            var timeProvider = Create.TimeProvider.ThatReturns(expectedTimeStarted);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), timeProvider);
            component.TimerStarted += (sender, args) =>
                {
                    raised = true;
                    raisedTimeStarted = args.TimeStarted;
                };
            
            component.Start(5.Minutes());

            Assert.True(raised);
            Assert.Equal(expectedTimeStarted, raisedTimeStarted);
        }

        [Fact]
        public void Start_WhenAlreadyStarted_ThrowsInvalidOperationException()
        {
            var component = Create.TimerComponent.ThatWorks();
            component.Start(5.Minutes());

            var ex = Assert.Throws<InvalidOperationException>(() => component.Start(2.Minutes()));
            Assert.Contains("Timer component is already running. Please Stop before calling Start again.", ex.Message);
        }

        [Fact]
        public void Start_WhenNotStarted_ThrowsInvalidOperationException()
        {
            var component = Create.TimerComponent.ThatWorks();

            var ex = Assert.Throws<InvalidOperationException>(() => component.Stop());
            Assert.Contains("Timer component is not running. Please Start before calling Stop.", ex.Message);
        }

        [Fact]
        public void Stop_AfterStart_RaisesTimerStoppedEvent()
        {
            var raised = false;
            var timeStarted = new DateTime(2010, 11, 16, 6, 50, 0);
            var timeStopped = new DateTime(2010, 11, 16, 6, 55, 0);
            var expectedElapsed = 5.Minutes();
            var raisedTimeStopped = DateTime.MinValue;
            var raisedElapsed = TimeSpan.Zero;
            var timeProvider = Create.TimeProvider.MockThatReturns(timeStarted);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), timeProvider.Object);
            component.TimerStopped += (sender, args) =>
                {
                    raisedTimeStopped = args.TimeStopped;
                    raisedElapsed = args.Elapsed;
                    raised = true;
                };

            component.Start(5.Minutes());
            timeProvider.Setup(x => x.Now).Returns(timeStopped);
            component.Stop();
            
            Assert.True(raised);
            Assert.Equal(timeStopped, raisedTimeStopped);
            Assert.Equal(expectedElapsed, raisedElapsed);
        }

        [Fact]
        public void Elapsed_AfterStartAndStop_IsRunTimeNotCurrentTime()
        {
            // Ensures the "Current Time" is Not Queried After Stopping
            var startTime = new DateTime(2010, 11, 17, 6, 0, 0);
            var stopTime = new DateTime(2010, 11, 17, 6, 5, 0);
            var timeAfterStop = new DateTime(2010, 11, 17, 7, 0, 0);
            var expectedElapsed = 5.Minutes();
            var time = Create.TimeProvider.MockThatReturns(startTime);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), time.Object);
            component.Start(5.Minutes());
            time.Setup(x => x.Now).Returns(stopTime);
            component.Stop();
            time.Setup(x => x.Now).Returns(timeAfterStop);
            var elapsed = component.Elapsed;
            Assert.Equal(expectedElapsed, elapsed);
        }

        [Fact]
        public void Elapsed_AfterStart_IsRunningTime()
        {
            // Ensures the "Current Time" is Queried While Running
            var startTime = new DateTime(2010, 11, 17, 6, 0, 0);
            var firstCheck = new DateTime(2010, 11, 17, 6, 5, 0);
            var expectedFirstElapsed = 5.Minutes();
            var secondCheck = new DateTime(2010, 11, 17, 6, 10, 0);
            var expectedSecondElapsed = 10.Minutes();
            var time = Create.TimeProvider.MockThatReturns(startTime);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), time.Object);
            component.Start(5.Minutes());
            time.Setup(x => x.Now).Returns(firstCheck);
            var firstElapsed = component.Elapsed;
            time.Setup(x => x.Now).Returns(secondCheck);
            var secondElapsed = component.Elapsed;
            Assert.Equal(expectedFirstElapsed, firstElapsed);
            Assert.Equal(expectedSecondElapsed, secondElapsed);
        }

        [Fact]
        public void Remaining_Stopped_ReturnsTimeSpanZero()
        {
            var component = Create.TimerComponent.ThatWorks();
            Assert.Equal(TimeSpan.Zero, component.Remaining);
        }

        [Fact]
        public void Remaining_TimerRunning_ReturnsTimeRemaining()
        {
            var startTime = new DateTime(2010, 11, 16, 7, 10, 0);
            var remainingTime = new DateTime(2010, 11, 16, 7, 15, 0);
            var expectedRemaining = 5.Minutes();
            var timeProvider = Create.TimeProvider.MockThatReturns(startTime);
            var component = Create.TimerComponent.With(Create.Timer.ThatWorks(), timeProvider.Object);

            component.Start(10.Minutes());
            timeProvider.Setup(x => x.Now).Returns(remainingTime);
            var actualRemaining = component.Remaining;

            Assert.Equal(expectedRemaining, actualRemaining);
        }

        // TODO (RC): Test for Stop When Internal Timer Ticks and Time > Start Time + TimeSpan Given to TimerComponent
        // TODO (RC): Tick Event Raised
    }
}