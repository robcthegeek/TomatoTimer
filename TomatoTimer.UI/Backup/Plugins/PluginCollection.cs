using System;
using System.Collections.Generic;
using System.ComponentModel;
using Leonis.TomatoTimer.Plugins;

namespace Leonis.TomatoTimer.UI.Plugins
{
    public class PluginCollection<T> where T : Plugin
    {
        public List<T> Plugins { get; private set; }
        public Dictionary<T, BackgroundWorker> ExecutingPlugins { get; private set; }

        public PluginCollection(List<T> plugins)
        {
            Plugins = plugins;
            ExecutingPlugins = new Dictionary<T, BackgroundWorker>();
        }

        public void Abort()
        {
            foreach (var plugin in ExecutingPlugins)
            {
                if (plugin.Key.Abortable)
                {
                    plugin.Key.Abort();
                    //ExecutingPlugins[plugin].CancelAsync();
                }
            }
        }

        public void Execute(Action<T> action)
        {
            foreach (var plugin in Plugins)
            {
                RunPlugin(plugin, action);
            }
        }

        private void RunPlugin(T plugin, Action<T> action)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
                                 {
                                     AddPluginToCollection(plugin, worker);
                                     action(plugin);
                                 };

            worker.RunWorkerCompleted += (sender, args) => RemovePluginFromCollection(plugin);
            worker.RunWorkerAsync();

            ExecutingPlugins.Add(plugin, worker);
        }

        void AddPluginToCollection(T plugin, BackgroundWorker worker)
        {
            ExecutingPlugins.Add(plugin, worker);
        }

        void RemovePluginFromCollection(T plugin)
        {
            ExecutingPlugins.Remove(plugin);
        }
    }
}