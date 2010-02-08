using System.Collections.Generic;
using Leonis.TomatoTimer.Plugins;
using Rhino.Mocks;

namespace Leonis.TomatoTimer.Tests.UI.Plugins
{
    public class With_Multiple_Item_IPlugin_Collection : PluginCollectionTest
    {
        private readonly Plugin p1 = MockRepository.GenerateMock<Plugin>();
        private readonly Plugin p2 = MockRepository.GenerateMock<Plugin>();
        private readonly Plugin p3 = MockRepository.GenerateMock<Plugin>();

        protected override List<Plugin> GetPluginsCollection()
        {
            return new List<Plugin> { p1, p2, p3 };
        }
    }
}