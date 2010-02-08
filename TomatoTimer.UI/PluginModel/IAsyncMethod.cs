using System;
using System.ComponentModel;

namespace TomatoTimer.UI.PluginModel
{
    /// <summary>
    /// Implementation That Allows Asynchronous Methods to be Tested By Mirroring BackgroundWorker Interface.
    /// </summary>
    public interface IAsyncMethod
    {
        /// <summary>
        /// Begin the Asynchronous Invocation.
        /// </summary>
        void RunAsync();

        /// <summary>
        /// Event Raised When Asynchronous Work Begins.
        /// </summary>
        event EventHandler MethodStarted;

        /// <summary>
        /// Event Raised When Asynchronous Work has been Completed.
        /// </summary>
        event EventHandler MethodFinished;

        /// <summary>
        /// Aborts the Current Asynchronous Method.
        /// </summary>
        bool Abort();

        /// <summary>
        /// Kills the Current Asynchronous Method (This Should Be Guaranteed By the Implementation).
        /// </summary>
        void Kill();
    }
}
