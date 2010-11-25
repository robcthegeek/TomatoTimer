
namespace TomatoTimer.Core.Tests.Factories
{
    internal static class Create
    {
        public static TimerFactory Timer
        {
            get
            {
                return new TimerFactory();
            }
        }

        public static TimerComponentFactory TimerComponent
        {
            get
            {
                return new TimerComponentFactory();
            }
        }

        public static CurrentTimeProviderFactory TimeProvider
        {
            get
            {
                return new CurrentTimeProviderFactory();
            }
        }
    }
}
