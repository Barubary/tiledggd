using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace TiledGGD
{
    abstract unsafe class BrowseableData
    {
        #region Fields

        #region Field: Offset
        /// <summary>
        /// The current offset of the data
        /// </summary>
        private long offset = 0;
        /// <summary>
        /// The current offset of the data
        /// </summary>
        public long Offset
        {
            get { return this.offset; }
            internal set
            {
                if (!HasData)
                    return;
                long newoffset = Math.Max(0, Math.Min(this.data.Length, value));
                if (newoffset != this.offset)
                {
                    this.offset = newoffset;
                    if (offset == 0)
                        ptr = (byte*)data[offset];
                    else
                    {
                        ptr = (byte*)data[offset - 1];
                        ptr++;
                    }
                    MainWindow.DoRefresh();
                }

            }
        }
        #endregion

        #region Field: Data
        /// <summary>
        /// The actual data. Will only contiain bytes.
        /// </summary>
        private byte[] data;
        /// <summary>
        /// The actual data. Should only be set within the load() method.
        /// </summary>
        protected byte[] Data { get { return this.data; } set { this.data = value; offset = 0; ResetPtr(); } }
        /// <summary>
        /// Get a byte of data
        /// </summary>
        /// <param name="idx">The index of the byte</param>
        /// <returns>The byte at index idx, or 0 if it's out of range</returns>
        protected byte getData(long idx, out bool end) {
            try { end = false; return this.data[idx]; }
            catch (IndexOutOfRangeException) { end = true; return 0; }
        }
        #endregion

        #region field: Current. Methods: Next, ResetPtr
        /// <summary>
        /// The pointer to a byte in Data
        /// </summary>
        private byte* ptr;
        /// <summary>
        /// the current offset of the pointer
        /// </summary>
        private long ptroffset;
        /// <summary>
        /// Peek the next byte. (look at next byte, don't move pointer)
        /// </summary>
        protected byte PeekNext
        {
            get
            {
                if (ptroffset >= Length)
                    return 0;
                return *ptr;
            }
        }
        /// <summary>
        /// The next byte. Also increases the pointer
        /// </summary>
        protected byte Next(out bool end)
        {
            end = false;
            if (++ptroffset > Length)
            {
                end = true;
                return 0;
            }
            return *ptr++;
        }
        /// <summary>
        /// Resets the pointer to the start of visible data
        /// </summary>
        protected void ResetPtr() {
            if (data == null)
                return;
            if (offset == 0)
                fixed (byte* ptr1 = &data[ptroffset = offset]) { ptr = ptr1; }
            else
                fixed (byte* ptr1 = &data[ptroffset = offset - 1]) { ptr = ptr1; ptr++; }
        }
        #endregion

        /// <summary>
        /// If this BrowseableData has data
        /// </summary>
        internal bool HasData { get { return this.data != null; } }

        #region Field: Length
        /// <summary>
        /// The LongLength of the data
        /// </summary>
        public long Length
        {
            get { try { return this.data.LongLength; } catch (Exception) { return 0; } }
        }
        #endregion

        protected string filepath = "";

        #endregion

        /// <summary>
        /// Load a file, and interpret it as BrowseableData. (either graphics or palette)
        /// </summary>
        /// <param name="filename">The name of the file to load</param>
        internal abstract void load(string filename);

        /// <summary>
        /// Skip SkipSize data
        /// </summary>
        /// <param name="positive">If the skip is to be in the positive direction.</param>
        abstract internal void DoSkip(bool positive);

        protected void DoSkip(bool positive, long bytes)
        {
            if (positive)
                Offset += bytes;
            else
                Offset -= bytes;
        }

        /// <summary>
        /// Loads the data of a file blindly; all bytes are copied.
        /// </summary>
        /// <param name="filename">The name of the file to load</param>
        protected void loadGenericData(String filename)
        {
            FileStream fstr = new FileStream(filename, FileMode.Open);
            if(fstr.Length > int.MaxValue){
                MessageBox.Show("Unable to load files >= 2 GB");
                fstr.Close();
                return;
            }
            this.data = new byte[fstr.Length];
            fstr.Read(this.data, 0, data.Length);
            fstr.Close();
            Offset = 0;
            ResetPtr();
        }

        protected void loadData(String filename)
        {
            // read the first 4 bytes, check if the Data has the method to parse it
            FileStream fstr = new FileStream(filename, FileMode.Open);
            string magHdr = "";
            for (int i = 0; i < 4; i++)
            {
                char ch = (char)fstr.ReadByte();
                if (char.IsLetterOrDigit(ch))
                    magHdr += ch;
                else
                    break;
            }
            fstr.Flush();
            fstr.Close();
            fstr.Dispose();
            if (magHdr.Length == 0)
                loadGenericData(filename);
            else
            {
                /*Type t = this is PaletteData ? (this as PaletteData).GetType() : (this is GraphicsData ? (this as GraphicsData).GetType() : this.GetType());
                MethodInfo[] methods = t.GetMethods();
                magHdr = magHdr.ToUpper();
                foreach (MethodInfo meth in methods)
                {
                    if (!meth.Name.StartsWith("loadFileAs"))
                        continue;
                    string tmp = meth.Name.Substring(10);
                    if (magHdr.Length < tmp.Length)
                        continue;
                    if (magHdr.Length > tmp.Length)
                        magHdr = magHdr.Substring(0, tmp.Length);
                    if (magHdr.Equals(tmp) || magHdr.Equals(reverseString(tmp)))
                    {
                        try
                        {
                            meth.Invoke(this, new object[] { filename });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        return;
                    }
                }*/
                switch (magHdr)
                {
                    case "NCGR":
                    case "RGCN":
                        if (this is GraphicsData)
                            (this as GraphicsData).loadFileAsNCGR(filename);
                        else
                            this.loadGenericData(filename);
                        return;
                    case "NCLR":
                    case "RLCN":
                        if (this is GraphicsData)
                            this.loadGenericData(filename);
                        else
                            (this as PaletteData).loadFileAsNCLR(filename);
                        return;
                    default: loadGenericData(filename); break;
                }
            }
        }

        private string reverseString(string instr)
        {
            string outstr = "";
            foreach(char c in instr)
                outstr = c + outstr;
            return outstr;
        }


        /// <summary>
        /// Paint the data in some way
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal abstract void paint(object sender, PaintEventArgs e);

        /// <summary>
        /// Copy the currently shown data onto the clipboard
        /// </summary>
        internal abstract void copyToClipboard();

        /// <summary>
        /// Save the currently shown data in a Bitmap
        /// </summary>
        internal abstract Bitmap toBitmap();
    }
}
