using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace TomatoTimer.Core
{
    /// <summary>
    /// Default Timer Implementation - Uses <seealso cref="System.Windows.Threading.DispatcherTimer"/>.
    /// </summary>
    public class Timer : ITimer, ICurrentTimeProvider
    {
        private readonly DispatcherTimer timer;

        public Timer()
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Tick += OnTick;
        }
        public TimeSpan Interval
        {
            set { timer.Interval = value; }
        }

        public event EventHandler Tick;
        protected void OnTick(object sender, EventArgs e)
        {
            if (Tick != null)
                Tick(this, null);
        }

        public bool IsEnabled
        {
            get { return timer.IsEnabled; }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
