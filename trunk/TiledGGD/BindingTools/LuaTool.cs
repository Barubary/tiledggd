using System;
using System.Collections.Generic;
using System.Text;

namespace TiledGGD.BindingTools
{
    /// <summary>
    /// A class that loads, parses and applies a Lua plugin
    /// </summary>
    class LuaTool : IDisposable
    {
        #region Fields & Properties
        /// <summary>
        /// The parent binding of this LuaTool
        /// </summary>
        private Binding parentBinding;
        /// <summary>
        /// The parent binding of this LuaTool
        /// </summary>
        public Binding Parent { get { return this.parentBinding; } }

        /// <summary>
        /// The name of the plugin-file this LuaTool uses
        /// </summary>
        private string luaFile;
        /// <summary>
        /// The name of the plugin-file this LuaTool uses
        /// </summary>
        public string PluginFileName { get { return this.luaFile; } }
        #endregion

        /// <summary>
        /// Create a new LuaTool
        /// </summary>
        /// <param name="parent">The parent Binding</param>
        /// <param name="plugin">The path to the Lua plugin-file</param>
        public LuaTool(Binding parent, string plugin)
        {
            this.parentBinding = parent;
            this.luaFile = plugin;
        }


        internal void loadFile(String filename)
        {
            if (string.IsNullOrEmpty(luaFile))
                throw new Exception("No Lua file specified");
        }



        #region IDisposable Members

        public void Dispose()
        {
            this.parentBinding = null;
        }

        #endregion
    }
}
