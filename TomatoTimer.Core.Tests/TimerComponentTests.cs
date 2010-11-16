using TomatoTimer.Core;
using Xunit;
using System;
using System.Threading;

namespace TomatoTimer.Core.Tests.Timer_Component
{
    public class TimerComponentTests
    {
        static TimeSpan TimerLength { get { return new TimeSpan(0, 0, 0, 42); } }

        protected ITimerComponent timerComponent;

        public TimerComponentTests()
        {
            timerComponent = new TimerComponent();
            timerComponent.Start(TimerLength);
        }

        [Fact]
        public void Remaining_OnCtor_TimeSpanZero()
        {
            var component = new TimerComponent();
            Assert.Equal(TimeSpan.Zero, component.Remaining);
        }

        [Fact]
        public void Elapsed_OnCtor_TimeSpanZero()
        {
            var component = new TimerComponent();
            Assert.Equal(TimeSpan.Zero, component.Elapsed);
        }

        // TODO (RC): Start Raises Started Event
        // TODO (RC): Start 2nd Time Throws InvalidOperationException
        // TODO (RC): Start w/ TimeSpan.Zero Throws ArgumentException
        // TODO (RC): Stop without Start Throws InvalidOperationException
        // TODO (RC): Stop w/ Start Raises Stopped Event
        // TODO (RC): Elapsed After Start and Stop is Run Time
        // TODO (RC): Elapsed While Running Continuously Re-Calcs Running Time
        // TODO (RC): Add TimeStarted to TimerComponent.Started EventArgs
        // TODO (RC): Add Elapsed to TimerComponent.Stopped EventArgs
        // TODO (RC): Add TimeStopped to TimerComponent.Stopped EventArgs

        // TODO (RC): Delete Everything Below this Comment!

        [Fact]
        public void Elapsed_Increases_As_Time_Passes()
        {
            var before = timerComponent.Elapsed;
            Thread.Sleep(10);
            var after = timerComponent.Elapsed;
            Assert.True(after > before,
                        string.Format("Elapsed Time Not Incremented Following Start of TimerComponent. Before: {0} After {1}", before, after));
        }

        [Fact]
        public void Remaining_Decreases_As_Time_Passes()
        {
            var before = timerComponent.Remaining;
            Thread.Sleep(10);
            var after = timerComponent.Remaining;
            Assert.True(after < before,
                        string.Format("Remaining Time Not Decreased Following Start of TimerComponent. Before: {0} After {1}", before, after));
        }

        [Fact]
        public void Remaining_Returns_Zero_Once_Stopped()
        {
            Thread.Sleep(10);
            timerComponent.Stop();
            Assert.Equal(TimeSpan.Zero, timerComponent.Remaining);
        }

        [Fact]
        public void Elapsed_Is_Not_Zero_Once_Stopped()
        {
            Thread.Sleep(10);
            timerComponent.Stop();
            Assert.NotEqual(TimeSpan.Zero, timerComponent.Elapsed);
        }

        [Fact]
        public void Elapsed_Remains_Static_Once_Stopped()
        {
            Thread.Sleep(10);
            timerComponent.Stop();
            var expected = timerComponent.Elapsed;
            Thread.Sleep(10);
            var actual = timerComponent.Elapsed;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Elapsed_Is_Reset()
        {
            Thread.Sleep(20);
            timerComponent.Stop();
            var existingElapsed = timerComponent.Elapsed;
            timerComponent.Start(TimerLength);
            Thread.Sleep(10);
            timerComponent.Stop();
            var actual = timerComponent.Elapsed;
            Assert.True(actual < existingElapsed,
                        string.Format("Elapsed Does Not Appear to be Reset Following Start/Stop. Actual/Existing Elapsed: {0}/{1}", actual, existingElapsed));
        }

        [Fact]
        public void Elapsed_Is_Postive()
        {
            Thread.Sleep(5);
            Assert.True(timerComponent.Elapsed > TimeSpan.Zero,
                        "Elapsed is Less Than TimeSpan.Zero (Should be Greater When Running)");
        }

        [Fact]
        public void Elapsed_Is_Postive_When_Stopped()
        {
            Thread.Sleep(5);
            timerComponent.Stop();
            Assert.True(timerComponent.Elapsed > TimeSpan.Zero,
                        "Elapsed is Less Than TimeSpan.Zero (Should be Greater Following a Stop After Running)");
        }

        [Fact]
        public void Remaining_Is_Postive()
        {
            Thread.Sleep(5);
            Assert.True(timerComponent.Remaining > TimeSpan.Zero,
                        "Remaining is Less Than TimeSpan.Zero (Should be Greater When Running)");
        }
    }
}