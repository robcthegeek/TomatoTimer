namespace TomatoTimer.UI.Core
{
    public class AppController
    {
        // This class should be responsible for spinning up the required components for the App.

        /* Namely:
         * 
         * - Core Timer
         * - Configuration (via App/User Settings). Used to setup the Core Timer.
         * - Plugins (via MEF, but should be abstract).
         * 
         * The main app thread can then spin up the core WPF UI, and hook in to the controller.
         */ 
    }
}