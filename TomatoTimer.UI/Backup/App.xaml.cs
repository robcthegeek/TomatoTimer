using System;
using System.Windows;

namespace Leonis.TomatoTimer.UI
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

            base.OnStartup(e);
            ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
            
            mainWindow = new Main();
            AttemptedNewInstance += (sender, args) => mainWindow.PopupWindow();
            mainWindow.Show();
        }
    }
}