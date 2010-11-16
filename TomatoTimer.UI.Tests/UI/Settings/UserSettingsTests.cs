using System.Xml.Serialization;
using TomatoTimer.UI.Settings;
using Xunit;
using Moq;

namespace TomatoTimer.Tests.Unit.UI.Settings
{
    public class UserSettingsTest
    {
        protected UserSettings settings;
        protected SettingsFile<UserSettings> file;
        protected Mock<IXmlFileStore> store;

        public UserSettingsTest()
        {
            settings = new UserSettings();
            store = new Mock<IXmlFileStore>();
            file = new SettingsFile<UserSettings>(store.Object);
        }
    }

    public class when_usersettings_inited : UserSettingsTest
    {
        [Fact]
        public void tomatotime_defaults_to_25min()
        {
            var tomato = TestConstants.TomatoTimeSpan.Minutes;
            Assert.Equal(tomato, settings.TomatoTime);
        }

        [Fact]
        public void breaktime_defaults_to_5min()
        {
            var @break = TestConstants.BreakTimeSpan.Minutes;
            Assert.Equal(@break, settings.BreakTime);
        }

        [Fact]
        public void setbreaktime_defaults_to_30min()
        {
            var setbreak = TestConstants.SetBreakTimeSpan.Minutes;
            Assert.Equal(setbreak, settings.SetBreakTime);
        }

        [Fact]
        public void usersettings_implements_ixmlserializable()
        {
            var xmls = settings as IXmlSerializable;
            Assert.NotNull(xmls);
        }
    }
}