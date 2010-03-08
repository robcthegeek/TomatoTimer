using System;
using System.ComponentModel.Composition;
using TomatoTimer.Core;
using TomatoTimer.Plugins;

namespace TomatoTimer.UI.PluginModel.Default
{
    [Export(typeof(TimerEventPlugin))]
    public class MiniTimerWindow : TimerEventPlugin
    {
        private readonly MiniTimer win;

        public MiniTimerWindow()
        {
            win = new MiniTimer();
        }

        public override void OnStartTomato()
        {
            win.TomatoStarted();
        }

        public override void OnEndTomato()
        {
            win.TomatoEnded();
        }

        public override void OnStartBreak()
        {
            win.BreakStarted();
        }

        public override void OnEndBreak()
        {
            win.BreakEnded();
        }

        public override void OnStartSetBreak()
        {
            win.SetBreakStarted();
        }

        public override void OnEndSetBreak()
        {
            win.SetBreakEnded();
        }

        public override void OnInterrupt()
        {
            win.Interrupted();
        }

        public override void OnTick(TickEventArgs args)
        {
            var totalTime = args.TimeElapsed.Add(args.TimeRemaining);
            var totalTicks = totalTime.Ticks;
            var ticksElapsed = args.TimeElapsed.Ticks;
            var progComplete = (ticksElapsed/totalTicks) * 100;
            win.Update(args.TimeRemaining, (int)progComplete);
        }
    }
}
