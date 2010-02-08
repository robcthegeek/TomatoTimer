using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Leonis.TomatoTimer.UI
{
    public partial class NotifyIconComponent : Component
    {
        private readonly Window parentWindow;

        #region Events
        /// <summary>
        /// Event Raised When the Tray Icon is Single-Clicked
        /// </summary>
        public event EventHandler<MouseEventArgs> TrayIconClicked;
        protected void OnTrayIconClicked(MouseEventArgs e)
        {
            if (TrayIconClicked != null)
            {
                TrayIconClicked(this, e);
            }
        }

        /// <summary>
        /// Event Raised When the Tray Icon is Double-Clicked (Be Aware That 'TrayIconClicked' Will be Raised First!)
        /// </summary>
        public event EventHandler<EventArgs> TrayIconDoubleClicked;
        protected void OnTrayIconDoubleClicked(EventArgs e)
        {
            if (TrayIconDoubleClicked != null)
            {
                TrayIconDoubleClicked(this, e);
            }
        }

        /// <summary>
        /// Raised When the Balloon is Closed by the User
        /// </summary>
        public event EventHandler<BalloonTipEventArgs> BalloonClosed;
        protected void OnBalloonClosed(BalloonTipEventArgs e)
        {
            if (BalloonClosed != null)
            {
                BalloonClosed(this, e);
            }
        }

        /// <summary>
        /// Raised When the Balloon is Clicked by the User
        /// </summary>
        public event EventHandler<BalloonTipEventArgs> BalloonClicked;
        protected void OnBalloonClicked(BalloonTipEventArgs e)
        {
            if (BalloonClicked != null)
            {
                BalloonClicked(this, e);
            }
        }

        /// <summary>
        /// Raised When the User Moves the Mouse Over the Tray Icon<para/>
        /// (Note There is a 'Cool-Down' Period of 15ms Before Event is Raised Again).
        /// </summary>
        public event EventHandler<EventArgs> TrayIconMouseMoved;
        protected void OnTrayIconMouseMoved(EventArgs e)
        {
            if (TrayIconMouseMoved != null)
            {
                TrayIconMouseMoved(this, e);
            }
        }
        #endregion

        /// <summary>
        /// Currently in the "MouseMove" Cool-Off Period (to Prevent Excessive Event Raising).
        /// </summary>
        private bool coolingDownFromMouseMove;

        /// <summary>
        /// Text Displayed When the User Cursors Over the Tray Icon.
        /// </summary>
        public string IconTitle
        {
            get { return notifyIcon.Text; }
            set
            {
                notifyIcon.Text = value;
            }
        }

        #region Menu Manipulation Methods
        public void ClearMenu() { menu.Items.Clear(); }

        public void AddMenuItem(string title, EventHandler onClick)
        {
            menu.Items.Add(title, null, onClick);
        }
        #endregion

        #region Constructors
        public NotifyIconComponent(Window window)
        {
            InitializeComponent();
            parentWindow = window;
            HookUpWindowEvents();
        }

        public NotifyIconComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        } 
        #endregion

        #region Window Events
        /// <summary>
        /// Registers as a Listener for Parent Window Events.
        /// </summary>
        private void HookUpWindowEvents()
        {
            parentWindow.StateChanged += parentWindow_StateChanged;
            parentWindow.Closed += parentWindow_Closed;
        }

        /// <summary>
        /// Hides the Window from the Taskbar When Minimised.
        /// </summary>
        void parentWindow_StateChanged(object sender, System.EventArgs e)
        {
            if (parentWindow.WindowState == WindowState.Minimized)
                parentWindow.ShowInTaskbar = false;
        }

        /// <summary>
        /// Kills the Component When the Window is Closed.
        /// </summary>
        void parentWindow_Closed(object sender, EventArgs e)
        {
            Dispose();
        } 
        #endregion

        #region NotifyIcon Event Handlers
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnTrayIconDoubleClicked(EventArgs.Empty);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && menu.Items.Count > 0)
                menu.Show();
            else
                OnTrayIconClicked(e);
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            var title = notifyIcon.BalloonTipTitle;
            var text = notifyIcon.BalloonTipText;
            OnBalloonClosed(new BalloonTipEventArgs(title, text));
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            OnBalloonClicked(new BalloonTipEventArgs(
                notifyIcon.BalloonTipTitle, notifyIcon.BalloonTipText));
        }

        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (coolingDownFromMouseMove) return;

            var timer = new DispatcherTimer(
                new TimeSpan(0, 0, 0, 15),
                DispatcherPriority.Normal,
                NotifyIconMouseMoveCallDown,
                Dispatcher.CurrentDispatcher);
            timer.Start();
            OnTrayIconMouseMoved(EventArgs.Empty);
            coolingDownFromMouseMove = true;
        }

        private void NotifyIconMouseMoveCallDown(object sender, EventArgs e)
        {
            coolingDownFromMouseMove = false;
        }
        #endregion

        /// <summary>
        /// Shows a Notification Balloon.
        /// </summary>
        /// <param name="title">Title/Header Text</param>
        /// <param name="message">Message to Display</param>
        /// <param name="timeOut">Time (in Milliseconds) to Display the Balloon</param>
        /// <param name="icon">ToolTipIcon to Display</param>
        public void ShowBalloon(string title, string message, int timeOut, ToolTipIcon icon)
        {
            notifyIcon.ShowBalloonTip(timeOut, title, message, icon);
        }

        /// <summary>
        /// Shows a Notification Balloon (for 100ms with No Icon).
        /// </summary>
        /// <param name="title">Title/Header Text</param>
        /// <param name="message">Message to Display</param>
        public void ShowBalloon(string title, string message)
        {
            ShowBalloon(title, message, 100, ToolTipIcon.None);
        }

        /// <summary>
        /// Shows a Notification Balloon (for 100ms).
        /// </summary>
        /// <param name="title">Title/Header Text</param>
        /// <param name="message">Message to Display</param>
        /// <param name="icon">ToolTipIcon to Display</param>
        public void ShowBalloon(string title, string message, ToolTipIcon icon)
        {
            ShowBalloon(title, message, 100, icon);
        }
    }
}
