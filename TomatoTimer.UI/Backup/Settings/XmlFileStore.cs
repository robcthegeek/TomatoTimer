using System;
using System.IO;

namespace Leonis.TomatoTimer.UI.Settings
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
            // TODO: Review to See How Exceptions Cause Things to Go Boom Here.
            throw new NotImplementedException();
        }

        public string LoadXml(string filePath)
        {
            // TODO: Review to See How Exceptions Cause Things to Go Boom Here.
            return File.ReadAllText(filePath);
        }
    }
}