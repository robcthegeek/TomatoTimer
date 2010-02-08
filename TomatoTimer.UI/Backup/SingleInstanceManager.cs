using System;
using Microsoft.VisualBasic.ApplicationServices;

namespace Leonis.TomatoTimer.UI
{
    // Taken From: http://my.safaribooksonline.com/9781430210849/create_a_single-instance_application#snippet
    public class SingleInstanceManager : WindowsFormsApplicationBase
    {
        private App app;

        public SingleInstanceManager()
        {
            // Disable Single Instance if Debugging.
#if !DEBUG
            IsSingleInstance = true;
#endif
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            base.OnStartup(eventArgs);

            app = new App();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            app.OnAttemptedNewInstance(EventArgs.Empty);
        }
    }
}