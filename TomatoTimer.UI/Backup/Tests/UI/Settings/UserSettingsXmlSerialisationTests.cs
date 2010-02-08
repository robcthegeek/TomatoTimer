using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Serialization;
using Leonis.TomatoTimer.UI.Settings;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Settings
{
    public class UserSettingsXmlSerialisationTests : UserSettingsTest
    {
        private string GetXml()
        {
            var xs = new XmlSerializer(typeof(UserSettings));
            var writer = new StringWriter();
            xs.Serialize(writer, settings);
            return writer.ToString();
        }

        [Fact]
        public void xml_contains_usersettings_root()
        {
            var xml = GetXml();
            Assert.Contains("<userSettings", xml); /* Does Not Terminate Since May Contain Attribs */
            Assert.Contains("</userSettings>", xml);
        }

        [Fact]
        public void root_node_contains_version_attribute()
        {
            var xml = GetXml();
            var reg = new Regex(@"<userSettings[\W]*version=");
            Assert.True(reg.IsMatch(xml));
        }

        [Fact]
        public void instructions_ouput_for_users()
        {
            var xml = GetXml();
            Assert.Contains("User Settings File for Tomato Timer", xml);
            Assert.Contains("User Preferences", xml);
        }

        [Fact]
        public void tomatotime_desc_is_output()
        {
            var xml = GetXml();
            Assert.Contains("'tomatoTime' - Number of Minutes For a Tomato", xml);
        }

        [Fact]
        public void tomatotime_is_output()
        {
            settings.TomatoTime = 42;
            var xml = GetXml();
            Assert.Contains("<tomatoTime>42</tomatoTime>", xml);
        }

        [Fact]
        public void breaktime_desc_is_output()
        {
            var xml = GetXml();
            Assert.Contains("'breakTime' - Number of Minutes For a (Short) Break", xml);
        }

        [Fact]
        public void breaktime_is_output()
        {
            settings.BreakTime = 42;
            var xml = GetXml();
            Assert.Contains("<breakTime>42</breakTime>", xml);
        }

        [Fact]
        public void setbreaktime_desc_is_output()
        {
            var xml = GetXml();
            Assert.Contains("'setBreakTime' - Number of Minutes For a (Long) Set Break", xml);
        }

        [Fact]
        public void setbreaktime_is_output()
        {
            settings.SetBreakTime = 42;
            var xml = GetXml();
            Assert.Contains("<setBreakTime>42</setBreakTime>", xml);
        }

        [Fact]
        public void StartBGColor_Serialises_As_KnownColor_Name()
        {
            settings.StartBGColor = Colors.Red;
            var xml = GetXml();
            Assert.Contains("<startBGColor>Red</startBGColor>", xml);
        }

        [Fact]
        public void StartBGColor_Serialises_As_Hex_For_UnknownColor()
        {
            settings.StartBGColor = Color.FromRgb(153, 140, 255);
            var xml = GetXml();
            Assert.Contains("<startBGColor>#998CFF</startBGColor>", xml);
        }

        [Fact]
        public void StartFGColor_Serialises_As_KnownColor_Name()
        {
            settings.StartBGColor = Colors.Red;
            var xml = GetXml();
            Assert.Contains("<startFGColor>Red</startFGColor>", xml);
        }

        [Fact]
        public void StartFGColor_Serialises_As_Hex_For_UnknownColor()
        {
            settings.StartBGColor = Color.FromRgb(153, 140, 255);
            var xml = GetXml();
            Assert.Contains("<startFGColor>#998CFF</startFGColor>", xml);
        }
    }
}