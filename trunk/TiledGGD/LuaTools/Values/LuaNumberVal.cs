using System;
using System.Collections.Generic;
using System.Text;

namespace TiledGGD.LuaTools.Values
{
    class LuaNumberVal : LuaValue
    {
        private long val;
        private bool set = false;

        public LuaNumberVal(long num) { this.val = num; this.set = true; }
        public LuaNumberVal() { }

        internal override object getValue()
        {
            if (!set)
                return null;
            return val;
        }
    }
}
