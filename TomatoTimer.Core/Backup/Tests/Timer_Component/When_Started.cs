using System;
using System.Threading;
using Xunit;

namespace TomatoTimer.Core.Tests.Timer_Component
{
    public class When_Started : TimerComponent_Test
    {
        TimeSpan TimerLength { get { return new TimeSpan(0, 0, 0, 42); } }

        public When_Started()
        {
            timerComponent.Start(TimerLength);
        }

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
                        string.Format("Elapsed Does Not Appear to be Reset Following Start/Stop. Elapsed: {0}", actual));
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