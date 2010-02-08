using System.Collections.Generic;
using TomatoTimer.UI.PluginModel;
using Rhino.Mocks;
using Xunit;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class PluginsCollectionTests
    {
        private readonly DummyPlugin plugin;
        private readonly IAsyncMethodManager<DummyPlugin> manager;
        private readonly PluginsCollection<DummyPlugin> collection;

        public PluginsCollectionTests()
        {
            plugin = MockRepository.GenerateMock<DummyPlugin>();
            manager = MockRepository.GenerateMock<IAsyncMethodManager<DummyPlugin>>();
            collection = new PluginsCollection<DummyPlugin>(manager) { plugin };
        }

        [Fact]
        public void Inited_With_Collection_Collection_Counts_Equal()
        {
            var plugins = new List<DummyPlugin> { plugin };
            var ctorColl = new PluginsCollection<DummyPlugin>(manager, plugins);
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
            manager.Expect(m => m.ExecuteAsync(null, null)).IgnoreArguments();
            collection.Execute((p,c) => p.AbortableMethod(c));
            manager.VerifyAllExpectations();
        }
    }
}
