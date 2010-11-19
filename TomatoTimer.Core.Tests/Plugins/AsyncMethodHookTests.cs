using System;
using TomatoTimer.Core.Plugins;
using Xunit;

namespace TomatoTimer.Core.Tests.Plugins
{
    public class AsyncMethodHookTests
    {
        [Fact]
        public void Ctor_NullAction_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new AsyncMethodHook(null));
            Assert.Contains("Action is Required to Create a Hook to it!", ex.Message);
            Assert.Equal("action", ex.ParamName);
        }

        [Fact]
        public void Run_ActionPassed_RaisesMethodStarted()
        {
            Action action = () => { return; };
            var hook = new AsyncMethodHook(action);
            var raised = false;
            hook.MethodStarted += (sender, args) => raised = true;
            hook.Run();
            Assert.True(raised);
        }

        [Fact]
        public void Run_ActionCompletedExecution_RaisesMethodCompleted()
        {
            Action action = () => { return; };
            var hook = new AsyncMethodHook(action);
            var raised = false;
            hook.MethodCompleted += (sender, args) => raised = true;
            hook.Run();
            Assert.True(raised);
        }
    }
}