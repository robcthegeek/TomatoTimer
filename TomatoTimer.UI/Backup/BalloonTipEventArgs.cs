using System;

namespace Leonis.TomatoTimer.UI
{
    /// <summary>
    /// EventArgs Used by NotifyIconComponent.BalloonTip* Events
    /// </summary>
    public class BalloonTipEventArgs : EventArgs
    {
        public string BalloonTitle { get; set; }
        public string BallonText { get; set; }

        /// <summary>
        /// Inits the BalloonTipEventArgs
        /// </summary>
        /// <param name="title">Title of the Balloon Raising the Event.</param>
        /// <param name="text">Text Displayed in Balloon When Raising the Event.</param>
        public BalloonTipEventArgs(string title, string text)
        {
            BalloonTitle = title;
            BallonText = text;
        }
    }
}
