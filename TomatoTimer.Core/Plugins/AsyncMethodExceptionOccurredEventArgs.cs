using System;

namespace TomatoTimer.Core.Plugins
{
    public class AsyncMethodExceptionOccurredEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public AsyncMethodExceptionOccurredEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}
