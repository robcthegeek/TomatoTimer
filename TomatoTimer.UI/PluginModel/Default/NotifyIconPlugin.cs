using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TomatoTimer.Core;
using TomatoTimer.Plugins;
using TomatoTimer.UI.Settings;

namespace TomatoTimer.UI.PluginModel.Default
{
    [Export(typeof(TimerEventPlugin))]
    public class NotifyIconPlugin : TimerEventPlugin
    {
        private NotifyIconComponent notify;
        private TimeSpan timeRemaining = TimeSpan.Zero;
        private Main mainWindow;

        public NotifyIconPlugin()
        {
            SetupNotifyIcon();
        }

        private void SetupNotifyIcon()
        {
            // HACK: Need to Refactor the Core App Code so the Main Window Reference is
            // not passed around - Add AppController and bind to events from that.
            mainWindow = Main.GetInstance();
            notify = new NotifyIconComponent(mainWindow)
                         {
                             IconTitle = Constants.APP_TITLE
                         };
            notify.TrayIconDoubleClicked += notify_TrayIconDoubleClicked;
            notify.TrayIconMouseMoved += notify_TrayIconMouseMoved;

            SetupTransitionMenu();
        }

        void SetupTransitionMenu()
        {
            notify.ClearMenu();
            notify.AddMenuItem("Start &Tomato", Menu_StartTomato);
            notify.AddMenuItem("Start &Break", Menu_StartBreak);
            notify.AddMenuItem("Start &Set Break", Menu_StartSetBreak);
            notify.AddMenuItem("-", null);
            notify.AddMenuItem("Exit", Menu_Exit);
        }

        void SetupRunningMenu()
        {
            notify.ClearMenu();
            notify.AddMenuItem("Interrupt", Menu_Interrupt);
        }

        #region Menu Event Handlers
        private void Menu_Interrupt(object sender, EventArgs e)
        {
            mainWindow.Timer.Interrupt();
        }

        private void Menu_StartSetBreak(object sender, EventArgs e)
        {
            mainWindow.Timer.StartSetBreak();
        }

        private void Menu_StartBreak(object sender, EventArgs e)
        {
            mainWindow.Timer.StartBreak();
        }

        void Menu_StartTomato(object sender, EventArgs e)
        {
            mainWindow.Timer.StartTomato();
        } 
        #endregion

        private void Menu_Exit(object sender, EventArgs e)
        {
            mainWindow.Close();
        }

        void notify_TrayIconDoubleClicked(object sender, EventArgs e)
        {
            mainWindow.PopupWindow();
        }

        void notify_TrayIconMouseMoved(object sender, EventArgs e)
        {
            ShowTimeRemainingBalloon();
        }

        private void ShowTimeRemainingBalloon()
        {
            notify.ShowBalloon("Tomato Timer",
                string.Format("Tomato Time Remaining: {0}", timeRemaining.ToFriendly()));
        }

        public override void OnStartTomato()
        {
            notify.ShowBalloon("Tomato Started", "Stay Focused!", 250, ToolTipIcon.Info);
            SetupRunningMenu();
        }

        public override void OnEndTomato()
        {
            notify.ShowBalloon("Tomato Completed", "Relax, Take a Break or Set Break - Good Job!", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        public override void OnStartBreak()
        {
            SetupRunningMenu();
        }

        public override void OnEndBreak()
        {
            notify.ShowBalloon("Break Ended", "Ready to destroy another Tomato?", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        public override void OnStartSetBreak()
        {
            SetupRunningMenu();
        }

        public override void OnEndSetBreak()
        {
            notify.ShowBalloon("Set Break Ended", "Hope you enjoyed the break..", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        public override void OnInterrupt()
        {
            notify.ShowBalloon("Interrupted", "Tomato/Break/Set Break Interrupted!");
            SetupTransitionMenu();
        }

        public override void OnTick(TickEventArgs args)
        {
            // Nothing to Do Here...
            timeRemaining = args.TimeRemaining;
        }
    }
}
