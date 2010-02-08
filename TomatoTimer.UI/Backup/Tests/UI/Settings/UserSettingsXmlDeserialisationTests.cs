using System;
using System.Windows.Media;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Settings
{
    public abstract class UserSettingsXmlDeserialisationTests : UserSettingsTest
    {
        private readonly DefaultUserSettingsXml Xml;
        protected abstract void SetupXml(DefaultUserSettingsXml xml);

        protected UserSettingsXmlDeserialisationTests()
        {
            Xml = new DefaultUserSettingsXml();
            SetupXml(Xml);
            var content = Xml.ToString();
            store.Expect(x => x.LoadXml(Arg<string>.Is.Anything)).Return(content);
            settings = file.Load();
        }
    }

    public class When_Loading_From_Custom_Xml : UserSettingsXmlDeserialisationTests
    {
        protected override void SetupXml(DefaultUserSettingsXml xml)
        {
            /* Override the Default Values to Test Assignment : Default (Req'd to Run Tests) */
            xml.TomatoTime = "30";
            xml.BreakTime = "10";
            xml.SetBreakTime = "60";
            xml.StartBGColor = "Yellow";
            xml.EndBGColor = "Purple";
            xml.StartFGColor = "Orange";
            xml.EndFGColor = "Pink";
        }

        // ** (Different Values Are Used to Avoid Collisions with Defaults). **
        [Fact]
        public virtual void TomatoTime_Is_30()
        {
            Assert.Equal(30, settings.TomatoTime);
        }

        [Fact]
        public virtual void BreakTime_Is_10()
        {
            Assert.Equal(10, settings.BreakTime);
        }

        [Fact]
        public virtual void SetBreakTime_Is_60()
        {
            Assert.Equal(60, settings.SetBreakTime);
        }

        [Fact]
        public void StartBGColor_Is_Yellow()
        {
            Assert.Equal(Colors.Yellow, settings.StartBGColor);
        }

        [Fact]
        public void EndBGColor_Is_Purpl()
        {
            Assert.Equal(Colors.Purple, settings.EndBGColor);
        }

        [Fact]
        public void StartFGColor_Is_Orange()
        {
            Assert.Equal(Colors.Orange, settings.StartFGColor);
        }

        [Fact]
        public void EndFGColor_Is_Pink()
        {
            Assert.Equal(Colors.Pink, settings.EndFGColor);
        }
    }

    public class When_Loading_From_Comment_Stripped_Xml : UserSettingsXmlDeserialisationTests
    {
        protected override void SetupXml(DefaultUserSettingsXml xml)
        {
            /* Quick Test to Ensure All Values Are Processed When Comments Removed */
            xml.IncludeComments = false;
        }
    }

    public class When_Loading_Invalid_TomatoTime_Xml : UserSettingsXmlDeserialisationTests
    {
        protected override void SetupXml(DefaultUserSettingsXml xml)
        {
            xml.TomatoTime = "bad";
        }

        [Fact]
        public void TomatoTime_Defaults_To_TomatoTimeSpan()
        {
            // Should be Set to Default on Bad Entry
            Assert.Equal(TestConstants.TomatoTimeSpan.Minutes, settings.TomatoTime);
        }
    }

    public class When_Load_Invalid_Breaktime_Xml : UserSettingsXmlDeserialisationTests
    {
        protected override void  SetupXml(DefaultUserSettingsXml xml)
        {
            xml.BreakTime = "bad";
        }

        [Fact]
        public void BreakTime_Defaults_To_BreakTimeSpan()
        {
            // Break Time Should be Default on Bad Entry
            Assert.Equal(TestConstants.BreakTimeSpan.Minutes, settings.BreakTime);
        }
    }

    public class When_Loading_Invalid_SetBreakTime_Xml : UserSettingsXmlDeserialisationTests
    {
        protected override void  SetupXml(DefaultUserSettingsXml xml)
        {
            xml.SetBreakTime = "bad";
        }

        [Fact]
        public void SetBreakTime_Defaults_To_SetBreakTimeSpan()
        {
            // Set Break Time Should be Default on Bad Entry
            Assert.Equal(TestConstants.SetBreakTimeSpan.Minutes, settings.SetBreakTime);
        }
    }

    public class When_Loading_Invalid_Colors : UserSettingsXmlDeserialisationTests
    {
        protected override void SetupXml(DefaultUserSettingsXml xml)
        {
            /* BEWARE: I originally put 'bad' in here, forgetting that is a valid color code! (BBAADD) :D */
            xml.StartBGColor = "thisisnotavalidcolor";
            xml.StartFGColor = "andanotherbadcolor!";
            xml.EndBGColor = "thiscantbeacolor";
        }

        [Fact]
        public void StartBGColor_Is_Set_To_Green()
        {
            var expected = Colors.Green;
            var actual = settings.StartBGColor;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EndBGColor_Defaults_To_Red()
        {
            var expected = Colors.Red;
            Assert.Equal(expected, settings.EndBGColor);
        }

        [Fact]
        public void StartFGColor_Is_Set_To_Black()
        {
            var expected = Colors.Black;
            var actual = settings.StartFGColor;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EndFGColor_Defaults_To_Black()
        {
            var expected = Colors.Black;
            Assert.Equal(expected, settings.EndFGColor);
        }
    }
}