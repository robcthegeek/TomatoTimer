using System.Collections.Generic;
using Leonis.TomatoTimer.Plugins;
using Leonis.TomatoTimer.UI.Plugins;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Plugins
{
    public class With_TimerEvent_Plugin
    {
        protected readonly PluginCollection<TimerEventPlugin> collection;
        protected readonly TimerEventPlugin plugin = MockRepository.GenerateMock<TimerEventPlugin>();

        public With_TimerEvent_Plugin()
        {
            collection = new PluginCollection<TimerEventPlugin>(new List<TimerEventPlugin> {plugin});
        }

        [Fact]
        public void Execute_Executes_OnStartTomato()
        {
            collection.Execute(p => p.OnStartTomato());
        }

        [Fact]
        public void Execute_Executes_OnEndTomato()
        {
            collection.Execute(p => p.OnEndTomato());
        }

        [Fact]
        public void Execute_Executes_OnStartBreak()
        {
            collection.Execute(p => p.OnStartBreak());
        }

        [Fact]
        public void Execute_Executes_OnEndBreak()
        {
            collection.Execute(p => p.OnEndBreak());
        }

        [Fact]
        public void Execute_Executes_OnStartSetBreak()
        {
            collection.Execute(p => p.OnStartSetBreak());
        }

        [Fact]
        public void Execute_Executes_OnEndSetBreak()
        {
            collection.Execute(p => p.OnEndSetBreak());
        }

        [Fact]
        public void Execute_Executes_OnInterrupt()
        {
            collection.Execute(p => p.OnInterrupt());
        }

        [Fact]
        public void ExecutingPlugins_Zero_On_Init()
        {
            Assert.Equal(0, collection.ExecutingPlugins.Count);
        }
    }
}