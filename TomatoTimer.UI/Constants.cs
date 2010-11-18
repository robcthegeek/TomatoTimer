using System;
using TomatoTimer.Core.Helpers;

namespace TomatoTimer.UI
{
    internal static class Constants
    {
        public const string APP_TITLE = "Tomato Timer";
        public const string CONTACT_EMAIL = "robcthegeek.public@gmail.com";

        public static TimeSpan TomatoTime      { get { return 25.Minutes(); } }
        public static TimeSpan BreakTime       { get { return 5.Minutes();  } }
        public static TimeSpan SetBreakTime    { get { return 30.Minutes(); } }
    }
}
