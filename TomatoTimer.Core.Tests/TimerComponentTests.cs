using TomatoTimer.Core;
using Xunit;
using System;
using System.Threading;
using TomatoTimer.Core.Tests.Factories;
using TomatoTimer.Core.Helpers;

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
            var component = Create.TimerComponent.ThatWorks();
            component.TimerStarted += (sender, args) => raised = true;
            component.Start(5.Minutes());
            Assert.True(raised);
        }

        [Fact]
        public void Start_WhenAlreadyStarted_ThrowsInvalidOperationException()
        {
            var component = Create.TimerComponent.ThatWorks();
            component.Start(5.Minutes());

            var ex = Assert.Throws<InvalidOperationException>(() => component.Start(2.Minutes()));
            Assert.Contains("Timer component is already running. Please Stop before calling Start again.", ex.Message);
        }

        // TODO (RC): Stop without Start Throws InvalidOperationException
        // TODO (RC): Stop w/ Start Raises Stopped Event
        // TODO (RC): Elapsed After Start and Stop is Run Time
        // TODO (RC): Elapsed While Running Continuously Re-Calcs Running Time
        // TODO (RC): Add TimeStarted to TimerComponent.Started EventArgs
        // TODO (RC): Add Elapsed to TimerComponent.Stopped EventArgs
        // TODO (RC): Add TimeStopped to TimerComponent.Stopped EventArgs
    }
}