using System;

namespace Leonis.TomatoTimer.Core
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Subtracts 'date1' from 'date2' to Determine the TimeSpan Between the Two Dates.
        /// </summary>
        /// <param name="date1">Date to Subtract.</param>
        /// <param name="date2">Date to Subtract FROM.</param>
        /// <returns>Difference Between the Two Dates as a TimeSpan.<para />
        /// If the TimeSpan Between the Two is a Negative Value, it is made Postive.</returns>
        public static TimeSpan TimeSpanBetween(this DateTime date1, DateTime date2)
        {
            TimeSpan diff = date2.Subtract(date1);

            return (diff < TimeSpan.Zero) ?
                TimeSpan.Zero.Subtract(diff): diff;
        }
    }
}
