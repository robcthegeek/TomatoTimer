using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomatoTimer.Core.Helpers
{
    public static class TimeSpanHelpers
    {
        public static TimeSpan Hours(this int value)
        {
            return new TimeSpan(value, 0, 0);
        }

        public static TimeSpan Minutes(this int value)
        {
            return new TimeSpan(0, value, 0);
        }

        public static TimeSpan Seconds(this int value)
        {
            return new TimeSpan(0, 0, value);
        }
    }
}
