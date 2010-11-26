using System;

namespace TomatoTimer.Core.Plugins
{
    public interface IAsyncMethod
    {
        event EventHandler<AsyncMethodStartedEventArgs> MethodStarted;
        event EventHandler<AsyncMethodCompletedEventArgs> MethodCompleted;
        event EventHandler<AsyncMethodCancelledEventArgs> MethodCancelled;
        void Run();
        void Cancel();
    }
}