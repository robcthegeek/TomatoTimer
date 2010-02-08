using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Leonis.TomatoTimer.Core;
using Leonis.TomatoTimer.Plugins;
using Leonis.TomatoTimer.UI.Graphics;
using Leonis.TomatoTimer.UI.Plugins;
using Leonis.TomatoTimer.UI.Settings;
using Timer=Leonis.TomatoTimer.Core.CoreTimer;

namespace Leonis.TomatoTimer.UI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        [ImportMany(typeof(Plugin))]
        List<Plugin> Plugins { get; set; }
        
        [ImportMany(typeof(TimerEventPlugin))]
        List<TimerEventPlugin> TimerEventPluginImports { get; set; }

        PluginCollection<TimerEventPlugin> TimerEventPlugins { get; set; }

        private Timer timer;
        private DispatcherTimer displayTimer;
        private NotifyIconComponent notify;
        private readonly MiniTimer mini;
        private HotKeys hotkeys;

        public Main()
        {
            InitializeComponent();

            InitialisePlugins();

            SetWindowTitle();
            SetupDisplayTimer();
            InitTomatoTimer();            
            mini = new MiniTimer(timer);
            SetupNotifyIcon();

            InitHotKeys();
        }

        private void InitialisePlugins()
        {
            // TODO: Spin Up the Plugins Loaded in the Current Directory and "Plugins" Directory.
            var asmCat = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(asmCat);
            container.ComposeParts(this);

            TimerEventPlugins = new PluginCollection<TimerEventPlugin>(TimerEventPluginImports);

            // TODO: Bind the Plugins to the Timer Events.
        }

        private void SetWindowTitle()
        {
            var version = Assembly.GetExecutingAssembly().Version();
            var appTitle = Current.Application.AppTitle;
            Title = string.Format("{0} (v{1})", appTitle, version);
        }

        private void InitHotKeys()
        {
            hotkeys = new HotKeys();
            hotkeys.StartTomatoHotKeyPressed += delegate { timer.StartTomato(); };
            hotkeys.StartBreakHotKeyPressed += delegate { timer.StartBreak(); };
            hotkeys.StartSetBreakHotKeyPressed += delegate { timer.StartSetBreak(); };
            hotkeys.InterruptHotKeyPressed += delegate { timer.Interrupt(); };
        }

        private void SetupDisplayTimer()
        {
            displayTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = new TimeSpan(0, 0, 0, 1),
                IsEnabled = true
            };
            displayTimer.Tick += UpdateUI;
            displayTimer.Start();
        }

        void InitTomatoTimer()
        {
                // Load Timings from Configuration.
                int tomatoLen = Current.User.TomatoTime;
                int breakLen = Current.User.BreakTime;
                int setBreakLen = Current.User.SetBreakTime;

                timer = new Timer(new TimerComponent())
                            {
                                TomatoTimeSpan = new TimeSpan(0, 0, tomatoLen, 0),
                                BreakTimeSpan = new TimeSpan(0, 0, breakLen, 0),
                                SetBreakTimeSpan = new TimeSpan(0, 0, setBreakLen, 0)
                            };

#if DEBUG
                var time = new TimeSpan(0, 0, 0, 10);
                timer.TomatoTimeSpan = time;
                timer.BreakTimeSpan = time;
                timer.SetBreakTimeSpan = time;
#endif

                timer.TomatoStarted += timer_TomatoStarted;
                timer.TomatoEnded += timer_TomatoEnded;
                timer.BreakStarted += timer_BreakStarted;
                timer.BreakEnded += timer_BreakEnded;
                timer.SetBreakStarted += timer_SetBreakStarted;
                timer.SetBreakEnded += timer_SetBreakEnded;
                timer.StateChangeFailed += timer_StateChangeFailed;
                timer.Interrupted += timer_Interrupted;
                timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, TickEventArgs e)
        {
            var t = e.TimeRemaining;
            Console.WriteLine(string.Format("Timer TimeRemaining: {0}\tElapsed: {1}", e.TimeRemaining, e.TimeElapsed));
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
            mini.Update(t, (int)ticksComplete);
        }

        private void SetupNotifyIcon()
        {
            notify = new NotifyIconComponent(this)
                         {
                             IconTitle = Current.Application.AppTitle
                         };
            notify.TrayIconDoubleClicked += notify_TrayIconDoubleClicked;
            notify.TrayIconMouseMoved += notify_TrayIconMouseMoved;

            SetupTransitionMenu();
        }

        void notify_TrayIconMouseMoved(object sender, EventArgs e)
        {
            if (timer.Running)
                ShowTimeRemainingBalloon();
        }

        private void SetupTransitionMenu()
        {
            mini.Hide();
            LblState.Content = "Please Start a Tomato, Break or Set Break";
            notify.ClearMenu();
            notify.AddMenuItem("Start &Tomato", Menu_StartTomato);
            notify.AddMenuItem("Start &Break", Menu_StartBreak);
            notify.AddMenuItem("Start &Set Break", Menu_StartSetBreak);
            notify.AddMenuItem("-", null);
            notify.AddMenuItem("Exit", Menu_Exit);

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

        private void Menu_Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupRunningMenu()
        {
            LblRemaining.Visibility = Visibility.Visible;
            ToggleTransitionButtons(false);
            notify.ClearMenu();
            notify.AddMenuItem("Interrupt", Menu_Interrupt);
            mini.Show();
            UpdateUI(this, EventArgs.Empty);
        }

        private void Menu_Interrupt(object sender, EventArgs e)
        {
            timer.Interrupt();
        }

        private void Menu_StartSetBreak(object sender, EventArgs e)
        {
            timer.StartSetBreak();
        }

        private void Menu_StartBreak(object sender, EventArgs e)
        {
            timer.StartBreak();
        }

        void Menu_StartTomato(object sender, EventArgs e)
        {
            timer.StartTomato();
        }

        void notify_TrayIconDoubleClicked(object sender, EventArgs e)
        {
            PopupWindow();
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
            TimerEventPlugins.Execute(p => p.OnInterrupt());

            notify.ShowBalloon("Interrupted", "Tomato/Break/Set Break Interrupted!");
            SetupTransitionMenu();
            PopupWindow();
        }

        void timer_SetBreakStarted(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnStartSetBreak());

            ProgRemaining.Maximum = timer.SetBreakTimeSpan.Ticks;
            LblState.Content = "Set Break Running";
            SetupRunningMenu();
        }

        private void timer_SetBreakEnded(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnEndSetBreak());

            notify.ShowBalloon("Set Break Ended", "Hope you enjoyed the break..", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        void timer_StateChangeFailed(object sender, StateChangeFailedEventArgs e)
        {
            // Don't do anything with this yet.
        }

        void timer_BreakStarted(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnStartBreak());

            ProgRemaining.Maximum = timer.BreakTimeSpan.Ticks;
            LblState.Content = "Break Running";
            SetupRunningMenu();
        }

        private void timer_BreakEnded(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnEndBreak());

            notify.ShowBalloon("Break Ended", "Ready to destroy another Tomato?", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        void timer_TomatoEnded(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnEndTomato());

            notify.ShowBalloon("Tomato Completed", "Relax, Take a Break or Set Break - Good Job!", ToolTipIcon.Info);
            SetupTransitionMenu();
        }

        void timer_TomatoStarted(object sender, EventArgs e)
        {
            TimerEventPlugins.Execute(p => p.OnStartTomato());

            WindowState = WindowState.Minimized;
            ProgRemaining.Maximum = timer.TomatoTimeSpan.Ticks;
            notify.ShowBalloon("Tomato Started", "Stay Focused!", 250, ToolTipIcon.Info);
            LblState.Content = "Tomato Running";
            SetupRunningMenu();
        } 
        #endregion

        void UpdateUI(object sender, EventArgs e)
        {
            if (!timer.Running) return;

            //var t = timer.TimeRemaining;
            //var ticksRemaining = t.Ticks;
            //var ticksMax = ProgRemaining.Maximum;
            //var ticksComplete = (ticksRemaining/ticksMax)*100;

            //// RC: Do NOT Repaint the Progress Bar if Minimised
            //// This is bloody expensive!
            //if (WindowState != WindowState.Minimized)
            //{
            //    LblRemaining.Content = t.ToFriendly();
            //    ProgRemaining.Value = ticksRemaining;
            //    var brush = BrushExtensions.BasedOnProgress((int) ticksComplete);
            //    ProgRemaining.Foreground = brush;
            //}
            //mini.Update(t, (int) ticksComplete);
        }

        private void BtnStartTomato_Click(object sender, RoutedEventArgs e)
        {
            timer.StartTomato();
        }

        private void BtnStartBreak_Click(object sender, RoutedEventArgs e)
        {
            timer.StartBreak();
        }

        private void BtnStartSetBreak_Click(object sender, RoutedEventArgs e)
        {
            timer.StartSetBreak();
        }

        private void BtnInterrupt_Click(object sender, RoutedEventArgs e)
        {
            timer.Interrupt();
        }

        private void ShowTimeRemainingBalloon()
        {
            var t = timer.TimeRemaining;
            notify.ShowBalloon("Tomato Timer",
                string.Format("Tomato Time Remaining: {0}", t.ToFriendly()));
        }
    }
}