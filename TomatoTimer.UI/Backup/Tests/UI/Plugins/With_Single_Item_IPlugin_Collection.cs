using System.Collections.Generic;
using Leonis.TomatoTimer.Plugins;
using Rhino.Mocks;

namespace Leonis.TomatoTimer.Tests.UI.Plugins
{
    public class With_Single_Item_IPlugin_Collection : PluginCollectionTest
    {
        private readonly Plugin pluginOne = MockRepository.GenerateMock<Plugin>();

        protected override List<Plugin> GetPluginsCollection()
        {
            return new List<Plugin> { pluginOne };
        }
    }
}