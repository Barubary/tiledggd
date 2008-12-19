using System;
using System.Collections.Generic;
using System.Text;
using TiledGGD.LuaTools.Values;

namespace TiledGGD.LuaTools.Expressions
{
    class LuaExpressionSequence : LuaExpression
    {
        private List<LuaExpression> exprs;

        /// <summary>
        /// Create a new (empty) LuaExpression sequence
        /// </summary>
        public LuaExpressionSequence()
        {
            this.exprs = new List<LuaExpression>();
        }

        /// <summary>
        /// Gets or sets an expression in this expression sequence
        /// </summary>
        /// <param name="index">The index of the expression</param>
        /// <returns>The index-th LuaExpression</returns>
        /// <exception cref="NullReferenceException">If the list of expressions is not yet initialized</exception>
        /// <exception cref="IndexOutOfRangeException">If the requested Expression does not exist</exception>
        internal LuaExpression this[int index]
        {
            get
            {
                return this.exprs[index];
            }
            set
            {
                this.exprs[index] = value;
            }
        }

        /// <summary>
        /// Adds an expression to this ExpressionSequence
        /// </summary>
        /// <param name="exp">The LuaExpression to add.</param>
        internal void addExpression(LuaExpression exp)
        {
            this.exprs.Add(exp);
        }

        internal override LuaValue interpret(ref Dictionary<string, LuaExpression> environment)
        {
            for (int i = 0; i < exprs.Count - 1; i++)
                exprs[i].interpret(ref environment);
            return exprs[exprs.Count - 1].interpret(ref environment);
        }
    }
}
