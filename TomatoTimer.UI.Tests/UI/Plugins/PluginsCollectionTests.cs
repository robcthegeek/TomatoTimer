using System.Collections.Generic;
using TomatoTimer.UI.PluginModel;
using Xunit;
using Moq;
using System;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class PluginsCollectionTests
    {
        private readonly Mock<DummyPlugin> plugin;
        private readonly Mock<IAsyncMethodManager<DummyPlugin>> manager;
        private readonly PluginsCollection<DummyPlugin> collection;

        public PluginsCollectionTests()
        {
            plugin = new Mock<DummyPlugin>();
            manager = new Mock<IAsyncMethodManager<DummyPlugin>>();
            collection = new PluginsCollection<DummyPlugin>(manager.Object) { plugin.Object };
        }

        [Fact]
        public void Inited_With_Collection_Collection_Counts_Equal()
        {
            var plugins = new List<DummyPlugin> { plugin.Object };
            var ctorColl = new PluginsCollection<DummyPlugin>(manager.Object, plugins);
            Assert.Equal(plugins.Count, ctorColl.Count);
        }

        [Fact]
        public void Clear_Sets_Collection_Count_To_Zero()
        {
            collection.Clear();
            Assert.Equal(0, collection.Count);
        }

        [Fact]
        public void Mangager_Is_Initialised()
        {
            Assert.NotNull(collection.Manager);
        }

        [Fact]
        public void Execute_Calls_Manager_ExecuteAsync()
        {
            manager.Setup(m => m.ExecuteAsync(It.IsAny<DummyPlugin>(), It.IsAny<Action<DummyPlugin, ExecutionContext>>()));
            collection.Execute((p,c) => p.AbortableMethod(c));
            manager.VerifyAll();
        }
    }
}
