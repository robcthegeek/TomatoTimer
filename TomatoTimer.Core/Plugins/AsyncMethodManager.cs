using System;

namespace TomatoTimer.Core.Plugins
{
    public class AsyncMethodManager
    {
        public AsyncMethodHook Run(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(
                    "action", "Action is Required to Run Async");

            var hook = new AsyncMethodHook(action);
            action();
            return hook;
        }
    }   
}
