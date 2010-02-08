using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Leonis.TomatoTimer.UI.Settings;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Settings
{
    public class AppSettingsTest
    {
        protected AppSettings settings;

        public AppSettingsTest()
        {
            settings = new AppSettings();
        }
    }

    public class when_appsettings_inited : AppSettingsTest
    {
        [Fact]
        public void apptitle_returns_tomatotimer()
        {
            Assert.Equal("Tomato Timer", settings.AppTitle);
        }

        [Fact]
        public void can_change_apptitle()
        {
            Assert.NotEqual("test", settings.AppTitle);
            settings.AppTitle = "test";
            Assert.Equal("test", settings.AppTitle);
        }

        [Fact]
        public void cannot_set_apptitle_to_empty()
        {
            Assert.Throws<ArgumentException>(
                () => settings.AppTitle = string.Empty);
        }

        [Fact]
        public void cannot_set_apptitle_to_null()
        {
            Assert.Throws<ArgumentException>(
                () => settings.AppTitle = null);
        }

        [Fact]
        public void can_change_contactemail()
        {
            Assert.NotEqual("test@domain.com", settings.ContactEmail);
            settings.ContactEmail = "test@domain.com";
            Assert.Equal("test@domain.com", settings.ContactEmail);
        }

        [Fact]
        public void contactemail_returns_robcthegeekpublic()
        {
            Assert.Equal("robcthegeek.public@gmail.com", settings.ContactEmail);
        }

        [Fact]
        public void cannot_set_contactemail_to_empty()
        {
            Assert.Throws<ArgumentException>(
                () => settings.ContactEmail = string.Empty);
        }

        [Fact]
        public void cannot_set_contactemail_to_null()
        {
            Assert.Throws<ArgumentException>(
                () => settings.ContactEmail = null);
        }

        [Fact]
        public void settings_implements_ixmlserializable()
        {
            var xs = settings as IXmlSerializable;
            Assert.NotNull(xs);
        }
    }

    public class when_serialising_appsettings_to_xml : AppSettingsTest
    {
        private string GetXml()
        {
            var xs = new XmlSerializer(typeof (AppSettings));
            var writer = new StringWriter();
            xs.Serialize(writer, settings);
            return writer.ToString();
        }

        [Fact]
        public void xml_contains_root_appsettings()
        {
            var xml = GetXml();
            Assert.Contains("<appSettings", xml); /* No Closing '>' Since Root Has Version Attrib */
            Assert.Contains("</appSettings>", xml);
        }

        [Fact]
        public void xml_contains_apptitle()
        {
            settings.AppTitle = "XML Title";
            var xml = GetXml();
            Assert.Contains("<appTitle>XML Title</appTitle>", xml);
        }

        [Fact]
        public void xml_contains_contactemail()
        {
            settings.ContactEmail = "a@b.com";
            var xml = GetXml();
            Assert.Contains("<contactEmail>a@b.com</contactEmail>", xml);
        }
    }

    public abstract class AppSettingsXmlParsingTest : AppSettingsTest
    {
        protected readonly IXmlFileStore store;

        protected AppSettingsXmlParsingTest()
        {
            store = MockRepository.GenerateMock<IXmlFileStore>();
            RefreshSettings();
        }

        protected void RefreshSettings()
        {
            store.Expect(x => x.LoadXml(Arg<string>.Is.Anything)).Return(Xml());
            var file = new SettingsFile<AppSettings>(store);
            settings = file.Load();
        }

        // Xml to Test Against.
        public abstract string Xml();
    }

    public class when_deserialising_good_xml : AppSettingsXmlParsingTest
    {
        public override string Xml()
        {
            return TestResources.CustomAppSettings;
        }

        [Fact]
        public void apptitle_is_test_appsettings()
        {
            Assert.Equal("Test AppSettings", settings.AppTitle);
        }

        [Fact]
        public void contactemail_is_test_at_appsettings_dot_com()
        {
            Assert.Equal("test@appsettings.com", settings.ContactEmail);
        }
    }

    public class when_deserialising_bad_xml : AppSettingsXmlParsingTest
    {
        private string replaceNode = string.Empty;
        private string replaceValue = string.Empty;

        void ReplaceNodeValue(string node, string value)
        {
            replaceNode = node;
            replaceValue = value;
            RefreshSettings();
        }

        public override string Xml()
        {
            var good = TestResources.CustomAppSettings;
            var searchPattern = string.Format(@"<{0}>(.*)</{0}>", replaceNode);
            var search = new Regex(searchPattern);
            var bad = search.Replace(good, string.Format(@"<{0}>{1}</{0}>", replaceNode, replaceValue));
            return bad;
        }

        [Fact]
        public void apptitle_returns_tomato_timer()
        {
            ReplaceNodeValue("appTitle", string.Empty);
            Assert.Equal("Tomato Timer", settings.AppTitle);
        }

        [Fact]
        public void contactemail_is_robcthegeek_dot_public_at_gmail_dot_com()
        {
            ReplaceNodeValue("contactEmail", string.Empty);
            Assert.Equal("robcthegeek.public@gmail.com", settings.ContactEmail);
        }
    }
}
