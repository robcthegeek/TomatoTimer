using System.IO;
using System.Reflection;

namespace TomatoTimer.Core
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns the Path for the Assembly Passed.<para />
        /// Uses Reflection to Determine the CodeBase.
        /// </summary>
        /// <param name="assembly">Assembly to Retrieve CodeBase Path For.</param>
        /// <returns>string.Empty if the Assembly is Null</returns>
        public static string Directory(this Assembly assembly)
        {
            if (assembly == null)
                return string.Empty;

            var codebase = assembly.CodeBase.Replace("file:///", string.Empty);
            var dir = Path.GetDirectoryName(codebase) + Path.DirectorySeparatorChar;
            return dir;
        }
    }
}