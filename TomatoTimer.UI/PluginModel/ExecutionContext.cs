namespace TomatoTimer.UI.PluginModel
{
    public class ExecutionContext
    {
        private bool abortable = true;
        public bool Abortable
        {
            get { return abortable; }
            set { abortable = value; }
        }
    }
}
