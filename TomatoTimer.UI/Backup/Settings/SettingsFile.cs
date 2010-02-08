using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Leonis.TomatoTimer.Core;

namespace Leonis.TomatoTimer.UI.Settings
{
    public class SettingsFile<SettingsType> where SettingsType : class, IXmlSerializable, new()
    {
        /// <summary>
        /// File Extension for Settings Files.
        /// </summary>
        private const string FILE_EXT = ".settings.xml";

        #region Events
        /// <summary>
        /// Raised When The File Specified for FileName Cannot Be Found.
        /// </summary>
        public event EventHandler<SettingsFileEventArgs> FileNotFound;
        protected void OnFileNotFound(SettingsFileEventArgs e)
        {
            if (FileNotFound != null)
            {
                FileNotFound(this, e);
            }
        }

        /// <summary>
        /// Raised When the XmlFileStore Has Problems Saving the File.
        /// </summary>
        public event EventHandler<SettingsFileEventArgs> ErrorSavingFile;
        protected void OnErrorSavingFile(SettingsFileEventArgs e)
        {
            if (ErrorSavingFile != null)
            {
                ErrorSavingFile(this, e);
            }
        }

        /// <summary>
        /// Raised When the XmlFileStore Has Problems Loading the File.
        /// </summary>
        public event EventHandler<SettingsFileEventArgs> ErrorLoadingFile;
        protected void OnErrorLoadingFile(SettingsFileEventArgs e)
        {
            if (ErrorLoadingFile != null)
            {
                ErrorLoadingFile(this, e);
            }
        }
        #endregion

        private readonly IXmlFileStore xmlFileStore;
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(
                        "Cannot Set FileName to Null/Empty String");
                if (!value.EndsWith(FILE_EXT))
                    value += FILE_EXT;

                CheckFileExists(value);
                fileName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return GenerateFilePath();
            }
        }

        /// <summary>
        /// Checks with the FileStore System to See if the FileName Passed Exists.<para />
        /// If No File Exists, Then the FileNotFound Event is Raised.
        /// </summary>
        /// <param name="value">Path of the File to Check For Existance.</param>
        private void CheckFileExists(string value)
        {
            var exists = xmlFileStore.Exists(value);
            if (!exists)
                OnFileNotFound(
                    new SettingsFileEventArgs(
                        value, string.Format("Settings File '{0}' Not Found", value)));
        }

        /// <summary>
        /// Represents a XML-Based Settings File That Can Be Saved/Loaded to a File Store.
        /// </summary>
        /// <param name="xmlFileStore">Interface to the File Store.</param>
        public SettingsFile(IXmlFileStore xmlFileStore)
        {
            this.xmlFileStore = xmlFileStore;
        }

        /// <summary>
        /// Represents a XML-Based Settings File That Can Be Saved/Loaded to a File Store.
        /// </summary>
        /// <param name="fileName">The Name of the Settings File.</param>
        /// <param name="xmlFileStore">Interface to the File Store.</param>
        public SettingsFile(string fileName, IXmlFileStore xmlFileStore) : this(xmlFileStore)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Generates the Full File Path by Prepending the FileName with the Assembly Directory.
        /// </summary>
        /// <returns>String in the Format of {AssemblyDirectory}{fileName}</returns>
        private string GenerateFilePath()
        {
            string dir = Assembly.GetExecutingAssembly().Directory();
            return string.Format("{0}{1}", dir, fileName);
        }

        /// <summary>
        /// Saves the Given Settings File to Storage.
        /// </summary>
        /// <param name="settings">XML-Based Settings to Save.</param>
        public void Save(SettingsType settings)
        {
            if (settings == null)
                return;

            // Serialise the Data to XML.
            var type = typeof (SettingsType);
            var xs = new XmlSerializer(type);
            var writer = new StringWriter();
            xs.Serialize(writer, settings);
            var xml = writer.ToString();
            SaveXmlToFile(xml);
        }

        private void SaveXmlToFile(string xml)
        {
            try
            {
                xmlFileStore.SaveXml(xml);
            }
            catch (Exception ex)
            {
                OnErrorSavingFile(new SettingsFileEventArgs(FilePath,
                    string.Format(
                        "There was a Problem Saving Settings File '{0}':\r\n{1}", FilePath, ex.Message)));
            }
        }

        /// <summary>
        /// Loads the Settings From Storage.
        /// </summary>
        /// <returns>Returns the SettingsType Representing Them</returns>
        public SettingsType Load()
        {
            // Get the XML from the Store
            var xml = GetXmlFromStore();

            return SettingsFromXml(xml);
        }

        private SettingsType SettingsFromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return new SettingsType();

            // Deserialise
            try
            {
                var xs = new XmlSerializer(typeof(SettingsType));
                var reader = new StringReader(xml);
                var o = xs.Deserialize(reader);
                return o as SettingsType;
            }
            catch (Exception)
            {
                return new SettingsType();
            }
        }

        string GetXmlFromStore()
        {
            try
            {
                return xmlFileStore.LoadXml(FilePath);
            }
            catch (Exception ex)
            {
                OnErrorLoadingFile(new SettingsFileEventArgs(
                    FilePath, "There Was a Problem Loading the XML From the File Store.\r\n" + ex.Message));
                return string.Empty;
            }
        }
    }
}
