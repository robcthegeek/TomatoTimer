using System.Reflection;
using Leonis.TomatoTimer.Core;

namespace Leonis.TomatoTimer.Tests
{
    class Utility
    {
        public static string AssemblyPath
        {
            get
            {
                string dir = Assembly.GetExecutingAssembly().Directory();
                return dir;
            }
        }
    }
}