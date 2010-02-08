using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Leonis.TomatoTimer.Tests.UI.Settings
{
    public class DefaultUserSettingsXml
    {
        public string TomatoTime { get; set; }
        public string BreakTime { get; set; }
        public string SetBreakTime { get; set; }
        public bool IncludeComments { get; set; }
        public string StartBGColor { get; set; }
        public string EndBGColor { get; set; }
        public string StartFGColor { get; set; }
        public object EndFGColor { get; set; }

        public DefaultUserSettingsXml()
        {
            TomatoTime = "25";
            BreakTime = "5";
            SetBreakTime = "30";
            IncludeComments = true;
            StartBGColor = "Green";
            EndBGColor = "Red";
            StartFGColor = "Black";
            EndFGColor = "Black";
        }

        public string Xml
        {
            get { return this.ToString(); }   
        }

        public override string  ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\" ?>");
            sb.AppendLine("\t<userSettings version=\"0.1\">");

            if (IncludeComments)
            {
                sb.AppendLine("\t<!-- User Settings File for Tomato Timer -->");
                sb.AppendLine(
                    "\t<!-- This File Can be Edited to Customise the Application Based on User Preferences -->");
                sb.AppendLine("\t<!-- 'tomatoTime' - Number of Minutes For a Tomato -->");
            }
            sb.AppendFormat("\t<tomatoTime>{0}</tomatoTime>", TomatoTime);
            if (IncludeComments)
                sb.AppendLine("\t<!-- 'breakTime' - Number of Minutes For a (Short) Break -->");
            sb.AppendFormat("\t<breakTime>{0}</breakTime>", BreakTime);
            if (IncludeComments)
                sb.AppendLine("\t<!-- 'setBreakTime' - Number of Minutes For a (Long) Set Break -->");
            sb.AppendFormat("\t<setBreakTime>{0}</setBreakTime>", SetBreakTime);

            if (IncludeComments)
                sb.AppendLine("\t<!-- 'startBGColor' - Color MiniTimer Background *Starts* With (i.e. With All Time Remaining) -->");
            sb.AppendFormat("\t<startBGColor>{0}</startBGColor>", StartBGColor);

            if (IncludeComments)
                sb.AppendLine("\t<!-- 'endBGColor' - Color MiniTimer Background *Ends* With (i.e. With NO Time Remaining) -->");
            sb.AppendFormat("\t<endBGColor>{0}</endBGColor>", EndBGColor);

            if (IncludeComments)
                sb.AppendLine("\t<!-- 'startFGColor' - Color MiniTimer Foreground (Text) *Starts* With (i.e. With All Time Remaining) -->");
            sb.AppendFormat("\t<startFGColor>{0}</startFGColor>", StartFGColor);

            if (IncludeComments)
                sb.AppendLine("\t<!-- 'endFGColor' - Color MiniTimer Foreground (Text) *Ends* With (i.e. With NO Time Remaining) -->");
            sb.AppendFormat("\t<endFGColor>{0}</endFGColor>", EndFGColor);

            sb.AppendLine("</userSettings>");
            
            return sb.ToString();
        }
    }
}
