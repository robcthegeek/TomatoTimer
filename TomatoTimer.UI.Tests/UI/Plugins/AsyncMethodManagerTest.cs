using System;
using System.ComponentModel;
using TomatoTimer.UI.PluginModel;
using Moq;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class AsyncMethodManagerTest
    {
        protected readonly IAsyncMethodManager<DummyPlugin> manager;
        protected readonly Mock<IAsyncMethod> method;
        protected readonly Mock<DummyPlugin> plugin;

        public AsyncMethodManagerTest()
        {
            plugin = new Mock<DummyPlugin>();
            method = new Mock<IAsyncMethod>(MockBehavior.Strict);

            method.Setup(m => m.RunAsync()).Raises(
                m => m.MethodStarted += null, this, new DoWorkEventArgs(null));

            plugin.Setup(p => p.AbortableMethod(It.IsAny<ExecutionContext>()));
            plugin.Setup(p => p.NonAbortableMethod(It.IsAny<ExecutionContext>()));

            manager = new AsyncMethodManager<DummyPlugin>();
            manager.AsyncMethod = method.Object;
        }

        protected bool MethodAbortable
        {
            set
            {
                if (value)
                    method.Setup(m => m.Abort()).Returns(true);
            }
        }
    }
}
