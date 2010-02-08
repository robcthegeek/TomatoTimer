using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TomatoTimer.UI.Graphics
{
    public static class ControlExtensions
    {
        public static void FadeOut(this FrameworkElement element, int duration)
        {
            FadeControl(element, DurationFromMilliseconds(duration), false);
        }

        public static void FadeIn(this FrameworkElement element, int duration)
        {
            FadeControl(element, DurationFromMilliseconds(duration), true);
        }

        private static void FadeControl(this FrameworkElement element, Duration duration, bool fadeIn)
        {
            var storyboard = new Storyboard();
            var animation = fadeIn ? new DoubleAnimation(0, 1, duration) : new DoubleAnimation(1, 0, duration);

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private static Duration DurationFromMilliseconds(int ms)
        {
            return new Duration(new TimeSpan(0, 0, 0, 0, ms));
        }
    }
}
