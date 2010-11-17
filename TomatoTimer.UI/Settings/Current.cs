namespace TomatoTimer.UI.Settings
{
    public static class Current
    {
        private static readonly UserSettings userSettings = LoadUserSettings();

        private static UserSettings LoadUserSettings()
        {
            var store = new XmlFileStore();
            var file = new SettingsFile<UserSettings>("User", store);
            return file.Load();
        }

        public static UserSettings User
        {
            get
            {
                return userSettings;
            }
        }
    }
}