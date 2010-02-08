using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Leonis.TomatoTimer.Plugins;

namespace Leonis.TomatoTimer.UI.Plugins
{
    [Export(typeof(Plugin))]
    [Export(typeof(TimerEventPlugin))]
    public class Mp3PlayerPlugin : TimerEventPlugin
    {
        private readonly MediaPlayer player;

        public Mp3PlayerPlugin()
        {
            player = new MediaPlayer();
        }

        private void PlayFile(string path)
        {
            player.Stop();
            player.Open(new Uri(path, UriKind.Relative));
            player.Play();
        }

        public override string FriendlyName
        {
            get { return "Basic Mp3 Player"; }
        }

        public override string FriendlyStatus
        {
            get { return "TODO: Return Current Status"; }
        }

        public override bool Abortable
        {
            get { return true; }
        }

        public override void Abort()
        {
            player.Stop();
            player.Close();
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
    }
}