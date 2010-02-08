using System.Reflection;
using TomatoTimer.Core;

namespace TomatoTimer.Tests.Unit
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