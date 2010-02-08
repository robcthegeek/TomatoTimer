using System.Windows.Media;
using TomatoTimer.UI.Graphics;
using Xunit;

namespace TomatoTimer.Tests.Unit.UI
{
    public class ColorExtensionsTests
    {
        public class when_calling_progressbetweencolours
        {
            [Fact]
            public void result_is_white_for_100pc_black_white()
            {
                Assert.Equal(Colors.White, Colors.Black.FadeToColor(Colors.White, 100));
            }

            [Fact]
            public void result_is_black_for_0pc_black_white()
            {
                Assert.Equal(Colors.Black, Colors.Black.FadeToColor(Colors.White, 0));
            }

            [Fact]
            public void result_is_192_192_192_for_75pc_black_white()
            {
                Assert.Equal(Color.FromRgb(192, 192, 192), Colors.Black.FadeToColor(Colors.White, 75));
            }

            [Fact]
            public void result_is_128_128_128_for_50pc_black_white()
            {
                Assert.Equal(Color.FromRgb(128, 128, 128), Colors.Black.FadeToColor(Colors.White, 50));
            }

            [Fact]
            public void result_is_64_64_64_for_25pc_black_white()
            {
                Assert.Equal(Color.FromRgb(64, 64, 64), Colors.Black.FadeToColor(Colors.White, 25));
            }

            [Fact]
            public void result_is_26_26_26_for_10pc_black_white()
            {
                Assert.Equal(Color.FromRgb(26, 26, 26), Colors.Black.FadeToColor(Colors.White, 10));
            }

            [Fact]
            public void result_is_95_95_95_for_37pc_black_white()
            {
                Assert.Equal(Color.FromRgb(95, 95, 95), Colors.Black.FadeToColor(Colors.White, 37));
            }

            [Fact]
            public void result_is_255_0_0_for_100pc_green_red()
            {
                var actual = Colors.Lime.FadeToColor(Colors.Red, 100);
                Assert.Equal(Color.FromRgb(255,0,0), actual);
            }
        }

        public class When_Calling_FromString
        {
            [Fact]
            public void Pascalcase_Black_Returns_Black()
            {
                var res = ColorExtensions.FromString("Black");
                Assert.Equal(Colors.Black, res);
            }
            
            [Fact]
            public void Lowercase_Black_Returns_Black()
            {
                var res = ColorExtensions.FromString("black");
                Assert.Equal(Colors.Black, res);
            }

            [Fact]
            public void Pascalcase_White_Returns_White()
            {
                var res = ColorExtensions.FromString("White");
                Assert.Equal(Colors.White, res);
            }
            
            [Fact]
            public void Lowercase_white_Returns_White()
            {
                var res = ColorExtensions.FromString("white");
                Assert.Equal(Colors.White, res);
            }

            [Fact]
            public void Hex_000000_Returns_Black()
            {
                var res = ColorExtensions.FromString("000000");
                Assert.Equal(Colors.Black, res);
            }

            [Fact]
            public void Hex_FF0000_Returns_Red()
            {
                var res = ColorExtensions.FromString("FF0000");
                Assert.Equal(Colors.Red, res);
            }

            [Fact]
            public void Hex_Hash0000FF_Returns_Blue()
            {
                var res = ColorExtensions.FromString("#0000FF");
                Assert.Equal(Colors.Blue, res);
            }

            [Fact]
            public void ARGB_FFFFFFFF_Returns_White()
            {
                var res = ColorExtensions.FromString("FFFFFFFF");
                Assert.Equal(Colors.White, res);
            }

            [Fact]
            public void ARGB_00FFFFFF_Returns_ZeroOpacityWhite()
            {
                var res = ColorExtensions.FromString("00FFFFFF");
                var expected = Color.FromArgb(0, 255, 255, 255);
                Assert.Equal(expected, res);
            }

            [Fact]
            public void EmptyString_Returns_ARGB00000000()
            {
                var expected = Color.FromArgb(0, 0, 0, 0);
                var actual = ColorExtensions.FromString(string.Empty);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullString_Returns_ARGB00000000()
            {
                var expected = Color.FromArgb(0, 0, 0, 0);
                var actual = ColorExtensions.FromString(null);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Back_Returns_ARGB00000000()
            {
                // Checking for Incorrect Attempts @ A Color.
                var expected = Color.FromArgb(0, 0, 0, 0);
                var actual = ColorExtensions.FromString("Back");
                Assert.Equal(expected, actual);
            }
        }

        public class When_Calling_ToKnownOrHex
        {
            [Fact]
            public void Red_Returns_Red()
            {
                // Red is a "Known Color"
                var actual = Colors.Red.ToKnownOrHex();
                Assert.Equal("Red", actual);
            }

            [Fact]
            public void HexFF0000_Returns_Red()
            {
                var actual = Color.FromRgb(255, 0, 0).ToKnownOrHex();
                Assert.Equal("Red", actual);
            }

            [Fact]
            public void HexF0F012_Returns_HashF0F012()
            {
                var actual = Color.FromArgb(255, 240, 240, 18).ToKnownOrHex();
                Assert.Equal("#F0F012", actual);
            }

            [Fact]
            public void Hex000000_Returns_Black()
            {
                var actual = Color.FromRgb(0, 0, 0).ToKnownOrHex();
                Assert.Equal("Black", actual);
            }
        }
    }
}