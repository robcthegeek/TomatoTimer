namespace TomatoTimer.UI.Settings
{
    public static class Current
    {
        private static readonly AppSettings appSettings = LoadAppSettings();

        private static AppSettings LoadAppSettings()
        {
            var store = new XmlFileStore();
            var file = new SettingsFile<AppSettings>("App", store);
            return file.Load();
        }

        private static readonly UserSettings userSettings = LoadUserSettings();

        private static UserSettings LoadUserSettings()
        {
            var store = new XmlFileStore();
            var file = new SettingsFile<UserSettings>("User", store);
            return file.Load();
        }

        public static AppSettings Application
        {
            get
            {
                return appSettings;
            }
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