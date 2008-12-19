using System;
using System.Collections.Generic;
using System.Text;
using TiledGGD.LuaTools.Expressions;
using TiledGGD.LuaTools.Values;

namespace TiledGGD.LuaTools
{
    class LuaParser
    {

        public static LuaExpression Parse(string statement)
        {
            return null;
        }

        #region operator idetifier functions (static)
        /// <summary>
        /// Checks if a LuaOperator is a Unary Operator
        /// </summary>
        /// <param name="op">The op to check</param>
        /// <returns><code>true</code> iff the op represents a unary operator</returns>
        public static bool IsUnop(LuaOperator op)
        {
            return ((long)op & 4) > 0;
        }

        /// <summary>
        /// Checks if a LuaOperator is a Binary Operator
        /// </summary>
        /// <param name="op">The op to check</param>
        /// <returns><code>true</code> iff the op represents a binary operator</returns>
        public static bool IsBinop(LuaOperator op)
        {
            return ((long)op & 32) > 0;
        }
        #endregion
    }

    #region Lua BCNF
    /* (strings starting with _ used to be bold; _ is never used in the original BCNF)
    chunk ::= {stat [`;´]} [laststat [`;´]]

	block ::= chunk

	stat ::=  varlist `=´ explist | 
		 functioncall | 
		 _do block _end | 
		 _while exp _do block _end | 
		 _repeat block _until exp | 
		 _if exp _then block {_elseif exp _then block} [_else block] _end | 
		 _for Name `=´ exp `,´ exp [`,´ exp] _do block _end | 
		 _for namelist _in explist _do block _end | 
		 _function funcname funcbody | 
		 _local _function Name funcbody | 
		 _local namelist [`=´ explist] 

	laststat ::= _return [explist] | _break

	funcname ::= Name {`.´ Name} [`:´ Name]

	varlist ::= var {`,´ var}

	var ::=  Name | prefixexp `[´ exp `]´ | prefixexp `.´ Name 

	namelist ::= Name {`,´ Name}

	explist ::= {exp `,´} exp

	exp ::=  _nil | _false | _true | Number | String | `...´ | function | 
		 prefixexp | tableconstructor | exp binop exp | unop exp 

	prefixexp ::= var | functioncall | `(´ exp `)´

	functioncall ::=  prefixexp args | prefixexp `:´ Name args 

	args ::=  `(´ [explist] `)´ | tableconstructor | String 

	function ::= _function funcbody

	funcbody ::= `(´ [parlist] `)´ block _end

	parlist ::= namelist [`,´ `...´] | `...´

	tableconstructor ::= `{´ [fieldlist] `}´

	fieldlist ::= field {fieldsep field} [fieldsep]

	field ::= `[´ exp `]´ `=´ exp | Name `=´ exp | exp

	fieldsep ::= `,´ | `;´

	binop ::= `+´ | `-´ | `*´ | `/´ | `^´ | `%´ | `..´ | 
		 `<´ | `<=´ | `>´ | `>=´ | `==´ | `~=´ | 
		 _and | _or

	unop ::= `-´ | _not | `_#´
    */
    #endregion

    #region operator enum
    public enum LuaOperator : long
    {
        UNOP_NEG = 4, //0b100, // -
        UNOP_NOT = 5, //0x101, // not
        UNOP_HASH = 6, //0x110, // # (length)

        BINOP_ADD = 32, //0x100000, // +
        BINOP_SUB = 33, //0x100001, // -
        BINOP_MUL = 34, //0x100010, // *
        BINOP_DIV = 35, //0x100011, // /
        BINOP_POW = 40, //0x101000, // ^
        BINOP_MOD = 41, //0x101001, // %
        BINOP_DD = 42, //0x101010, // .. (concatenation)
        BINOP_LT = 43, //0x101011, // <
        BINOP_LE = 48, //0x110000, // <=
        BINOP_GT = 49, //0x110001, // >
        BINOP_GE = 50, //0x110010, // >=
        BINOP_EQ = 51, //0x110011, // ==
        BINOP_NE = 56, //0x111000, // ~=
        BINOP_AND = 57, //0x111001, // and
        BINOP_OR = 58, //0x1110010 // or
    }
    #endregion

    #region Operator precedence list
    /*
     or
     and
     <     >     <=    >=    ~=    ==
     ..
     +     -
     *     /     %
     not   #     - (unary)
     ^
     */
    #endregion

}
