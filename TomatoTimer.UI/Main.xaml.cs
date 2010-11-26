using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using TomatoTimer.Core;
using TomatoTimer.Plugins;
using TomatoTimer.UI.Graphics;
using TomatoTimer.UI.Plugins;
using TomatoTimer.Core.Plugins;

namespace TomatoTimer.UI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private readonly ITomatoTimer timer;
        // HACK: This is a real hack to allow the MiniTimer/NotifyIcon Plugins Interim Access to Running Timer Core.
        private static Main instance;
        public static Main GetInstance()
        {
            return instance;
        }

        private HotKeys hotkeys;

        public ITomatoTimer Timer { get; private set; }

        [ImportMany(typeof(TimerEventPlugin))]
        List<TimerEventPlugin> TimerEventPluginImports { get; set; }

        public Main(ITomatoTimer timer)
        {
            Timer = timer;
            instance = this;
            InitializeComponent();

            SetupTransitionMenu();
            InitialisePlugins();
            SetWindowTitle();
            BindToTimerEvents();            
            InitHotKeys();
        }

        private void InitialisePlugins()
        {
            var asmCat = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(asmCat);
            container.ComposeParts(this);
        }

        private void SetWindowTitle()
        {
            var version = string.Format(
                "(v{0}.{1}.{2})",
                Environment.Version.Major,
                Environment.Version.Minor,
                Environment.Version.Revision);
            var appTitle = Constants.APP_TITLE;
            Title = string.Format("{0} {1}", appTitle, version);
        }

        private void InitHotKeys()
        {
            hotkeys = new HotKeys();
            hotkeys.StartTomatoHotKeyPressed += delegate { Timer.StartTomato(); };
            hotkeys.StartBreakHotKeyPressed += delegate { Timer.StartBreak(); };
            hotkeys.StartSetBreakHotKeyPressed += delegate { Timer.StartSetBreak(); };
            hotkeys.InterruptHotKeyPressed += delegate { Timer.Interrupt(); };
        }

        void BindToTimerEvents()
        {
                Timer.TomatoStarted += timer_TomatoStarted;
                Timer.TomatoEnded += timer_TomatoEnded;
                Timer.BreakStarted += timer_BreakStarted;
                Timer.BreakEnded += timer_BreakEnded;
                Timer.SetBreakStarted += timer_SetBreakStarted;
                Timer.SetBreakEnded += timer_SetBreakEnded;
                Timer.StateChangeFailed += timer_StateChangeFailed;
                Timer.Interrupted += timer_Interrupted;
                Timer.Tick += timer_Tick;
        }

        private void ExecuteTimerEvent(Action<TimerEventPlugin> action)
        {
            foreach (var plugin in TimerEventPluginImports)
            {
                Action actionToRun = () => {
                        action(plugin);
                    };
                var method = new ParallelAsyncMethod(actionToRun);
                method.Run();
            }
        }

        private void timer_Tick(object sender, TickEventArgs e)
        {
            ExecuteTimerEvent(p => p.OnTick(e));
            UpdateProgBar();
        }

        private void SetupTransitionMenu()
        {
            LblState.Content = "Please Start a Tomato, Break or Set Break";
            LblRemaining.Visibility = Visibility.Collapsed;
            ProgRemaining.Value = 0;
            ToggleTransitionButtons(true);
        }

        void ToggleTransitionButtons(bool show)
        {
            BtnInterrupt.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
            BtnStartTomato.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            BtnStartBreak.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            BtnStartSetBreak.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetupRunningMenu()
        {
            LblRemaining.Visibility = Visibility.Visible;
            ToggleTransitionButtons(false);
        }

        public void PopupWindow()
        {
            if (WindowState != WindowState.Normal)
            {
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
            }

            Show();
            Activate();
        }

        #region Timer Event Handlers
        void timer_Interrupted(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnInterrupt());
            SetupTransitionMenu();
            PopupWindow();
        }

        void timer_SetBreakStarted(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnStartSetBreak());
            ProgRemaining.Maximum = Timer.SetBreakTimeSpan.Ticks;
            LblState.Content = "Set Break Running";
            SetupRunningMenu();
        }

        private void timer_SetBreakEnded(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnEndSetBreak());
            SetupTransitionMenu();
        }

        void timer_StateChangeFailed(object sender, StateChangeFailedEventArgs e)
        {
            // Don't do anything with this yet.
        }

        void timer_BreakStarted(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnStartSetBreak());
            ProgRemaining.Maximum = Timer.BreakTimeSpan.Ticks;
            LblState.Content = "Break Running";
            SetupRunningMenu();
        }

        private void timer_BreakEnded(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnEndBreak());
            SetupTransitionMenu();
        }

        void timer_TomatoEnded(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnEndTomato());
            SetupTransitionMenu();
        }

        void timer_TomatoStarted(object sender, EventArgs e)
        {
            ExecuteTimerEvent(p => p.OnStartTomato());
            WindowState = WindowState.Minimized;
            ProgRemaining.Maximum = Timer.TomatoTimeSpan.Ticks;
            LblState.Content = "Tomato Running";
            SetupRunningMenu();
        } 
        #endregion

        void UpdateProgBar()
        {
            if (!Timer.Running) return;

            var t = Timer.TimeRemaining;
            var ticksRemaining = t.Ticks;
            var ticksMax = ProgRemaining.Maximum;
            var ticksComplete = (ticksRemaining / ticksMax) * 100;

            // RC: Do NOT Repaint the Progress Bar if Minimised
            // This is bloody expensive!
            if (WindowState != WindowState.Minimized)
            {
                LblRemaining.Content = t.ToFriendly();
                ProgRemaining.Value = ticksRemaining;
                var brush = BrushExtensions.BasedOnProgress((int)ticksComplete);
                ProgRemaining.Foreground = brush;
            }
        }

        private void BtnStartTomato_Click(object sender, RoutedEventArgs e)
        {
            Timer.StartTomato();
            UpdateProgBar();
        }

        private void BtnStartBreak_Click(object sender, RoutedEventArgs e)
        {
            Timer.StartBreak();
            UpdateProgBar();
        }

        private void BtnStartSetBreak_Click(object sender, RoutedEventArgs e)
        {
            Timer.StartSetBreak();
            UpdateProgBar();
        }

        private void BtnInterrupt_Click(object sender, RoutedEventArgs e)
        {
            Timer.Interrupt();
        }
    }
}