using System;
using System.Windows.Forms;
using ManagedWinapi;

namespace TomatoTimer.UI
{
    public class HotKeys
    {
        #region Events
        public event EventHandler<EventArgs> BindHotKeyFailed;
        protected void OnBindHotKeyFailed(EventArgs e)
        {
            if (BindHotKeyFailed != null)
            {
                BindHotKeyFailed(this, e);
            }
        }

        public event EventHandler<EventArgs> StartTomatoHotKeyPressed;
        protected void OnStartTomatoHotKeyPressed(EventArgs e)
        {
            if (StartTomatoHotKeyPressed != null)
            {
                StartTomatoHotKeyPressed(this, e);
            }
        }

        public event EventHandler<EventArgs> StartBreakHotKeyPressed;
        protected void OnStartBreakHotKeyPressed(EventArgs e)
        {
            if (StartBreakHotKeyPressed != null)
            {
                StartBreakHotKeyPressed(this, e);
            }
        }

        public event EventHandler<EventArgs> StartSetBreakHotKeyPressed;
        protected void OnStartSetBreakHotKeyPressed(EventArgs e)
        {
            if (StartSetBreakHotKeyPressed != null)
            {
                StartSetBreakHotKeyPressed(this, e);
            }
        }

        public event EventHandler<EventArgs> InterruptHotKeyPressed;
        protected void OnInterruptHotKeyPressed(EventArgs e)
        {
            if (InterruptHotKeyPressed != null)
            {
                InterruptHotKeyPressed(this, e);
            }
        } 
        #endregion

        /// <summary>
        /// Binds a Hotkey (with WinKey and Control) Using the Managed Win API.
        /// </summary>
        /// <param name="actionName">'Friendly' Name for the Action Being Bound (e.g. 'Start Tomato')<para />
        /// This is used to Feed Back Issues in Events.</param>
        /// <param name="keyCode">Key to Bind</param>
        /// <param name="handler">Delegate to Call When Key Pressed</param>
        /// <returns>Reference to the Hotkey Object.</returns>
        Hotkey InitHotKey(string actionName, Keys keyCode, EventHandler handler)
        {
            var key = new Hotkey
            {
                Ctrl = true,
                WindowsKey = true,
                KeyCode = keyCode
            };
            
            key.HotkeyPressed += handler;
            
            try
            {
                key.Enabled = true;
                return key;
            }
            catch (HotkeyAlreadyInUseException)
            {
                OnBindHotKeyFailed(EventArgs.Empty);
                return null;
            }
        }

        public HotKeys()
        {
            InitHotKey("Start Tomato", Keys.T, startTomato_HotkeyPressed);
            InitHotKey("Start Break", Keys.B, startBreak_HotkeyPressed);
            InitHotKey("Start Set Break", Keys.S, startSetBreak_HotkeyPressed);
            InitHotKey("Interrupt", Keys.I, interrupt_HotkeyPressed);
        }

        private void interrupt_HotkeyPressed(object sender, EventArgs e)
        {
            OnInterruptHotKeyPressed(EventArgs.Empty);
        }

        private void startSetBreak_HotkeyPressed(object sender, EventArgs e)
        {
            OnStartSetBreakHotKeyPressed(EventArgs.Empty);
        }

        private void startBreak_HotkeyPressed(object sender, EventArgs e)
        {
            OnStartBreakHotKeyPressed(EventArgs.Empty);
        }

        private void startTomato_HotkeyPressed(object sender, EventArgs e)
        {
            OnStartTomatoHotKeyPressed(EventArgs.Empty);
        }
    }
}
