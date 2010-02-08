using TomatoTimer.Plugins;
using TomatoTimer.UI.PluginModel;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class DummyPlugin : Plugin
    {
        public virtual void NonAbortableMethod(ExecutionContext context)
        {
            if (context != null)
                context.Abortable = false;
        }

        public virtual void AbortableMethod(ExecutionContext context)
        {
            if (context != null)
                context.Abortable = true;
        }
    }
}