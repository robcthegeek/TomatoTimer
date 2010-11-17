using System;
using System.IO;

namespace TomatoTimer.UI.Settings
{
    public class XmlFileStore : IXmlFileStore
    {
        public bool Exists(string fileName)
        {
            // BUG: If an Exception is thrown here, it causes the static settings class to epic fail, catch and resolve.
            return File.Exists(fileName);
        }

        public void SaveXml(string xml)
        {
            throw new NotImplementedException();
        }

        public string LoadXml(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}