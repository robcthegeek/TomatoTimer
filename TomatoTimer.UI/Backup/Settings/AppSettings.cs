using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Leonis.TomatoTimer.UI.Settings
{
    [XmlRoot("appSettings")]
    public class AppSettings : IXmlSerializable
    {
        #region Properties

        private string appTitle;

        /// <summary>
        /// Application Display Title
        /// </summary>
        public string AppTitle
        {
            get { return appTitle; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(
                        "Cannot Set Application Title Setting to Empty/Null.");

                appTitle = value;
            }
        }

        private string contactEmail;

        /// <summary>
        /// Primary Contact Address for Feedback etc.
        /// </summary>
        public string ContactEmail
        {
            get { return contactEmail; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(
                        "Cannot Set Contact Email to Empty/Null");
                contactEmail = value;
            }
        }

        #endregion

        public AppSettings()
        {
            AppTitle = "Tomato Timer";
            ContactEmail = "robcthegeek.public@gmail.com";
        }

        #region IXmlSerializable Implementation
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "appTitle":
                            AppTitle = reader.GetStringElementContentOrDefault(AppTitle);
                            break;
                        case "contactEmail":
                            ContactEmail = reader.GetStringElementContentOrDefault(ContactEmail);
                            break;
                        default:
                            reader.Read();
                            break;
                    }
                }
                else
                    reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("version", "0.1 Beta");
            writer.WriteElementString("appTitle", AppTitle);
            writer.WriteElementString("contactEmail", ContactEmail);
        } 
        #endregion
    }
}
