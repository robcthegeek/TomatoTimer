using System;

namespace Leonis.TomatoTimer.UI.Settings
{
    public class SettingsFileEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public string Description { get; set; }

        public SettingsFileEventArgs(string fileName, string description)
        {
            FileName = fileName;
            Description = description;
        }
    }
}
