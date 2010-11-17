using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomatoTimer.Core
{
    public interface ITimer
    {
        TimeSpan Interval { set; }
        event EventHandler Tick;
        bool IsEnabled { get; }
        void Start();
        void Stop();
    }
}
