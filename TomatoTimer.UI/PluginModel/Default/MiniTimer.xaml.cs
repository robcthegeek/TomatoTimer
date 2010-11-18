using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TomatoTimer.Core;
using TomatoTimer.UI.Graphics;

namespace TomatoTimer.UI.PluginModel.Default
{
    /// <summary>
    /// MiniTimer Window Used to Display the Time Remaining to the User.
    /// </summary>
    internal partial class MiniTimer : Window
    {       
        private const int FADE_DURATION = 250;
        private Color startBGColor = Colors.Green;
        private Color endBGColor = Colors.Red;
        private Color startFGColor = Colors.Black;
        private Color endFGColor = Colors.Black;
        private bool mouseHasLeft;

        public MiniTimer()
        { 
            InitializeComponent();
            SetupWindowPosition();
        }

        private void StartRunning()
        {
            Show();
            mouseHasLeft = true;
            LblTimeRemaining.Content = "Starting..";
        }

        public void SetBreakStarted()
        {
            StartRunning();
        }

        private void StartTransition()
        {
            Hide();
            mouseHasLeft = false;
        }

        public void SetBreakEnded()
        {
            StartTransition();
        }

        public void Interrupted()
        {
            StartTransition();
        }

        public void BreakEnded()
        {
            StartTransition();
        }

        public void BreakStarted()
        {
            StartRunning();
        }

        public void TomatoEnded()
        {
            StartTransition();
        }

        public void TomatoStarted()
        {
            StartRunning();
        }

        private void SetupWindowPosition()
        {
            var screenSize = new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.WorkArea.Height);
            Top = 0;
            Left = (screenSize.Width - Width) / 2;
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
            Main.GetInstance().Timer.Interrupt();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnInterrupt.FadeOut(FADE_DURATION);
            mouseHasLeft = true;
        }
    }
}