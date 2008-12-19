using System;
using System.Collections.Generic;
using System.Text;
using TiledGGD.LuaTools.Values;

namespace TiledGGD.LuaTools.Expressions
{
    abstract class LuaExpression
    {
        /// <summary>
        /// Interpret this LuaExpression into a LuaValue
        /// </summary>
        /// <param name="environment">The current environment</param>
        /// <returns>The LuaValue this Expression produces</returns>
        internal abstract LuaValue interpret(ref Dictionary<string, LuaExpression> environment);

    }
}
