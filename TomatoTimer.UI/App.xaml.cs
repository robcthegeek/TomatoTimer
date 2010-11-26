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

            var timer = container.Resolve<ITomatoTimer>();

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

        private static ITomatoTimer CreateTimerFromSettings()
        {
            // HACK: This Configuration Should be Managed By AppController
            var tomatoLen = Constants.TomatoTime;
            var breakLen = Constants.BreakTime;
            var setBreakLen = Constants.SetBreakTime;

            var dispatcherTimer = new Timer();
            var timer = new TomatoTimer.Core.TomatoTimer(new TimerComponent(dispatcherTimer, dispatcherTimer))
            {
                TomatoTimeSpan = tomatoLen,
                BreakTimeSpan = breakLen,
                SetBreakTimeSpan = setBreakLen
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