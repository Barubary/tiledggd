using System;
using System.Collections.Generic;
using System.Text;

namespace TiledGGD.BindingTools
{
    class PluginException : Exception
    {
        public PluginException() : base() { }
        public PluginException(string message) : base(message) { }
        public PluginException(string message, Exception inner) : base(message, inner) { }
    }
}
