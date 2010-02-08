using System;
using System.ComponentModel;
using Rhino.Mocks;
using Xunit;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class AsyncMethodMangerTest_When_Abortable : AsyncMethodManagerTest
    {
        public AsyncMethodMangerTest_When_Abortable()
        {
            MethodAbortable = true;
        }

        [Fact]
        public void RunningCount_Inits_At_Zero()
        {
            Assert.Equal(0, manager.RunningCount);
        }

        [Fact]
        public void RunningCount_Returns_To_Zero_After_ExecuteAsync()
        {
            manager.ExecuteAsync(plugin, (m, c) => m.AbortableMethod(c));
            method.Raise(m => m.MethodFinished += null, this, EventArgs.Empty);
            Assert.Equal(0, manager.RunningCount);
        }

        [Fact]
        public void Abort_Calls_Method_Abort()
        {
            manager.ExecuteAsync(plugin, (p, c) => p.AbortableMethod(c));
            manager.Abort();
            method.AssertWasCalled(m => m.Abort());
        }

        [Fact]
        public void Abort_Raises_Aborting_Event()
        {
            var raised = false;
            manager.Aborting += (sender, args) => raised = true;
            manager.ExecuteAsync(plugin, (p, c) => p.AbortableMethod(c));
            manager.Abort();

            Assert.True(raised);
        }

        [Fact]
        public void Abort_Raises_Aborted_When_Methods_Aborted()
        {
            var raised = false;
            manager.Aborted += (sender, args) => raised = true;
            manager.ExecuteAsync(plugin, (p, c) => p.AbortableMethod(c));
            manager.Abort();
            Assert.True(raised);
        }
    }
}