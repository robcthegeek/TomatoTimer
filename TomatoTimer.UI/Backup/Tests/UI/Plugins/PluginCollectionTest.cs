using System.Collections.Generic;
using Leonis.TomatoTimer.Plugins;
using Leonis.TomatoTimer.UI.Plugins;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Plugins
{
    public abstract class PluginCollectionTest
    {
        protected readonly PluginCollection<Plugin> collection;

        List<Plugin> Plugins { get { return GetPluginsCollection(); } }

        protected PluginCollectionTest()
        {
            collection = new PluginCollection<Plugin>(Plugins);
        }

        protected abstract List<Plugin> GetPluginsCollection();

        [Fact]
        public void Can_Create_Collection_With_Plugins()
        {
            Assert.NotNull(collection);
        }

        [Fact]
        public void Create_With_Plugins_Sets_Plugins_Collection()
        {
            Assert.NotNull(collection.Plugins);
        }

        [Fact]
        public void Create_With_Plugins_Inits_Plugins_With_Same_Count()
        {
            Assert.Equal(GetPluginsCollection().Count, collection.Plugins.Count);
        }

        [Fact]
        public void Abort_Calls_Abort_On_Plugins_When_Abortable()
        {
            foreach (var plugin in Plugins)
            {
                plugin.Expect(p => p.Abortable).Return(true);
                plugin.Expect(p => p.Abort());
            }

            collection.Abort();

            foreach (var plugin in Plugins)
            {
                plugin.VerifyAllExpectations();
            }
        }

        [Fact]
        public void Abort_Does_Not_Call_Abort_On_Plugins_When_Not_Abortable()
        {
            foreach (var plugin in Plugins)
            {
                plugin.Expect(p => p.Abortable).Return(false);
            }

            collection.Abort();

            foreach (var plugin in Plugins)
            {
                plugin.AssertWasNotCalled(p => p.Abort());
            }
        }
    }
}
