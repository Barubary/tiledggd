using System;
using System.Collections.Generic;
using System.Text;

namespace TiledGGD.LuaTools.Values
{
    abstract class LuaValue
    {
        /// <summary>
        /// Gets the actual value of this LuaValue
        /// </summary>
        /// <returns></returns>
        abstract internal object getValue();
    }
}
