using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;

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

        /// <summary>
        /// The BrowsableData that will get its data from this LuaTool
        /// </summary>
        private BrowsableData bData;

        /// <summary>
        /// The current interpreter. only exists while loadFile is being called.
        /// </summary>
        private Lua interp;

        /// <summary>
        /// The current data. only exists while loadfile is being called.
        /// </summary>
        private byte[] theData;

        /// <summary>
        /// If one of the setData methods has been used
        /// </summary>
        private bool usedSetData = false;
        #endregion

        /// <summary>
        /// Create a new LuaTool
        /// </summary>
        /// <param name="parent">The parent Binding</param>
        /// <param name="plugin">The path to the Lua plugin-file</param>
        public LuaTool(Binding parent, string plugin)
        {
            this.parentBinding = parent;
            this.luaFile = MainWindow.getPath() + "Plugins/" + plugin;

            switch (this.parentBinding.BindingType)
            {
                case BindingType.GRAPHICS: bData = MainWindow.GraphData; break;
                case BindingType.PALETTE: bData = MainWindow.PalData; break;
            }
        }


        internal void loadFile(String filename)
        {
            if (string.IsNullOrEmpty(luaFile))
                throw new Exception("No Lua file specified");

            if (!System.IO.File.Exists(filename))
                throw new Exception("Cannot find file " + filename);

            // store the file data
            this.theData = System.IO.File.ReadAllBytes(filename);

            interp = new Lua();
            this.usedSetData = false;

            #region register predefined functions & variables
            // register the functions
            interp.RegisterFunction("read", this, this.GetType().GetMethod("read"));
            interp.RegisterFunction("read2", this, this.GetType().GetMethod("read2"));
            interp.RegisterFunction("readWORD", this, this.GetType().GetMethod("readWORD"));
            interp.RegisterFunction("readlWORD", this, this.GetType().GetMethod("readlWORD"));
            interp.RegisterFunction("readDWORD", this, this.GetType().GetMethod("readDWORD"));
            interp.RegisterFunction("readlDWORD", this, this.GetType().GetMethod("readlDWORD"));
            interp.RegisterFunction("readString", this, this.GetType().GetMethod("readString"));
            interp.RegisterFunction("readString2", this, this.GetType().GetMethod("readString2"));
            interp.RegisterFunction("stringToInt", this, this.GetType().GetMethod("stringToInt"));
            interp.RegisterFunction("setData", this, this.GetType().GetMethod("setData"));
            interp.RegisterFunction("setData2", this, this.GetType().GetMethod("setData2"));
            interp.RegisterFunction("toHexadecimal", this, this.GetType().GetMethod("ToHexadecimal"));

            // register the default variables
            interp["filesize"] = theData.Length;
            interp["filename"] = filename.Substring(filename.LastIndexOfAny(new char[] { '\\', '/' }) + 1);
            interp["filepath"] = filename.Substring(0, filename.LastIndexOfAny(new char[] { '\\', '/' }) + 1);
            #endregion

            // read the plugin
            try
            {
                interp.DoFile(this.luaFile);

                // if the variable invalid has been set, display it, and reset the data
                if (interp["invalid"] != null)
                {
                    MainWindow.showError(interp.GetString("invalid"));
                    this.bData.Data = new byte[0];
                }
                else
                {
                    #region format output variable
                    // try to read the format
                    if (interp["format"] != null)
                    {
                        int format = (int)(double)interp["format"];
                        switch (this.Parent.BindingType)
                        {
                            case BindingType.GRAPHICS:
                                if (format < 1 || format > 7)
                                    MainWindow.showError("Plugin warning: the format of the graphics should range from 1 up to and including 7.\n"
                                                              + "Value " + format + " is ignored");
                                else
                                    GraphicsData.GraphFormat = (GraphicsFormat)format;
                                break;
                            case BindingType.PALETTE:
                                if (format < 5 || format > 7)
                                    MainWindow.showError("Plugin warning: the format of the palette should range from 5 up to and including 7.\n"
                                                              + "Value " + format + " is ignored");
                                else
                                    PaletteData.PalFormat = (PaletteFormat)format;
                                break;
                        }
                    }
                    #endregion

                    #region palette order
                    if (interp["order"] != null)
                        if (this.Parent.BindingType == BindingType.PALETTE || (int)GraphicsData.GraphFormat >= 5)
                        {
                            string s = ((string)interp["order"]).ToUpper();
                            if (s.Length != 3 || !s.Contains("R") || !s.Contains("G") || !s.Contains("B"))
                                MainWindow.showError("Plugin warning: the colour order is invalid.\n"
                                                     + "Value " + s + " is ignored.");
                            else
                                PaletteData.PalOrder = (PaletteOrder)Enum.Parse(typeof(PaletteOrder), s);
                        }
                    #endregion

                    #region Endianness
                    if (interp["bigendian"] != null)
                    {
                        bool isBE = (bool)interp["bigendian"];
                        switch (this.Parent.BindingType)
                        {
                            case BindingType.GRAPHICS:
                                GraphicsData.IsBigEndian = isBE; break;
                            case BindingType.PALETTE:
                                PaletteData.IsBigEndian = isBE; break;
                        }
                    }
                    #endregion

                    if (this.Parent.BindingType == BindingType.GRAPHICS)
                    {
                        #region width
                        if (interp["width"] != null)
                        {
                            uint origW = GraphicsData.Width;
                            try
                            {
                                uint w = (uint)(double)interp["width"];
                                GraphicsData.Width = w;
                            }
                            catch (Exception)
                            {
                                GraphicsData.Width = origW;
                                MainWindow.showError("Plugin warning: invalid width.\n"
                                                     + "Value " + interp.GetString("width") + " is ignored.");
                            }
                        }
                        #endregion

                        #region height
                        if (interp["height"] != null)
                        {
                            uint origH = GraphicsData.Height;
                            try
                            {
                                uint h = (uint)(double)interp["height"];
                                GraphicsData.Height = h;
                            }
                            catch (Exception)
                            {
                                GraphicsData.Height = origH;
                                MainWindow.showError("Plugin warning: invalid height.\n"
                                                     + "Value " + interp.GetString("height") + " is ignored.");
                            }
                        }
                        #endregion

                        #region tile size
                        if (interp["tilesize"] != null)
                        {
                            System.Drawing.Point pt = GraphicsData.TileSize;
                            try
                            {
                                System.Drawing.Point p = new System.Drawing.Point(pt.X, pt.Y);

                                LuaTable t = interp.GetTable("tilesize");

                                if (t["x"] != null)
                                    p.X = (int)(double)t["x"];
                                else if (t[0] != null)
                                    p.X = (int)(double)t[0];

                                if (t["y"] != null)
                                    p.Y = (int)(double)t["y"];
                                else if (t[1] != null)
                                    p.Y = (int)(double)t[1];

                                GraphicsData.TileSize = p;
                            }
                            catch (Exception)
                            {
                                MainWindow.showError("Plugin warning: invalid tile size provided.\nValue is ignored.");
                                GraphicsData.TileSize = pt;
                            }
                        }
                        #endregion

                        #region tiled
                        if (interp["tiled"] != null)
                        {
                            bool tl = GraphicsData.Tiled;
                            try
                            {
                                GraphicsData.Tiled = (bool)interp["tiled"];
                            }
                            catch (Exception)
                            {
                                MainWindow.showError("Plugin warning: invalid tile size provided.\nValue is ignored.");
                                GraphicsData.Tiled = tl;
                            }
                        }
                        #endregion
                    }

                    if (!usedSetData)
                        this.bData.Data = this.theData;

                    if (interp["warning"] != null)
                        MainWindow.ShowWarning(interp.GetString("warning"));
                }
            }
            catch (Exception e)
            {
                MainWindow.showError("Plugin error: \n" + e.Message);
            }

            // close and delete the interpreter, and delete the data
            interp.Close();
            interp = null;
            this.theData = null;
        }

        #region Predefined functions

        #region read(offset) : byte
        /// <summary>
        /// Reads a single byte from the input file
        /// </summary>
        /// <param name="offset">The offset at which to read a byte</param>
        /// <param name="b">The byte read</param>
        /// <exception cref="PluginException">When the offset is out of range.</exception>
        public void read(double offset, out byte b)
        {
            if ((int)offset > this.theData.Length)
                throw new PluginException("Plugin error; offset for read(offset) is out of bounds");
            else
                b = this.theData[(int)offset];
        }
        #endregion

        #region read2(offset, length)
        /// <summary>
        /// Reads an array of bytes from the input file
        /// </summary>
        /// <param name="offset">The start of the array</param>
        /// <param name="length">The maximum length of the array</param>
        /// <param name="tbl">The LuaTable containing the data from offset up to the end of the file or offset + length</param>
        public void read2(double offset, double length, out LuaTable tbl)
        {
            object o = interp["t"]; // temporarily store a possibly present variable t.
            
            byte b;
            interp.NewTable("t");
            for (int i = 0; i < (int)length && tryGetData(i+(int)offset, out b); i++)
                interp.DoString("t[" + i + "] = " + b);
            tbl = (LuaTable)interp["t"];

            interp["t"] = o; // replace our temporary table with the old value for t
        }
        // helper method for readArray; copy of version in BrowsableData
        private bool tryGetData(long offset, out byte b)
        {
            try { b = this.theData[offset]; return true; }
            catch (IndexOutOfRangeException) { b = 0; return false; }
        }
        #endregion

        #region readWORD(offset)
        /// <summary>
        /// Reads a WORD from the input file
        /// </summary>
        /// <param name="offset">The offset at which to read a WORD</param>
        /// <param name="w">The WORD read</param>
        /// <exception cref="PluginException">When the offset is out of range.</exception>
        public void readWORD(double offset, out int w)
        {
            byte b;
            w = 0;
            // abuse the read(offset) method
            read(offset, out b);
            w |= b;
            read(offset + 1, out b);
            w |= b << 8;
        }
        #endregion

        #region readlWORD(offset)
        /// <summary>
        /// Reads a LittleEndian WORD from the input file
        /// </summary>
        /// <param name="offset">The offset at which to read a LittleEndian WORD</param>
        /// <param name="w">The WORD read</param>
        /// <exception cref="PluginException">When the offset is out of range.</exception>
        public void readlWORD(double offset, out int w)
        {
            byte b;
            w = 0;
            // abuse the read(offset) method
            read(offset + 1, out b);
            w |= b;
            read(offset, out b);
            w |= b << 8;
        }
        #endregion

        #region readDWORD(offset)
        /// <summary>
        /// Reads a DWORD from the input file
        /// </summary>
        /// <param name="offset">The offset at which to read a DWORD</param>
        /// <param name="dw">The DWORD read</param>
        /// <exception cref="PluginException">When the offset is out of range.</exception>
        public void readDWORD(double offset, out int dw)
        {
            int i;
            dw = 0;
            // abuse the readWORD(offset) method
            readWORD(offset, out i);
            dw |= i;
            readWORD(offset + 2, out i);
            dw |= i << 16;
        }
        #endregion

        #region readlDWORD(offset)
        /// <summary>
        /// Reads a LittleEndian DWORD from the input file
        /// </summary>
        /// <param name="offset">The offset at which to read a LittleEndian DWORD</param>
        /// <param name="dw">The DWORD read</param>
        /// <exception cref="PluginException">When the offset is out of range.</exception>
        public void readlDWORD(double offset, out int dw)
        {
            int i;
            dw = 0;
            // abuse the readlWORD(offset) method
            readlWORD(offset + 2, out i);
            dw |= i;
            readlWORD(offset, out i);
            dw |= i << 16;
        }
        #endregion

        #region readString(offset)
        /// <summary>
        /// Reads a string from the file. Stops reading when the EOF has been reached, or a \0 has been read
        /// </summary>
        /// <param name="offset">where to start reading the string</param>
        /// <param name="str">The string starting at offset</param>
        public void readString(double offset, out string str)
        {
            str = "";
            byte b;
            while (tryGetData((long)(offset++), out b))
                if (b == 0)
                    break;
                else
                    str += (char)b;
        }
        #endregion

        #region readString2(offset, length)
        /// <summary>
        /// Reads a string from the file. Stops reading when the EOF has been reached, or a \0 has been read
        /// </summary>
        /// <param name="offset">where to start reading the string</param>
        /// <param name="maxlen">The maximum length of the string</param>
        /// <param name="str">The string starting at offset</param>
        public void readString2(double offset, double maxlen, out string str)
        {
            str = "";
            byte b;
            maxlen = Math.Round(maxlen);
            while (tryGetData((long)(offset++), out b) && maxlen-- > 0)
                if (b == 0)
                    break;
                else
                    str += (char)b;
        }
        #endregion

        #region stringToInt(string)
        /// <summary>
        /// Converts the first 4 characters of a string tino an integer, by reading them as bytes in a big-endian fashion
        /// </summary>
        /// <param name="str">The string to read</param>
        /// <param name="i">The first 4 characters of the string converted to a big-endian integer</param>
        public void stringToInt(string str, out int i)
        {
            i = 0;
            for (int p = 0; p < 4 && p < str.Length; p++)
                i |= (byte)str[p] << (p * 8);
        }
        #endregion

        #region ToHexadecimal
        /// <summary>
        /// Convert an integer into an hexadecimal string.
        /// </summary>
        /// <param name="i">The integer to convert</param>
        /// <param name="str">The hexadecimal representation of the given integer</param>
        public void ToHexadecimal(int i, out string str)
        {
            str = String.Format("{0:X}", i);
        }
        #endregion

        #region setData(offset)
        /// <summary>
        /// Sets the data of the underlying BrosableData
        /// </summary>
        /// <param name="offset">Where the actual data in the file begins</param>
        public void setData(double offset)
        {
            int o = (int)offset;
            if (o >= this.theData.Length)
                throw new PluginException("Cannot set the data to begin after the file");
            byte[] newdata = new byte[this.theData.Length - o];
            for (int i = 0; i < newdata.Length; i++)
                newdata[i] = this.theData[i + o];
            this.bData.Data = newdata;
            this.usedSetData = true;
        }
        #endregion

        #region setData2(offset, length)
        /// <summary>
        /// Sets the data of the underlying BrosableData
        /// </summary>
        /// <param name="offset">Where the actual data in the file begins</param>
        /// <param name="length">The (maximum) length of the data</param>
        public void setData2(double offset, double length)
        {
            int o = (int)offset;
            if (o >= this.theData.Length)
                throw new PluginException("Cannot set the data to begin after the file");
            byte[] newdata = new byte[(int)Math.Min(this.theData.Length - o, length)];

            for (int i = 0; i < newdata.Length; i++)
                newdata[i] = this.theData[i + o];

            this.bData.Data = newdata;

            if (newdata.Length < length)
                MainWindow.showError(String.Format("Plugin warning: no more data to read.\n"
                                                   + "Desired data length: 0x{0:x}\n"
                                                   + "Actual length: 0x{1:x}", 
                                                   (int)length, newdata.Length));

            this.usedSetData = true;
        }
        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.parentBinding = null;
        }

        #endregion
    }
}
