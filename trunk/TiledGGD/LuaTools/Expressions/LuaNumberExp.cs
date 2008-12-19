using System;
using System.Collections.Generic;
using System.Text;
using TiledGGD.LuaTools.Values;

namespace TiledGGD.LuaTools.Expressions
{
    /// <summary>
    /// A Number LuaExpression
    /// </summary>
    class LuaNumberExp : LuaExpression
    {
        private long num;
        private bool set = false;

        /// <summary>
        /// Create a new LuaNumber
        /// </summary>
        /// <param name="num">The value of this LuaNumber</param>
        public LuaNumberExp(long num) { this.num = num; this.set = true; }

        /// <summary>
        /// Create an empty LuaNumber
        /// </summary>
        public LuaNumberExp() { }

        internal override LuaValue interpret(ref Dictionary<string, LuaExpression> environment)
        {
            if (!set)
                return null;
            return new LuaNumberVal(num);
        }
    }
}
