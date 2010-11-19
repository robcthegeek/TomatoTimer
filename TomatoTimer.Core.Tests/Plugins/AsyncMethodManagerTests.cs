using System;
using TomatoTimer.Core.Plugins;
using Xunit;

namespace TomatoTimer.Core.Tests.Plugins
{
    public class AsyncMethodManagerTests
    {
        [Fact]
        public void Run_NullAction_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new AsyncMethodManager().Run(null));
            Assert.Contains("Action is Required to Run Async", ex.Message);
            Assert.Equal("action", ex.ParamName);
        }

        [Fact]
        public void Run_ActionPassed_ReturnsAsyncMethodHook()
        {
            Action action = () => { return; };
            var manager = new AsyncMethodManager();
            var hook = manager.Run(action);
            Assert.IsType<AsyncMethodHook>(hook);
        }

        [Fact]
        public void Run_ActionPassed_MethodIsExecuted()
        {
            var run = false;
            Action action = () => run = true;
            var manager = new AsyncMethodManager();
            var hook = manager.Run(action);
            Assert.True(run);
        }

        [Fact]
        public void Run_ActionPassed_SetsActionPropertyOnHooke()
        {
            Action action = () => { return; };
            var manager = new AsyncMethodManager();
            var hook = manager.Run(action);
            Assert.Same(action, hook.Action);
        }
    }
}
