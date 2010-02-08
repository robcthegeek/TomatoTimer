using System;
using System.ComponentModel;
using TomatoTimer.UI.PluginModel;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class AsyncMethodManagerTest
    {
        protected readonly IAsyncMethodManager<DummyPlugin> manager;
        protected readonly IAsyncMethod method;
        protected readonly DummyPlugin plugin;

        public AsyncMethodManagerTest()
        {
            plugin = MockRepository.GenerateMock<DummyPlugin>();
            method = MockRepository.GenerateMock<IAsyncMethod>();

            // Set Up Stub
            method.Expect(m => m.RunAsync()).Do(
                new Action(() => method.Raise(m => m.MethodStarted += null, this, new DoWorkEventArgs(null))));

            plugin.Expect(p => p.AbortableMethod(null))
                .IgnoreArguments()
                .CallOriginalMethod(OriginalCallOptions.NoExpectation);

            plugin.Expect(p => p.NonAbortableMethod(null))
                .IgnoreArguments()
                .CallOriginalMethod(OriginalCallOptions.NoExpectation);

            manager = new AsyncMethodManager<DummyPlugin>();
            manager.AsyncMethod = method;
        }

        protected bool MethodAbortable
        {
            set
            {
                if (value)
                    method.Expect(m => m.Abort()).Return(true);
            }
        }
    }
}
