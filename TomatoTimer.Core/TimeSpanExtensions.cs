using System;
using System.Collections.Generic;
using System.Text;

namespace TomatoTimer.Core
{
    public static class TimeSpanExtensions
    {
        public static string ToFriendly(this TimeSpan ts)
        {
            if (ts == TimeSpan.Zero)
                return "0 seconds";
            if (ts < new TimeSpan(0, 0, 0, 1))
                return "Less than a second";

            // Create a Stack for the Values
            var output = new Stack<string>();

            string seconds = PluralTimeValBasedOnInt(ts.Seconds, "second", "seconds");
            StackNonEmptyVal(output, seconds);

            string minutes = PluralTimeValBasedOnInt(ts.Minutes, "minute", "minutes");
            StackNonEmptyVal(output, minutes);
            
            string hours = PluralTimeValBasedOnInt(ts.Hours, "hour", "hours");
            StackNonEmptyVal(output, hours);

            var rtn = BuildStringFromOutputStack(output);
            return rtn;
        }

        private static string BuildStringFromOutputStack(Stack<string> output)
        {
            var sb = new StringBuilder();

            while (output.Count > 0)
            {
                var s = output.Pop();
                sb.Append(" ");
                sb.Append(s);

                // Determine if there is another Item in the Stack to Add Punctuation
                if (output.Count <= 0) continue;
                var lastItem = output.Count == 1;
                var punc = lastItem ? " and" : ",";
                sb.Append(punc);
            }
            return sb.ToString().Trim();
        }

        static void StackNonEmptyVal(Stack<string> stack, string value)
        {
            if (!string.IsNullOrEmpty(value))
                stack.Push(value);
        }

        static string PluralTimeValBasedOnInt(int value, string valueIfOne, string valueIfMany)
        {
            if (value < 1)
                return string.Empty;
            
            var s = (value == 1) ? valueIfOne : valueIfMany;
            return string.Format("{0} {1}", value, s);
        }
    }
}
