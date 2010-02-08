using System;

namespace TomatoTimer.UI.PluginModel
{
    public interface IAsyncMethodManager<T>
    {

        /// <summary>
        /// Raised When an 'Abort' Request has been Filed with the Manager.
        /// </summary>
        event EventHandler Aborting;

        /// <summary>
        /// Raised When an Abort Request has been Completed with the Manager (And All Executing Methods Have Been Cancelled).
        /// </summary>
        event EventHandler Aborted;

        /// <summary>
        /// Raised When a 'Kill' Request has been Filed with the Manager. 
        /// </summary>
        event EventHandler Killing;

        /// <summary>
        /// Raised When a 'Kill' Request has been Completed (And Executing Methods were Killed). 
        /// </summary>
        event EventHandler Killed;

        /// <summary>
        /// Executes a Method Asynchronously on the given Target.
        /// </summary>
        /// <param name="target">Target Object for Method Invocation</param>
        /// <param name="action">Method to Invoke</param>
        void ExecuteAsync(T target, Action<T, ExecutionContext> action);

        /// <summary>
        /// Number of Methods Currently Executing Asynchronously.
        /// </summary>
        int RunningCount { get; }

        /// <summary>
        /// TODO: This is to be removed once IoC Container in Place.
        /// </summary>
        IAsyncMethod AsyncMethod { get; set; }

        /// <summary>
        /// Aborts the Currently Executing Asynchronous Methods.
        /// </summary>
        void Abort();

        /// <summary>
        /// Kills (Forceably Quit) All Executing Methods (Irrespective of if they are "Abortable" or Not).
        /// </summary>
        void Kill();
    }
}