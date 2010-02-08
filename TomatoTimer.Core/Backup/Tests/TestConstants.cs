using System;

namespace TomatoTimer.Core.Tests
{
    static class TestConstants
    {
        public static TimeSpan TomatoTimeSpan
        {
            get { return new TimeSpan(0, 0, 25, 0); }
        }

        public static TimeSpan BreakTimeSpan
        {
            get { return new TimeSpan(0, 0, 5, 0); }
        }

        public static TimeSpan SetBreakTimeSpan
        {
            get { return new TimeSpan(0, 0, 30, 0); }
        }
    }
}