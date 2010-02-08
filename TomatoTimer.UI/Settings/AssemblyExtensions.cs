using System.Reflection;

namespace TomatoTimer.UI.Settings
{
    public static class AssemblyExtensions
    {
        public static string Version(this Assembly asm)
        {
            var version = asm.GetName().Version;
            return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
    }
}
