using System;
using Leonis.TomatoTimer.UI.Settings;
using Rhino.Mocks;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI.Settings
{
    public class SettingsFileTest
    {
        protected SettingsFile<AppSettings> file;
        protected AppSettings settings;
        protected IXmlFileStore store;

        public SettingsFileTest()
        {
            store = MockRepository.GenerateMock<IXmlFileStore>();
            file = new SettingsFile<AppSettings>("TestFile", store);
            settings = new AppSettings();
        }

        protected string FileExtension
        {
            get { return ".settings.xml"; }   
        }
    }

    public class when_settingsfile_inited : SettingsFileTest
    {
        
    }

    public class when_setting_filename : SettingsFileTest
    {
        [Fact]
        public void get_returns_set_filename()
        {
            var exp = "test" + FileExtension;
            file.FileName = exp;
            Assert.Equal(exp, file.FileName);
        }

        [Fact]
        public void extension_is_appended_if_missing()
        {
            file.FileName = "test";
            Assert.Equal("test" + FileExtension, file.FileName);
        }

        [Fact]
        public void extension_is_not_appended_if_passed()
        {
            file.FileName = "test" + FileExtension;
            Assert.Equal("test" + FileExtension, file.FileName);
        }

        [Fact]
        public void filename_empty_throws_argex()
        {
            Assert.Throws<ArgumentException>(
                () => file.FileName = string.Empty);
        }

        [Fact]
        public void filename_null_throws_argex()
        {
            Assert.Throws<ArgumentException>(
                () => file.FileName = null);
        }

        [Fact]
        public void can_pass_filename_via_ctor()
        {
            file = new SettingsFile<AppSettings>("testfile", store);
            Assert.Equal("testfile" + FileExtension, file.FileName);
        }

        [Fact]
        public void passing_empty_filename_to_ctor_throws_argex()
        {
            Assert.Throws<ArgumentException>(
                () => file = new SettingsFile<AppSettings>(string.Empty, store));
        }

        [Fact]
        public void passing_null_filename_to_ctor_throws_argex()
        {
            Assert.Throws<ArgumentException>(
                () => file = new SettingsFile<AppSettings>(null, store));
        }

        [Fact]
        public void filestore_checks_for_existance_of_file()
        {
            file.FileName = "test";
            store.AssertWasCalled(x => x.Exists("test" + FileExtension));
        }

        [Fact]
        public void raises_filenotfound_when_no_file_exists()
        {
            // Arrange
            store.Expect(x => x.Exists("test" + FileExtension)).Return(false);
            bool raised = false;
            file.FileNotFound += ((sender, e) => { raised = true; });
            // Act
            file.FileName = "test";
            // Assert
            Assert.True(raised);
        }
    }

    public class when_filepath_set : SettingsFileTest
    {
        [Fact]
        public void filepath_ends_in_filename()
        {
            // Arrange
            // Nothing to Do Here

            // Act
            file.FileName = "test";

            // Assert
            var path = file.FilePath;
            Assert.True(path.EndsWith("test" + FileExtension));
        }

        [Fact]
        public void filepath_starts_with_assembly_directory()
        {
            // Arrange
            var dir = Utility.AssemblyPath;

            // Act
            // Nothing to Do Here

            // Assert
            var path = file.FilePath;
            Assert.True(path.StartsWith(dir));
        }
    }

    public class when_saving_file : SettingsFileTest
    {
        /* Xml Serialization is Tested in AppSettings Tests */

        [Fact]
        public void store_is_passed_xml_to_save()
        {
            // Arrange
            store.Expect(x => x.SaveXml(string.Empty)).IgnoreArguments();

            // Act
            file.Save(settings);

            // Assert
            store.VerifyAllExpectations();
        }

        [Fact]
        public void if_settings_null_file_is_not_saved()
        {
            // Arrange
            AppSettings nullSettings = null;

            // Act
            file.Save(nullSettings);

            // Assert
            store.AssertWasNotCalled(x => x.SaveXml(Arg<String>.Is.Anything));
        }

        [Fact]
        public void if_xmlfilestore_throws_error_errorsavingfile_is_raised()
        {
            // Arrange
            store.Expect(x => x.SaveXml(Arg<string>.Is.Anything)).Throw(new Exception());
            bool raised = false;
            file.ErrorSavingFile += (sender, args) => raised = true;

            // Act
            file.Save(settings);

            // Assert
            Assert.True(raised);
        }
    }

    public class when_loading_file : SettingsFileTest
    {
        [Fact]
        public void when_store_throws_ex_raises_errorloadingfile()
        {
            // Arrange
            store.Expect(x => x.LoadXml(Arg<string>.Is.Anything)).Throw(new Exception());
            bool raised = false;
            file.ErrorLoadingFile += (sender, args) => raised = true;

            // Act
            file.Load();

            // Assert
            Assert.True(raised);
        }

        [Fact]
        public void when_store_throws_ex_returns_default_appsettings()
        {
            // Arrange
            var @default = new AppSettings();
            store.Expect(x => x.LoadXml(Arg<string>.Is.Anything)).Throw(new Exception());

            // Act
            var loaded = file.Load();

            // Assert
            Assert.Equal(@default.AppTitle, loaded.AppTitle);
            Assert.Equal(@default.ContactEmail, loaded.ContactEmail);
        }

        [Fact]
        public void xml_is_loaded_from_store()
        {
            // Arrange
            store.Expect(x => x.LoadXml(Arg<string>.Is.Anything)).Return("<appSettings />");

            // Act
            file.Load();

            // Assert
            store.VerifyAllExpectations();
        }
    }
}