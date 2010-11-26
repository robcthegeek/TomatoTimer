using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Inited_With_Default_Values : CoreTimer_Tests
    {
        [Fact]
        public void TomatoTimeSpan_25min()
        {
            var ts = timer.TomatoTimeSpan;
            Assert.Equal(TestConstants.TomatoTimeSpan, ts);
        }

        [Fact]
        public void BreakTimeSpan_5min()
        {
            var ts = timer.BreakTimeSpan;
            Assert.Equal(TestConstants.BreakTimeSpan, ts);
        }

        [Fact]
        public void SetBreakTimeSpan_30min()
        {
            var ts = timer.SetBreakTimeSpan;
            Assert.Equal(TestConstants.SetBreakTimeSpan, ts);
        }

        [Fact]
        public void State_StopppedState()
        {
            Assert.True(timer.State is TomatoTimer.StoppedState);
        }

        [Fact]
        public void Running_False()
        {
            Assert.False(timer.Running);
        }

        [Fact]
        public void TimeRemaining_Is_Zero()
        {
            Assert.Equal(TimeSpan.Zero, timer.TimeRemaining);
        }

        [Fact]
        public void TimerComponent_RaisesComponentStarted_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }
    }
}