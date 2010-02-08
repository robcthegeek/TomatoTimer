using System;
using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests
{
    public abstract class TimeSpanExtensionsTest
    {
        protected TimeSpan TimeSpan;

        protected string ToFriendlyValue
        {
            get { return TimeSpan.ToFriendly().ToLower(); }
        }
    }

    public class when_timespan_is_zero :TimeSpanExtensionsTest
    {
        [Fact]
        public void zero_timespan_tofriendly_returns_0_seconds()
        {
            TimeSpan = TimeSpan.Zero;
            Assert.Contains("0 seconds", ToFriendlyValue);
        }
    }

    public class hour_segment_tests : TimeSpanExtensionsTest
    {
        [Fact]
        public void one_hour_tofriendly_returns_1_hour()
        {
            TimeSpan = new TimeSpan(0, 1, 0, 0);
            Assert.Contains("1 hour", ToFriendlyValue);
        }

        [Fact]
        public void two_hours_tofriendly_returns_2_hours()
        {
            TimeSpan = new TimeSpan(0, 2, 0, 0);
            Assert.Contains("2 hours", ToFriendlyValue);
        }

        [Fact]
        public void zero_hours_does_not_return_hour()
        {
            TimeSpan = new TimeSpan(0, 0, 10, 0);
            Assert.DoesNotContain("hour", ToFriendlyValue);
        }
    }

    public class minute_segment_tests : TimeSpanExtensionsTest
    {
        [Fact]
        public void one_min_tofriendly_returns_1_minute()
        {
            TimeSpan = new TimeSpan(0, 0, 1, 0);
            Assert.Contains("1 minute", ToFriendlyValue);
        }

        [Fact]
        public void two_mins_tofriendly_returns_2_minutes()
        {
            TimeSpan = new TimeSpan(0, 0, 2, 0);
            Assert.Contains("2 minutes", ToFriendlyValue);
        }

        [Fact]
        public void zero_minutes_does_not_return_minute()
        {
            TimeSpan = new TimeSpan(0, 0, 0, 42);
            Assert.DoesNotContain("minute", ToFriendlyValue);
        }
    }

    public class seconds_segment_tests : TimeSpanExtensionsTest
    {
        [Fact]
        public void one_sec_tofriendly_returns_1_second()
        {
            TimeSpan = new TimeSpan(0, 0, 0, 1);
            Assert.Contains("1 second", ToFriendlyValue);
        }

        [Fact]
        public void two_secs_tofriendly_returns_2_seconds()
        {
            TimeSpan = new TimeSpan(0, 0, 0, 2);
            Assert.Contains("2 seconds", ToFriendlyValue);
        }

        [Fact]
        public void zero_seconds_does_not_return_seconds()
        {
            TimeSpan = new TimeSpan(0, 1, 0, 0);
            Assert.DoesNotContain("second", ToFriendlyValue);
        }

        [Fact]
        public void less_than_second_returns_less_than_a_second()
        {
            TimeSpan = new TimeSpan(0, 0, 0, 0, 5);
            Assert.Equal("less than a second", ToFriendlyValue);
        }

        [Fact]
        public void seventy_seconds_returns_1_min_and_10_seconds()
        {
            TimeSpan = new TimeSpan(0, 0, 0, 70);
            Assert.Equal("1 minute and 10 seconds", ToFriendlyValue);
        }
    }

    public class composite_tests : TimeSpanExtensionsTest
    {
        [Fact]
        public void two_components_are_and_together()
        {
            TimeSpan = new TimeSpan(0, 0, 42, 10);
            Assert.Equal("42 minutes and 10 seconds", ToFriendlyValue);
        }
        
        [Fact]
        public void three_components_are_comma_then_and_together()
        {
            TimeSpan = new TimeSpan(0, 1, 32, 10);
            Assert.Equal("1 hour, 32 minutes and 10 seconds", ToFriendlyValue);
        }

        [Fact]
        public void one_hour_12_seconds_returns_1_hour_12_seconds()
        {
            TimeSpan = new TimeSpan(0, 1, 0, 12);
            Assert.Equal("1 hour and 12 seconds", ToFriendlyValue);
        }
    }
}