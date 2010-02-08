using System;
using System.Windows.Media;
using System.Xml;
using TomatoTimer.UI.Graphics;

namespace TomatoTimer.UI.Settings
{
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Reads the Current Element Content as an String.<para />
        /// If there are any problems, then the 'default' value is returned.
        /// </summary>
        /// <param name="reader">XmlReader Being Traversed.</param>
        /// <param name="default">Value to Return on any Read Errors.</param>
        /// <returns>Element Content if Successful, 'default' Value Specified if Otherwise.</returns>
        public static string GetStringElementContentOrDefault(this XmlReader reader, string @default)
        {
            if (reader.NodeType != XmlNodeType.Element)
                return @default;
            try
            {
                return reader.ReadElementContentAsString();
            }
            catch (Exception)
            {
                reader.Read();      /* Skip the Bad Element. */
                return @default;
            }
        }

        /// <summary>
        /// Reads the Current Element Content as an Int.<para />
        /// If there are any problems, then the 'default' value is returned.
        /// </summary>
        /// <param name="reader">XmlReader Being Traversed.</param>
        /// <param name="default">Value to Return on any Read Errors.</param>
        /// <returns>Element Content if Successful, 'default' Value Specified if Otherwise.</returns>
        public static int GetIntElementContent(this XmlReader reader, int @default)
        {
            if (reader.NodeType != XmlNodeType.Element)
                return @default;

            try
            {
                return reader.ReadElementContentAsInt();
            }
            catch (Exception)
            {
                reader.Read();      /* Skip the Bad Element. */
                return @default;
            }
        }

        /// <summary>
        /// Reads the Current Element Content as a Media.Color.<para/>
        /// If there are any problems, then the 'default' value is returned.
        /// </summary>
        /// <param name="reader">XmlReader Being Traversed.</param>
        /// <param name="default">Value to Return on any Read Errors.</param>
        /// <returns>Element Content if Successful, 'default' Value Specified if Otherwise.</returns>
        public static Color GetColorElementContent(this XmlReader reader, Color @default)
        {
            if (reader.NodeType != XmlNodeType.Element)
                return @default;

            try
            {
                var content = reader.ReadElementContentAsString();
                var parsedColor = ColorExtensions.FromString(content);
                return parsedColor.IsEmpty() ? @default : parsedColor;
            }
            catch (Exception)
            {
                reader.Read();      /* Skip the Bad Element. */
                return @default;
            }
        }
    }
}
