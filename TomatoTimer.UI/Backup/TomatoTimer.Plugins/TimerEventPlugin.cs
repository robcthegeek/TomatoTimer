namespace Leonis.TomatoTimer.Plugins
{
    public abstract class TimerEventPlugin : Plugin
    {
        public abstract void OnStartTomato();
        public abstract void OnEndTomato();
        public abstract void OnStartBreak();
        public abstract void OnEndBreak();
        public abstract void OnStartSetBreak();
        public abstract void OnEndSetBreak();
        public abstract void OnInterrupt();
    }
}