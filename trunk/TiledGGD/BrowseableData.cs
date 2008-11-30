using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

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
                long newoffset = Math.Max(0, Math.Min(this.data.Length - 1, value));
                if (newoffset != this.offset)
                {
                    this.offset = newoffset;
                    ptr = (byte*)data[offset];
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
        protected byte[] Data { get { return this.data; } set { this.data = value; } }
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
        /// The current byte
        /// </summary>
        protected byte Current { get { return *ptr; } }
        /// <summary>
        /// The next byte. Also increases the pointer
        /// </summary>
        protected byte Next(out bool end)
        {
            end = false;
            if (++ptroffset >= Length)
            {
                end = true;
                return 0;
            }
            ptr++;
            return *ptr;
        }
        /// <summary>
        /// Resets the pointer to the start of visible data
        /// </summary>
        protected void ResetPtr() { fixed (byte* ptr1 = &data[ptroffset = offset]) { ptr = ptr1; } }
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
            this.data = new byte[fstr.Length];
            for (long l = 0; l < fstr.Length; l++)
                data[l] = (byte)fstr.ReadByte();
            fstr.Close();
            Offset = 0;
            ResetPtr();
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
