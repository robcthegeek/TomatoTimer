using System;
using System.ComponentModel;
using Rhino.Mocks;
using Xunit;

namespace TomatoTimer.Tests.Unit.UI.Plugins
{
    public class AsyncMethodManagerTests : AsyncMethodManagerTest
    {
        void ExecuteAbortableMethod ()
        {
            manager.ExecuteAsync(plugin, (mc, c) => mc.AbortableMethod(c));    
        }

        [Fact]
        public void ExecuteAsync_Calls_Method_RunWorkerAsync()
        {
            ExecuteAbortableMethod();
            method.VerifyAllExpectations();
        }

        [Fact]
        public void ExecuteAsync_SignsUpFor_Method_MethodStarted()
        {
            ExecuteAbortableMethod();
            method.AssertWasCalled(m => m.MethodStarted += Arg<EventHandler>.Is.Anything);
        }

        [Fact]
        public void ExecuteAsync_SignsUpFor_Method_MethodFinished()
        {
            ExecuteAbortableMethod();
            method.AssertWasCalled(m => m.MethodFinished += Arg<EventHandler>.Is.Anything);
        }

        [Fact]
        public void ExecuteAsync_Increases_RunningCount()
        {
            var count = 0;
            ExecuteAbortableMethod();
            count = manager.RunningCount;
            Assert.Equal(1, count);
        }

        [Fact]
        public void Abort_Does_Not_Call_Method_Abort_If_Not_Executed()
        {
            manager.Abort();
            method.AssertWasNotCalled(m => m.Abort());
        }

        [Fact]
        public void Kill_Calls_Method_Kill()
        {
            ExecuteAbortableMethod();
            manager.Kill();
            method.AssertWasCalled(m => m.Kill());
        }

        [Fact]
        public void Kill_Raises_Killing_Event_When_Methods_Executing()
        {
            var raised = false;
            manager.Killing += (sender, args) => raised = true;
            ExecuteAbortableMethod();
            manager.Kill();
            Assert.True(raised);
        }

        [Fact]
        public void Killing_Event_Not_Raised_If_No_Methods_Executing()
        {
            var raised = false;
            manager.Killing += (sender, args) => raised = true;
            manager.Kill();
            Assert.False(raised);
        }

        [Fact]
        public void Killed_Event_Raised_When_Methods_Killed()
        {
            var raised = false;
            manager.Killed += (sender, args) => raised = true;
            ExecuteAbortableMethod();
            manager.Kill();
            Assert.True(raised);
        }

        [Fact]
        public void Killed_Event_Not_Raised_When_No_Methods_Killed()
        {
            var raised = false;
            manager.Killed += (sender, args) => raised = true;
            manager.Kill();
            Assert.False(raised);
        }
    }
}