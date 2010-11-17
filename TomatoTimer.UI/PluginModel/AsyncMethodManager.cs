using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace TomatoTimer.UI.PluginModel
{
    /// <summary>
    /// Default Concrete Implementation of IAsyncMethodManager&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">Type of Target for Invocation.</typeparam>
    public class AsyncMethodManager<T> : IAsyncMethodManager<T>
    {
        /// <summary>
        /// Raised When an Abort Request has been Filed with the Manager.
        /// </summary>
        public event EventHandler Aborting;
        /// <summary>
        /// Raises the 'Aborting' Event.
        /// </summary>
        protected void OnAborting()
        {
            if (Aborting != null)
            {
                Aborting(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised When an Abort Request has been Completed with the Manager (And All Executing Methods Have Been Cancelled).
        /// </summary>
        public event EventHandler Aborted;
        /// <summary>
        /// Raises the 'Aborted' Event.
        /// </summary>
        protected void OnAborted()
        {
            if (Aborted != null)
                Aborted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised When the Manager has been requested to 'Kill' Executing Methods.
        /// </summary>
        public event EventHandler Killing;
        /// <summary>
        /// Raises the 'Killing' Event.
        /// </summary>
        protected void OnKilling()
        {
            if (Killing != null)
            {
                Killing(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised when a 'Kill' Request has been completed (and methods were killed).
        /// </summary>
        public event EventHandler Killed;
        /// <summary>
        /// Raises the 'Killed' Event.
        /// </summary>
        protected void OnKilled()
        {
            if (Killed != null)
            {
                Killed(this, EventArgs.Empty);
            }
        }

        Dictionary<string, KeyValuePair<IAsyncMethod, ExecutionContext>> RunningMethods { get; set; }

        public IAsyncMethod AsyncMethod { get; set; }

        public AsyncMethodManager()
        {
            RunningMethods = new Dictionary<string, KeyValuePair<IAsyncMethod, ExecutionContext>>();
        }

        /// <summary>
        /// Executes a Method Asynchronously on the given Target.
        /// </summary>
        /// <param name="target">Target Object for Method Invocation</param>
        /// <param name="action">Method to Invoke</param>
        public void ExecuteAsync(T target, Action<T, ExecutionContext> action)
        {
            var guid = Guid.NewGuid().ToString();          
            var method = AsyncMethod ?? new BackgroundWorkerAsyncMethod();

            var dispatcher = Dispatcher.CurrentDispatcher;
            method.MethodStarted += (sender, args) =>
                                      {
                                          var context = new ExecutionContext();
                                          AddRunningMethod(guid, method, context);
                                          dispatcher.Invoke(DispatcherPriority.Background,
                                              new Action(() => action(target, context)));
                                      };

            method.MethodFinished += (sender, args) => RemoveExecutingMethod(guid);
            method.RunAsync();
        }

        private void AddRunningMethod(string guid, IAsyncMethod method, ExecutionContext context)
        {
            RunningMethods.Add(guid, new KeyValuePair<IAsyncMethod, ExecutionContext>(method, context));
        }

        private void RemoveExecutingMethod(string guid)
        {
            RunningMethods.Remove(guid);
        }


        /// <summary>
        /// Number of Methods Currently Executing Asynchronously.
        /// </summary>
        public int RunningCount
        {
            get
            {
                return RunningMethods.Count;
            }
        }

        /// <summary>
        /// Aborts the Currently Executing Asynchronous Methods.
        /// </summary>
        public void Abort()
        {
            OnAborting();

            var guids = new string[RunningMethods.Count];
            RunningMethods.Keys.CopyTo(guids, 0);

            foreach (var guid in guids)
            {
                var method = RunningMethods[guid].Key;
                var context = RunningMethods[guid].Value;
                if (context.Abortable)
                {
                    var aborted = method.Abort();
                    if (aborted)
                        RemoveExecutingMethod(guid);
                }
            }

            if (RunningMethods.Count == 0)
                OnAborted();
        }

        /// <summary>
        /// Kills (Forceably Quit) All Executing Methods (Irrespective of if they are "Abortable" or Not).
        /// </summary>
        public void Kill()
        {
            var guids = new string[RunningMethods.Count];
            RunningMethods.Keys.CopyTo(guids, 0);

            if (guids.Length == 0) return;

            OnKilling();

            foreach (var guid in guids)
            {
                var method = RunningMethods[guid].Key;
                method.Kill();
            }         

            OnKilled();
        }
    }
}