using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Leonis.TomatoTimer.UI.Graphics;

namespace Leonis.TomatoTimer.UI.Settings
{
    [XmlRoot("userSettings")]
    public class UserSettings : IXmlSerializable
    {
        /// <summary>
        /// Time (in Minutes) for a Tomato.
        /// </summary>
        public int TomatoTime { get; set; }

        /// <summary>
        /// Time (in Minutes) for a Break;
        /// </summary>
        public int BreakTime { get; set; }

        /// <summary>
        /// Time (in Minutes) for a Set Break.
        /// </summary>
        public int SetBreakTime { get; set; }

        /// <summary>
        /// Color MiniTimer Background *Starts* With (i.e. With All Time Remaining)
        /// </summary>
        public Color StartBGColor { get; set; }

        /// <summary>
        /// Color MiniTimer Background *Starts* With (i.e. With NO Time Remaining)
        /// </summary>
        public Color EndBGColor { get; set; }

        /// <summary>
        /// Color MiniTimer Foreground *Starts* With (i.e. With All Time Remaining)
        /// </summary>
        public Color StartFGColor { get; set; }

        /// <summary>
        /// Color MiniTimer Foreground *Ends* With (i.e. With NO Time Remaining)
        /// </summary>
        public Color EndFGColor { get; set; }

        public UserSettings()
        {
            TomatoTime = 25;
            BreakTime = 5;
            SetBreakTime = 30;
            StartBGColor = Colors.Green;
            EndBGColor = Colors.Red;
            StartFGColor = Colors.Black;
            EndFGColor = Colors.Black;
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "tomatoTime":
                            TomatoTime = reader.GetIntElementContent(TomatoTime);
                            break;
                        case "breakTime":
                            BreakTime = reader.GetIntElementContent(BreakTime);
                            break;
                        case "setBreakTime":
                            SetBreakTime = reader.GetIntElementContent(SetBreakTime);
                            break;
                        case "startBGColor":
                            StartBGColor = reader.GetColorElementContent(StartBGColor);
                            break;
                        case "endBGColor":
                            EndBGColor = reader.GetColorElementContent(EndBGColor);
                            break;
                        case "startFGColor" :
                            StartFGColor = reader.GetColorElementContent(StartFGColor);
                            break;
                        case "endFGColor":
                            EndFGColor = reader.GetColorElementContent(EndFGColor);
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
            writer.WriteAttributeString("version", "0.1");
            writer.WriteComment("User Settings File for Tomato Timer");
            writer.WriteComment("This File Can be Edited to Customise the Application Based on User Preferences");
            
            writer.WriteComment("'tomatoTime' - Number of Minutes For a Tomato");
            writer.WriteElementString("tomatoTime", TomatoTime.ToString());
            writer.WriteComment("'breakTime' - Number of Minutes For a (Short) Break");
            writer.WriteElementString("breakTime", BreakTime.ToString());
            writer.WriteComment("'setBreakTime' - Number of Minutes For a (Long) Set Break");
            writer.WriteElementString("setBreakTime", SetBreakTime.ToString());
            writer.WriteComment("'startBGColor' - Color MiniTimer Background *Starts* With (i.e. With All Time Remaining)");
            writer.WriteElementString("startBGColor", StartBGColor.ToKnownOrHex());
            writer.WriteComment("'startFGColor' - Color MiniTimer Foreground (Text) *Starts* With (i.e. With All Time Remaining)");
            writer.WriteElementString("startFGColor", StartBGColor.ToKnownOrHex());
        }
    }
}
