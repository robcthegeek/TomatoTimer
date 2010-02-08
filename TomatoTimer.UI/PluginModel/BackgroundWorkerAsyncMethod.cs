using System;
using System.ComponentModel;
using System.Threading;

namespace TomatoTimer.UI.PluginModel
{
    public class BackgroundWorkerAsyncMethod : IAsyncMethod
    {
        private BackgroundWorker worker;

        public void RunAsync()
        {
            worker = new BackgroundWorker();
            worker.DoWork += OnMethodStarted;
            worker.RunWorkerCompleted += OnMethodFinished;
            worker.RunWorkerAsync();
        }

        public event EventHandler MethodStarted;
        protected void OnMethodStarted(object sender, DoWorkEventArgs e)
        {
            if (MethodStarted != null)
                MethodStarted(this, EventArgs.Empty);
        }

        public event EventHandler MethodFinished;
        protected void OnMethodFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            worker = null;
            if (MethodFinished != null)
                MethodFinished(this, EventArgs.Empty);
        }

        public bool Abort()
        {
            worker.CancelAsync();
            return true;
        }

        public void Kill()
        {
            worker.Dispose();
        }
    }
}