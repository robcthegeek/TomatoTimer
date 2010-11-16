using Xunit;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class AsyncMethodMangerTest_When_Not_Abortable : AsyncMethodManagerTest
    {
        public AsyncMethodMangerTest_When_Not_Abortable()
        {
            MethodAbortable = false;
        }

        [Fact]
        public void Abort_Does_Not_Call_Method_Abort_On_NonAbortable_Method()
        {
            manager.ExecuteAsync(plugin.Object, (p, c) => p.NonAbortableMethod(c));
            method.Verify(m => m.Abort());
        }

        [Fact]
        public void Abort_DoesNotRaise_Aborted_For_Non_Aborted_Methods()
        {
            MethodAbortable = false;
            var raised = false;
            manager.Aborted += (sender, args) => raised = true;
            manager.ExecuteAsync(plugin.Object, (p, c) => p.AbortableMethod(c));
            manager.Abort();
            Assert.False(raised);
        }
    }
}