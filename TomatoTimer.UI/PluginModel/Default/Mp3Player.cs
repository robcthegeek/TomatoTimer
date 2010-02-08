using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using TomatoTimer.Core;
using TomatoTimer.Plugins;

namespace TomatoTimer.UI.PluginModel.Default
{
    [Export(typeof(TimerEventPlugin))]
    public class Mp3PlayerPlugin : TimerEventPlugin
    {
        private readonly MediaPlayer player;

        public Mp3PlayerPlugin()
        {
            player = new MediaPlayer();
            player.Volume = 100;
            FriendlyName = "Basic Mp3 Player";
            FriendlyStatus = "TODO: Return Current Status";
        }

        private void PlayFile(string path)
        {
            player.Stop();
            player.Open(new Uri(path, UriKind.Relative));
            player.Play();
        }

        public override void OnStartTomato()
        {
            PlayFile(@"sounds\tomatostarted.mp3");
        }

        public override void OnEndTomato()
        {
            PlayFile(@"sounds\tomatoended.mp3");
        }

        public override void OnStartBreak()
        {
            PlayFile(@"sounds\breakstarted.mp3");
        }

        public override void OnEndBreak()
        {
            PlayFile(@"sounds\breakended.mp3");
        }

        public override void OnStartSetBreak()
        {
            PlayFile(@"sounds\setbreakstarted.mp3");
        }

        public override void OnEndSetBreak()
        {
            PlayFile(@"sounds\setbreakended.mp3");
        }

        public override void OnInterrupt()
        {
            PlayFile(@"sounds\tomatointerrupted.mp3");
        }

        public override void OnTick(TickEventArgs args)
        {
            PlayFile(@"sounds\tick.mp3");
        }
    }
}