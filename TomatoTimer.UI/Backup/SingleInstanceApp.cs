using System;

namespace Leonis.TomatoTimer.UI
{
    public class SingleInstanceApp
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Create our new single-instance manager
            var manager = new SingleInstanceManager();
            manager.Run(args);
        }
    }

}
