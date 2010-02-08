namespace Leonis.TomatoTimer.Plugins
{
    public abstract class Plugin
    {
        public virtual string FriendlyName { get; protected set;}
        public virtual string FriendlyStatus { get; protected set;}
        public virtual bool Abortable { get; protected set; }
        public abstract void Abort();
    }
}