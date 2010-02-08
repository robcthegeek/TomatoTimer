using System.Windows.Media;
using Leonis.TomatoTimer.UI.Graphics;
using Xunit;

namespace Leonis.TomatoTimer.Tests.UI
{
    public abstract class BrushExtensionsTest
    {
        protected Brush GetBrushBasedOnProgress(int progress)
        {
            Brush b = BrushExtensions.BasedOnProgress(progress);
            return b;
        }
    }

    public class when_calling_basedonprogress : BrushExtensionsTest
    {
        [Fact]
        public void progress_at_100_returns_green()
        {
            Assert.Equal(Brushes.Green, GetBrushBasedOnProgress(100));
        }

        [Fact]
        public void progress_greater_than_100_returns_green()
        {
            Assert.Equal(Brushes.Green, GetBrushBasedOnProgress(100));
        }

        [Fact]
        public void progress_at_90_returns_green()
        {
            Assert.Equal(Brushes.Green, GetBrushBasedOnProgress(90));
        }

        [Fact]
        public void progress_at_75_returns_green()
        {
            Assert.Equal(Brushes.Green, GetBrushBasedOnProgress(75));
        }

        [Fact]
        public void progress_at_50_returns_greenyellow()
        {
            Assert.Equal(Brushes.GreenYellow, GetBrushBasedOnProgress(50));
        }

        [Fact]
        public void progress_at_60_returns_greenyellow()
        {
            Assert.Equal(Brushes.GreenYellow, GetBrushBasedOnProgress(60));
        }

        [Fact]
        public void progress_at_74_returns_greenyellow()
        {
            Assert.Equal(Brushes.GreenYellow, GetBrushBasedOnProgress(60));
        }

        [Fact]
        public void progress_at_49_returns_yellow()
        {
            Assert.Equal(Brushes.Yellow, GetBrushBasedOnProgress(49));
        }

        [Fact]
        public void progress_at_30_returns_yellow()
        {
            Assert.Equal(Brushes.Yellow, GetBrushBasedOnProgress(30));
        }

        [Fact]
        public void progress_at_25_returns_yellow()
        {
            Assert.Equal(Brushes.Yellow, GetBrushBasedOnProgress(25));
        }

        [Fact]
        public void progress_at_24_returns_orange()
        {
            Assert.Equal(Brushes.Orange, GetBrushBasedOnProgress(24));
        }

        [Fact]
        public void progress_at_20_returns_orange()
        {
            Assert.Equal(Brushes.Orange, GetBrushBasedOnProgress(20));
        }

        [Fact]
        public void progress_at_15_returns_orange()
        {
            Assert.Equal(Brushes.Orange, GetBrushBasedOnProgress(15));
        }

        [Fact]
        public void progress_at_14_returns_orangered()
        {
            Assert.Equal(Brushes.OrangeRed, GetBrushBasedOnProgress(14));
        }

        [Fact]
        public void progress_at_10_returns_orangered()
        {
            Assert.Equal(Brushes.OrangeRed, GetBrushBasedOnProgress(10));
        }

        [Fact]
        public void progress_at_5_returns_orangered()
        {
            Assert.Equal(Brushes.OrangeRed, GetBrushBasedOnProgress(5));
        }

        [Fact]
        public void progress_at_4_returns_red()
        {
            Assert.Equal(Brushes.Red, GetBrushBasedOnProgress(4));
        }

        [Fact]
        public void progress_at_0_returns_red()
        {
            Assert.Equal(Brushes.Red, GetBrushBasedOnProgress(0));
        }
    }
}
