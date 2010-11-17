using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomatoTimer.Core.Tests.Factories
{
    internal class TimerComponentFactory
    {
        public ITimerComponent ThatWorks()
        {
            return With(Create.Timer.ThatWorks(), Create.TimeProvider.ThatWorks());
        }

        public ITimerComponent With(ITimer timer, ICurrentTimeProvider timeProvider)
        {
            return new TimerComponent(timer, timeProvider);
        }
    }
}
