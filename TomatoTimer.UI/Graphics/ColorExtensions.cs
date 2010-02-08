using System;
using System.Collections.Generic;
using KnownColor = System.Drawing.KnownColor;
using Color = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;

namespace TomatoTimer.UI.Graphics
{
    public static class ColorExtensions
    {
        public static Color FadeToColor(this Color baseColor, Color color, int percent)
        {
            var diffR = ProgressByteDiff(baseColor.R, color.R, percent);
            var diffG = ProgressByteDiff(baseColor.G, color.G, percent);
            var diffB = ProgressByteDiff(baseColor.B, color.B, percent);
            var diffCol = Color.FromRgb((byte)(diffR), (byte)(diffG), (byte)(diffB));
            return diffCol;
        }

        static double ProgressByteDiff(byte b1, byte b2, int progress)
        {
            // First We Need to Get the Diff of the Two
            var negativeDiff = b2 < b1;
            var diff = b1 - b2;
            var signedDiff = negativeDiff ? 0 - diff : diff;
            double percent = (double)signedDiff / 100;
            double complete = percent * progress;
            double result = Math.Floor(Math.Round(b1 + complete,2));
            return result < 0 ? (0 - result) : result;
        }


        /// <summary>
        /// Attempts to Parse a Color from a given String.<para/>
        /// This Can Either be a "Known" Color or a A/RGB Hex Value.
        /// </summary>
        /// <param name="color">Colour to Parse.</param>
        /// <returns>ARGB #00000000 On Any Error.</returns>
        public static Color FromString(string color)
        {
            if (string.IsNullOrEmpty(color))
                return Empty();

            // See if the String is a KnownColor
            var parse = GetKnownColour(color);
            
            // Try to Parse Hex
            if (IsEmpty(parse))
                parse = GetHexColor(color);

            return Color.FromArgb(parse.A, parse.R, parse.G, parse.B);
        }

        /// <summary>
        /// Parses a Colour from the given string if it matches an item in the System.Color.KnownColor enumeration.
        /// </summary>
        /// <param name="color">String to parse.</param>
        /// <returns>Null if No Colour Found.</returns>
        /// <see cref="http://msdn.microsoft.com/en-us/library/system.drawing.knowncolor.aspx"/>
        static DrawingColor GetKnownColour(string color)
        {
            var rtn = System.Drawing.Color.FromName(color);
            return rtn;
        }

        /// <summary>
        /// Returns True if the Drawing.Color Passed is "Empty" (ARGB: #00000000).
        /// </summary>
        /// <param name="color">Colour to Check.</param>
        public static bool IsEmpty(this DrawingColor color)
        {
            return (color.A == 0 && color.R == 0 && color.B == 0 && color.G == 0);
        }

        /// <summary>
        /// Returns True if the Media.Color Passed is "Empty" (ARGB: #00000000).
        /// </summary>
        /// <param name="color">Colour to Check.</param>
        public static bool IsEmpty(this Color color)
        {
            return IsEmpty(color.ToDrawingColor());
        }

        /// <summary>
        /// An "Empty" Media.Color (ARGB #00000000).
        /// </summary>
        /// <returns></returns>
        public static Color Empty()
        {
            return Color.FromArgb(0, 0, 0, 0);
            
        }

        /// <summary>
        /// Returns a Drawing.Color Object Based on Hex/HTML-Style Input (FFFFFF).
        /// </summary>
        /// <param name="color">String to Parse.</param>
        /// <returns>ARGB Color #00000000 on Any Failure to Parse.</returns>
        static System.Drawing.Color GetHexColor(string color)
        {
            if (!color.StartsWith("#"))
                color = color.Insert(0, "#");

            try
            {
                var rtn = System.Drawing.ColorTranslator.FromHtml(color);
                return rtn;
            }
            catch (Exception)
            {
                return Empty().ToDrawingColor();
            }
        }

        /// <summary>
        /// Converts a Color to Either "KnownColor" or Hex String (e.g. 'Red' or '#FFFF000000').
        /// </summary>
        /// <remarks>If Color Alpha is 255, then it is ommitted from the Hex string.</remarks>
        public static string ToKnownOrHex(this Color color)
        {
            var known = GetKnownColor(color);
            var hex = (color.A != 255) ? color.ToString() : System.Drawing.ColorTranslator.ToHtml(color.ToDrawingColor());
            return (string.IsNullOrEmpty(known)) ? hex : known;
        }

        /// <summary>
        /// Gets the Non-System KnownColor for the Given Color By Comparing to the KnownColor Enum Values.
        /// </summary>
        /// <returns>Null if Not a KnownColor</returns>
        private static string GetKnownColor(Color color)
        {
            var ignored = SystemKnownColors();
            foreach (var knownCol in Enum.GetValues(typeof(KnownColor)))
            {
                // Don't Check Against System KnownColors.
                if (ignored.Contains((KnownColor)knownCol))
                    continue;

                var compare = DrawingColor.FromKnownColor((KnownColor)knownCol);
                if (ColorsEqual(color, compare))
                    return Enum.GetName(typeof (KnownColor), knownCol);
            }
            return null;
        }

        /// <summary>
        /// Returns a List of all of the System "KnownColor's" (e.g. "Control", "Desktop").
        /// </summary>
        public static List<KnownColor> SystemKnownColors()
        {
            return new List<KnownColor>
                       {
                           KnownColor.ActiveBorder,
                           KnownColor.ActiveCaption,
                           KnownColor.ActiveCaptionText,
                           KnownColor.AppWorkspace,
                           KnownColor.ButtonFace,
                           KnownColor.ButtonHighlight,
                           KnownColor.ButtonShadow,
                           KnownColor.Control,
                           KnownColor.ControlDark,
                           KnownColor.ControlDarkDark,
                           KnownColor.ControlLight,
                           KnownColor.ControlLightLight,
                           KnownColor.ControlText,
                           KnownColor.Desktop,
                           KnownColor.GradientActiveCaption,
                           KnownColor.GradientInactiveCaption,
                           KnownColor.Highlight,
                           KnownColor.HighlightText,
                           KnownColor.InactiveBorder,
                           KnownColor.InactiveCaption,
                           KnownColor.InactiveCaptionText,
                           KnownColor.Info,
                           KnownColor.InfoText,
                           KnownColor.Menu,
                           KnownColor.MenuBar,
                           KnownColor.MenuHighlight,
                           KnownColor.MenuText,
                           KnownColor.ScrollBar,
                           KnownColor.Window,
                           KnownColor.WindowFrame,
                           KnownColor.WindowText
                       };
        }

        /// <summary>
        /// Compares a Media.Color to Drawing.Color. Returns T/F Dependent on Equality.
        /// </summary>
        static bool ColorsEqual(Color mediaColor, DrawingColor drawingColor)
        {
            return (
                (mediaColor.A == drawingColor.A) &&
                (mediaColor.R == drawingColor.R) &&
                (mediaColor.G == drawingColor.G) &&
                (mediaColor.B == drawingColor.B));
        }

        /// <summary>
        /// Converts a Media.Color to a Drawing.Color.
        /// </summary>
        static DrawingColor ToDrawingColor(this Color color)
        {
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}