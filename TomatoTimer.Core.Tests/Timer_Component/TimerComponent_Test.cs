using TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests.Timer_Component
{
    public class TimerComponent_Test
    {
        protected ITimerComponent timerComponent;

        public TimerComponent_Test()
        {
            timerComponent = new TimerComponent();
        }
    }
}