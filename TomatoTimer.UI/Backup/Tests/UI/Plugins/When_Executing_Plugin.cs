using System;
using System.Diagnostics;
using System.Threading;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Plugins
{
    public class When_Executing_Plugin : With_TimerEvent_Plugin
    {
        // TODO: These Tests are simply not repeatable - need to ACCURATELY test that
        // plugins are executed on different threads!

        public When_Executing_Plugin()
        {
            plugin.Expect(p => p.OnStartTomato()).WhenCalled(
                delegate
                    {
                        Thread.Sleep(5);
                    });
        }

        ~When_Executing_Plugin()
        {
            Thread.Sleep(5);
        }

        [Fact]
        public void Execute_Executes_On_Seperate_Thread()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            collection.Execute(p => p.OnStartTomato());
            stopwatch.Stop();
            Assert.True(stopwatch.Elapsed < new TimeSpan(0, 0, 1),
                        "PluginCollection.Execute Did Not Seem to Execute on Different Thread.");
        }

        [Fact]
        public void ExecutingPlugins_Is_One_When_Executing_Plugin()
        {
            collection.Execute(p => p.OnStartTomato());
            Assert.Equal(1, collection.ExecutingPlugins.Count);
        }

        [Fact] public void ExecutingPlugins_Returns_To_Zero_When_Plugin_Completed_Operation()
        {
            collection.Execute(p => p.OnStartTomato());
            Thread.Sleep(10);
            Assert.Equal(0, collection.ExecutingPlugins.Count);
        }

        [Fact]
        public void Abort_Following_Execute_Clears_ExecutingPlugins()
        {
            collection.Execute(p => p.OnStartTomato());
            plugin.Expect(p => p.Abortable).Return(true);
            collection.Abort();
            Assert.Equal(0, collection.ExecutingPlugins.Count);
        }
    }
}