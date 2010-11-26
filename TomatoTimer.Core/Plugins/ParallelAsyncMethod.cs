using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TomatoTimer.Core.Plugins
{
    public class ParallelAsyncMethod : IAsyncMethod
    {
        public event EventHandler<AsyncMethodStartedEventArgs> MethodStarted;
        // TODO (RC): Check with P&P - Is This the Expected Pattern?
        protected void RaiseMethodStarted()
        {
            var args = new AsyncMethodStartedEventArgs();
            if (MethodStarted != null)
            {
                MethodStarted(this, args);
            }
        }

        public event EventHandler<AsyncMethodCompletedEventArgs> MethodCompleted;
        protected void RaiseMethodCompleted()
        {
            var args = new AsyncMethodCompletedEventArgs();
            if (MethodCompleted != null)
            {
                MethodCompleted(this, args);
            }
        }

        public event EventHandler<AsyncMethodCancelledEventArgs> MethodCancelled;
        protected void RaiseMethodCancelled()
        {
            var args = new AsyncMethodCancelledEventArgs();
            if (MethodCancelled != null)
            {
                MethodCancelled(this, args);
            }
        }

        private readonly CancellationTokenSource cancellationSource;
        private readonly TaskFactory taskFactory;
        private readonly Action action;
        private Task task;

        public ParallelAsyncMethod(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(
                    "action", "Action is Required to Run Async Method");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            this.cancellationSource = cancellationTokenSource;
            this.taskFactory = new TaskFactory(cancellationToken);
            this.action = () =>
            {
                RaiseMethodStarted();
                action();
                RaiseMethodCompleted();
            };
        }

        public void Run()
        {
            task = taskFactory.StartNew(action);
        }

        public void Cancel()
        {
            if (task != null && task.Status == TaskStatus.Running)
            {
                cancellationSource.Cancel();
                RaiseMethodCancelled();
            }
        }
    }
}
