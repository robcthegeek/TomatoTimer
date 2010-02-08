using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Leonis.TomatoTimer.Core;
using Leonis.TomatoTimer.UI.Graphics;
using Leonis.TomatoTimer.UI.Settings;

namespace Leonis.TomatoTimer.UI
{
    /// <summary>
    /// MiniTimer Window Used to Display the Time Remaining to the User.
    /// </summary>
    public partial class MiniTimer : Window
    {
        private readonly CoreTimer timer;
        
        //private readonly DispatcherTimer uiTimer;
        private const int FADE_DURATION = 250;
        private Color startBGColor;
        private Color endBGColor;
        private Color startFGColor;
        private Color endFGColor;
        private bool mouseHasLeft;

        public MiniTimer()
        { 
            InitializeComponent();
            SetupWindowPosition();
            ReadSettings();
        }

        public MiniTimer(CoreTimer timer) : this()
        {
            this.timer = timer;
            BindToTimerEvents();
        }

        private void BindToTimerEvents()
        {
            timer.TomatoStarted += Timer_TomatoStarted;
            timer.TomatoEnded += Timer_TomatoEnded;
            timer.BreakStarted += Timer_BreakStarted;
            timer.BreakEnded += Timer_BreakEnded;
            timer.SetBreakStarted += Timer_SetBreakStarted;
            timer.SetBreakEnded += Timer_SetBreakEnded;
            timer.Interrupted += Timer_Interrupted;
        }

        #region Timer Event Handlers
        private void Timer_SetBreakStarted(object sender, EventArgs e)
        {
            StartRunning();
        }

        private void StartRunning()
        {
            mouseHasLeft = true;
        }

        private void StartTransition()
        {
            mouseHasLeft = false;
        }

        private void Timer_SetBreakEnded(object sender, EventArgs e)
        {
            StartTransition();
        }

        private void Timer_Interrupted(object sender, EventArgs e)
        {
            StartTransition();
        }

        private void Timer_BreakEnded(object sender, EventArgs e)
        {
            StartTransition();
        }

        private void Timer_BreakStarted(object sender, EventArgs e)
        {
            StartRunning();
        }

        private void Timer_TomatoEnded(object sender, EventArgs e)
        {
            StartTransition();
        }

        private void Timer_TomatoStarted(object sender, EventArgs e)
        {
            StartRunning();
        }
        #endregion

        private void SetupWindowPosition()
        {
            var screenSize = new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.WorkArea.Height);
            Top = 0;
            Left = (screenSize.Width - Width) / 2;
        }

        private void ReadSettings()
        {
            startBGColor = Current.User.StartBGColor;
            endBGColor = Current.User.EndBGColor;
            startFGColor = Current.User.StartFGColor;
            endFGColor = Current.User.EndFGColor;
        }

        /// <summary>
        /// Refresh the UI Elements for the Mini Timer Display.
        /// </summary>
        /// <param name="remaining">About of Time Remaining in the Current Operation (Tomato/Break/Set Break).</param>
        /// <param name="progress">Percent of Operation Complete (0-100)</param>
        public void Update(TimeSpan remaining, int progress)
        {
            if (progress < 0)
                progress = 0;
            if (progress > 100)
                progress = 100;

            // Refresh UI Elements Based on
            var time = remaining.ToFriendly();
            LblTimeRemaining.Content = time;
            Background = new SolidColorBrush(endBGColor.FadeToColor(startBGColor, progress));
            LblTimeRemaining.Foreground = new SolidColorBrush(endFGColor.FadeToColor(startFGColor, progress));
            LblViewBox.UpdateLayout();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var state = WindowState;
                WindowState = state == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseHasLeft) return;
            FadeInInterruptBtn();
        }

        private void FadeInInterruptBtn()
        {
            if (!mouseHasLeft) return;
            BtnInterrupt.FadeIn(FADE_DURATION);
            mouseHasLeft = false;
        }

        private void BtnInterrupt_Click(object sender, RoutedEventArgs e)
        {
            timer.Interrupt();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnInterrupt.FadeOut(FADE_DURATION);
            mouseHasLeft = true;
        }
    }
}
