using System;
using TomatoTimer.Core.Helpers;
using Xunit;

namespace TomatoTimer.Core.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void TimeSpanBetween_Returns_Zero_For_Same_DateTime()
        {
            var now = DateTime.Now;
            var expected = TimeSpan.Zero;
            var actual = now.TimeSpanBetween(now);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeSpanBetween_Returns_10s_For_10s_Difference()
        {
            var now = DateTime.Now;
            var nowPlus10 = now.Add(new TimeSpan(0, 0, 10));
            var expected = new TimeSpan(0, 0, 10);
            var actual = now.TimeSpanBetween(nowPlus10);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeSpanBetween_Returns_10s_For_Negative10s_Difference()
        {
            var now = DateTime.Now;
            var nowMinus10 = now.Subtract(new TimeSpan(0, 0, 10));
            var expected = new TimeSpan(0, 0, 10);
            var actual = now.TimeSpanBetween(nowMinus10);
            Assert.Equal(expected, actual);
        }
    }
}
