using System.Windows.Media;

namespace TomatoTimer.UI.Graphics
{
    public static class BrushExtensions
    {
        public static Brush BasedOnProgress(int progress)
        {
            if (progress > 74)
                return Brushes.Green;
            if (progress > 49)
                return Brushes.GreenYellow;
            if (progress > 24)
                return Brushes.Yellow;
            if (progress > 14)
                return Brushes.Orange;
            if (progress > 4)
                return Brushes.OrangeRed;
            return Brushes.Red;
        }
    }
}