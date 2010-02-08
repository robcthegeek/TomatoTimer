using System.Threading;
using TomatoTimer.UI.PluginModel;
using Xunit;

namespace TomatoTimer.UI.Tests.Integration
{
    public class AsyncMethodManagerTests
    {
        private readonly AsyncMethodManager<Plugin> manager;
        private readonly Plugin plugin;

        public AsyncMethodManagerTests()
        {
            manager = new AsyncMethodManager<Plugin>();
            plugin = new Plugin();
        }

        [Fact]
        public void ExecuteAsync_Increments_RunningCount()
        {
            plugin.Blocked = true;
            manager.ExecuteAsync(plugin, (p, c) => p.BlockingMethod());
            Thread.Sleep(10);
            Assert.Equal(1, manager.RunningCount);
            plugin.Blocked = false;
        }

        [Fact]
        public void ExecuteAsync_Decreases_RunningCount_When_Complete()
        {
            manager.ExecuteAsync(plugin, (p, c) => p.EmptyMethod());
            Thread.Sleep(10);
            Assert.Equal(0, manager.RunningCount);
        }
    }

    public class Plugin
    {
        public bool Blocked = false;
        public void EmptyMethod()
        {

        }

        public void BlockingMethod()
        {
            while(Blocked)
            {

            }
        }
    }
}