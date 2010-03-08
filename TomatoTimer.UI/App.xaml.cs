using System;
using System.Windows;
using TomatoTimer.Core;
using Autofac;

namespace TomatoTimer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Main mainWindow;
        public event EventHandler<EventArgs> AttemptedNewInstance;
        internal void OnAttemptedNewInstance(EventArgs e)
        {
            if (AttemptedNewInstance != null)
            {
                AttemptedNewInstance(this, e);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = CreateContainerBuilder();
            var container = builder.Build();

            base.OnStartup(e);
            ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;

            var timer = container.Resolve<ITimer>();

            mainWindow = new Main(timer);
            AttemptedNewInstance += (sender, args) => mainWindow.PopupWindow();
            mainWindow.Show();
        }

        private static ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();

            var timer = CreateTimerFromSettings();
            builder.RegisterInstance(timer);

            return builder;
        }

        private static ITimer CreateTimerFromSettings()
        {
            // TODO: This Configuration Should be Managed By AppController
            // Load Timings from Configuration.
            var tomatoLen = Settings.Current.User.TomatoTime;
            var breakLen = Settings.Current.User.BreakTime;
            var setBreakLen = Settings.Current.User.SetBreakTime;

            var timer = new CoreTimer(new TimerComponent())
            {
                TomatoTimeSpan = new TimeSpan(0, 0, tomatoLen, 0),
                BreakTimeSpan = new TimeSpan(0, 0, breakLen, 0),
                SetBreakTimeSpan = new TimeSpan(0, 0, setBreakLen, 0)
            };

#if DEBUG
            var time = new TimeSpan(0, 0, 0, 10);
            timer.TomatoTimeSpan = time;
            timer.BreakTimeSpan = time;
            timer.SetBreakTimeSpan = time;
#endif

            return timer;
        }
    }
}