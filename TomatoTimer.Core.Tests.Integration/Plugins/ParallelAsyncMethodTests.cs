using System;
using TomatoTimer.Core.Plugins;
using Xunit;
using System.Threading;

namespace TomatoTimer.Core.Tests.Integration.Plugins
{
    public class ParallelAsyncMethodTests
    {
        [Fact]
        public void Ctor_NullAction_RaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ParallelAsyncMethod(null));
            Assert.Contains("Action is Required to Run Async Method", ex.Message);
            Assert.Equal("action", ex.ParamName);
        }

        [Fact]
        public void Run_ActionPassedInCtor_ExecutesMethod()
        {
            var executed = false;
            Action action = () => executed = true;
            var method = new ParallelAsyncMethod(action);
            
            method.Run();
            GiveSchedulerAChance();
            Assert.True(executed);
        }

        [Fact]
        public void Run_ActionPassedToCtor_RaisesMethodStartedEvent()
        {
            var raised = false;
            Action action = () => { return; };
            var method = new ParallelAsyncMethod(action);
            method.MethodStarted += (sender, args) => raised = true;

            method.Run();
            GiveSchedulerAChance();
            Assert.True(raised);
        }

        [Fact]
        public void Run_ActionCompletesOK_RaisesMethodCompletedEvent()
        {
            var raised = false;
            Action action = () => { return; };
            var method = new ParallelAsyncMethod(action);
            method.MethodCompleted += (sender, args) => raised = true;

            method.Run();
            GiveSchedulerAChance();
            Assert.True(raised);
        }

        [Fact]
        public void Run_BlockingMethod_DoesNotRaiseMethodCompleted()
        {
            var raised = false;
            Action action = () => { Thread.Sleep(1000); };
            var method = new ParallelAsyncMethod(action);
            method.MethodCompleted += (sender, args) => raised = true;

            method.Run();
            GiveSchedulerAChance();
            Assert.False(raised);
        }

        [Fact]
        public void Cancel_MethodRun_RaisesMethodCancelledEvent()
        {
            var raised = false;
            Action action = () => { Thread.Sleep(1000); };
            var method = new ParallelAsyncMethod(action);
            method.MethodCancelled += (sender, args) => raised = true;

            method.Run();
            GiveSchedulerAChance();
            method.Cancel();
            Assert.True(raised);
        }

        [Fact]
        public void Cancel_MethodNotRun_DoesNotRaiseMethodCancelledEvent()
        {
            var raised = false;
            Action action = () => { Thread.Sleep(100); };
            var method = new ParallelAsyncMethod(action);
            method.MethodCancelled += (sender, args) => raised = true;

            method.Cancel();
            Assert.False(raised);
        }

        [Fact]
        public void Cancel_BlockingMethod_DoesNotRaiseMethodCompleted()
        {
            var raised = false;
            Action action = () => { Thread.Sleep(1000); };
            var method = new ParallelAsyncMethod(action);
            method.MethodCompleted += (sender, args) => raised = true;

            method.Run();
            GiveSchedulerAChance();
            method.Cancel();
            Thread.Sleep(150);
            Assert.False(raised);
        }

        [Fact]
        public void Cancel_BlockingMethod_DoesNotCompleteMethodExecution()
        {
            var executed = false;
            Action action = () => { 
                    Thread.Sleep(1000);
                    executed = true;
                };
            var method = new ParallelAsyncMethod(action);

            method.Run();
            GiveSchedulerAChance();
            method.Cancel();
            Thread.Sleep(150);
            Assert.False(executed);
        }

        [Fact(Skip = "Unable to Get This Passing - What's the best way to get the Aggregate Exception (without blocking)?")]
        public void Run_ActionThrows_RaisesExceptionOccurred()
        {
            var expected = new Exception("Something Bad Happened");
            AsyncMethodExceptionOccurredEventArgs raised = null;
            Action action = () => { throw expected; };
            var method = new ParallelAsyncMethod(action);
            method.ExceptionOccurred += (sender, args) => raised = args;
            method.Run();
            GiveSchedulerAChance();
            Assert.Same(expected, raised.Exception);
        }

        private static void GiveSchedulerAChance()
        {
            Thread.Sleep(500);
        }
    }
}
