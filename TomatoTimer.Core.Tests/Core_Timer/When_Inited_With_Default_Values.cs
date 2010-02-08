using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Core_Timer
{
    public class When_Inited_With_Default_Values : CoreTimer_Test
    {
        [Fact]
        public void tomato_timespan_defaults_to_25_min()
        {
            var ts = timer.TomatoTimeSpan;
            Assert.Equal(TestConstants.TomatoTimeSpan, ts);
        }

        [Fact]
        public void break_timespan_defaults_to_5_min()
        {
            var ts = timer.BreakTimeSpan;
            Assert.Equal(TestConstants.BreakTimeSpan, ts);
        }

        [Fact]
        public void setbreak_timespan_defaults_to_30_min()
        {
            var ts = timer.SetBreakTimeSpan;
            Assert.Equal(TestConstants.SetBreakTimeSpan, ts);
        }

        [Fact]
        public void state_is_timerstoppedstate()
        {
            Assert.True(timer.State is CoreTimer.StoppedState);
        }

        [Fact]
        public void running_is_false()
        {
            Assert.False(timer.Running);
        }

        [Fact]
        public void timeremaining_is_zero()
        {
            AssertTimeRemainingIsZero();
        }

        [Fact]
        public void timercomponent_started_throws_invalid_op()
        {
            Assert.Throws<InvalidOperationException>(() => RaiseTimerComponentStarted());
        }
    }
}