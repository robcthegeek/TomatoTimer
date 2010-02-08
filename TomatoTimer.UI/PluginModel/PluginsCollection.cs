using System;
using System.Collections;
using System.Collections.Generic;
using TomatoTimer.Plugins;

namespace TomatoTimer.UI.PluginModel
{
    public class PluginsCollection<T> : IEnumerable where T : Plugin
    {
        public IAsyncMethodManager<T> Manager { get; private set; }
        List<T> Plugins { get; set; }

        /// <summary>
        /// Returns the Number of Plugins in the Collection.
        /// </summary>
        public int Count
        {
            get
            {
                return Plugins.Count;
            }
        }

        /// <summary>
        /// Initialises the Plugins Collection with Zero Plugins.
        /// </summary>
        /// <param name="manager">Manager to Manage Execution of Methods Asynchronously.</param>
        public PluginsCollection(IAsyncMethodManager<T> manager)
        {
            Manager = manager;
            Plugins = new List<T>();
        }

        /// <summary>
        /// Initialises the Plugins Collection with the Plugins Collection Passed.
        /// </summary>
        /// <param name="manager">Manager to Manage Execution of Methods Asynchronously.</param>
        /// <param name="plugins">Plugins to Add to the Plugins Collection.</param>
        public PluginsCollection(IAsyncMethodManager<T> manager, List<T> plugins) : this(manager)
        {
            Plugins = plugins;
        }

        /// <summary>
        /// Add a Plugin to the Collection.
        /// </summary>
        /// <param name="plugin">Plugin to Add.</param>
        public void Add(T plugin)
        {
            Plugins.Add(plugin);
        }

        /// <summary>
        /// Clears All Plugins from the Collection.
        /// </summary>
        public void Clear()
        {
            Plugins.Clear();
        }

        /// <summary>
        /// Returns an IEnumerator Instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return Plugins.GetEnumerator();
        }

        /// <summary>
        /// Executes a Plugin Method.
        /// </summary>
        /// <param name="action">Plugin Method to Execute.</param>
        public void Execute(Action<T, ExecutionContext> action)
        {
            foreach (var plugin in Plugins)
            {
                var p = plugin;
                Manager.ExecuteAsync(p, action);
            }
        }
    }
}
