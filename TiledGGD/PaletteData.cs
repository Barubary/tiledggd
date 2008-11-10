using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TiledGGD
{
    class PaletteData : BrowseableData
    {
        #region Fields

        #region Field: Palette Format
        /// <summary>
        /// The format of the palette
        /// </summary>
        private static PaletteFormat palFormat;
        /// <summary>
        /// The format of the palette
        /// </summary>
        internal static PaletteFormat PalFormat
        {
            get { return palFormat; }
            set
            {
                if (palFormat != value)
                {
                    palFormat = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: Palette Order
        /// <summary>
        /// The palette order
        /// </summary>
        private static PaletteOrder palOrder;
        /// <summary>
        /// The palette order
        /// </summary>
        internal static PaletteOrder PalOrder
        {
            get { return palOrder; }
            set
            {
                if (palOrder != value)
                {
                    palOrder = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: Alpha Location
        /// <summary>
        /// The location of the Alpha value
        /// </summary>
        private static AlphaLocation alphaLocation;
        /// <summary>
        /// The location of the Alpha value
        /// </summary>
        internal static AlphaLocation alphaLoc
        {
            get { return alphaLocation; }
            set
            {
                if (alphaLocation != value)
                {
                    alphaLocation = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: IsBigEndian
        /// <summary>
        /// If the data is BigEndian. (if not, it's abviously LittleEndian)
        /// </summary>
        private static bool isBigEndian = true;
        /// <summary>
        /// If the data is BigEndian. (if not, it's abviously LittleEndian)
        /// </summary>
        internal static bool IsBigEndian
        {
            get { return isBigEndian; }
            set
            {
                if (isBigEndian != value)
                {
                    isBigEndian = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        /// <summary>
        /// The size of a colour in the display panel
        /// </summary>
        private Point palPixelSize = new Point(16, 16);

        #endregion

        #region Constructors

        internal PaletteData()
            : this(PaletteFormat.FORMAT_2BPP, PaletteOrder.ORDER_BGR)
        {
        }

        internal PaletteData(PaletteFormat pFormat, PaletteOrder pOrder)
        {
            palFormat = pFormat;
            palOrder = pOrder;
            if (palFormat == PaletteFormat.FORMAT_2BPP)
                alphaLocation = AlphaLocation.START;
            else if (palFormat == PaletteFormat.FORMAT_4BPP)
                alphaLocation = AlphaLocation.END;
            else
                alphaLocation = AlphaLocation.NONE;
        }

        #endregion

        internal override void load(string filename)
        {
            base.loadGenericData(filename);
        }

        internal override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for(int y=0; y<16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color c = Color.FromArgb((int)getPalette((uint)(y * 16 + x)));
                    Pen p = new Pen(c);
                    g.FillRectangle(p.Brush, x * palPixelSize.X, y * palPixelSize.Y, palPixelSize.X, palPixelSize.Y);
                }
        }

        internal override void copyToClipboard()
        {
            throw new Exception("The method PaletteData.CopyToClipboard is not yet implemented.");
        }

        #region Method: getPalette(int idx)
        /// <summary>
        /// Get the ARGB value of the idx'st palette
        /// </summary>
        /// <param name="idx">The index of the palette to get</param>
        /// <returns>The ARGB value of the palette</returns>
        internal uint getPalette(uint idx)
        {
            return getPalette(idx, this.Data, this.Offset, (int)palFormat, IsBigEndian);
        }

        /// <summary>
        /// Get the ARGB value of the idx'st palette or pixel
        /// </summary>
        /// <param name="idx">the index of the palette or pixel to get</param>
        /// <param name="data">the data to get the information from</param>
        /// <param name="offset">the offset in the data the index 0 is</param>
        /// <param name="format">the format of the colour. just cast PaletteFormat or GraphicsFormat to an int</param>
        /// <param name="bendian">If the data is BigEndian or not. (LittleEndian otherwise)</param>
        /// <returns>The ARGB value of the palette or pixel, or pure black if the index is our of range</returns>
        internal uint getPalette(uint idx, byte[] data, long offset, int format, bool bendian)
        {
            uint bt;
            uint b1, b2, b3, b4;
            uint fst, scn, thd;
            uint a, r, g, b;
            try
            {
                switch ((PaletteFormat)format)
                {
                    #region case 2BPP
                    case PaletteFormat.FORMAT_2BPP:
                        if (bendian)
                        {
                            b1 = (UInt32)data[offset + idx * 2 + 1];
                            b2 = (UInt32)data[offset + idx * 2];
                        }
                        else
                        {
                            b1 = (UInt32)data[offset + idx * 2];
                            b2 = (UInt32)data[offset + idx * 2 + 1];
                        }
                        bt = (b1 << 8) | b2;
                        // if alphalocation == none, assume alpha at start
                        // default: (a)bgr(a)
                        switch (alphaLocation)
                        {
                            case AlphaLocation.NONE:
                            case AlphaLocation.START:
                                a = (bt & 0x8000) >> 15;
                                fst = (bt & 0x7C00) >> 7;//>> 10) << 3;
                                scn = (bt & 0x03E0) >> 2;//>> 5) << 3;
                                thd = (bt & 0x001F) << 3;//>> 0) << 3;
                                break;
                            case AlphaLocation.END:
                                fst = (bt & 0xF800) >> 8;//>> 11) << 3;
                                scn = (bt & 0x07C0) >> 3;//>> 6) << 3;
                                thd = (bt & 0x003E) << 2;//>> 1) << 3;
                                a = (bt & 0x0001) >> 0;
                                break;
                            default: throw new Exception("Unknown Error: invalid alpha position " + alphaLocation.ToString());
                        }
                        a *= 0xFF;
                        break;
                    #endregion

                    #region case 3BPP
                    case PaletteFormat.FORMAT_3BPP:
                        a = 0xFF;
                        // default: bgr
                        if (bendian)
                        {
                            fst = b1 = (UInt32)data[offset + idx * 3];
                            scn = b2 = (UInt32)data[offset + idx * 3 + 1];
                            thd = b3 = (UInt32)data[offset + idx * 3 + 2];
                        }
                        else
                        {
                            fst = b3 = (UInt32)data[offset + idx * 3];
                            scn = b2 = (UInt32)data[offset + idx * 3 + 1];
                            thd = b1 = (UInt32)data[offset + idx * 3 + 2];
                        }
                        break;
                    #endregion

                    #region case 4BPP
                    case PaletteFormat.FORMAT_4BPP:
                        if (bendian)
                        {
                            b1 = (UInt32)data[offset + idx * 4];
                            b2 = (UInt32)data[offset + idx * 4 + 1];
                            b3 = (UInt32)data[offset + idx * 4 + 2];
                            b4 = (UInt32)data[offset + idx * 4 + 3];
                        }
                        else
                        {
                            b4 = (UInt32)data[offset + idx * 4];
                            b3 = (UInt32)data[offset + idx * 4 + 1];
                            b2 = (UInt32)data[offset + idx * 4 + 2];
                            b1 = (UInt32)data[offset + idx * 4 + 3];
                        }
                        // deafult: (a)bgr(a)
                        // assume argb if no a
                        switch (alphaLocation)
                        {
                            case AlphaLocation.NONE:
                            case AlphaLocation.START:
                                a = b1;
                                fst = b2;
                                scn = b3;
                                thd = b4;
                                break;
                            case AlphaLocation.END:
                                fst = b1;
                                scn = b2;
                                thd = b3;
                                a = b4;
                                break;
                            default: throw new Exception("Unknown Error: invalid alpha position " + alphaLocation.ToString());
                        }
                        break;
                    #endregion

                    default: throw new Exception("Unkown error: invalid palette Bpp " + palFormat.ToString());
                }
            }
            catch (IndexOutOfRangeException)
            {
                return 0xFF000000;
            }

            #region use palOrder
            switch (palOrder)
            {
                #region case BGR
                case PaletteOrder.ORDER_BGR:
                    b = fst;
                    g = scn;
                    r = thd;
                    break;
                #endregion

                #region case BRG
                case PaletteOrder.ORDER_BRG:
                    b = fst;
                    r = scn;
                    g = thd;
                    break;
                #endregion

                #region case GBR
                case PaletteOrder.ORDER_GBR:
                    g = fst;
                    b = scn;
                    r = thd;
                    break;
                #endregion

                #region case GRB
                case PaletteOrder.ORDER_GRB:
                    g = fst;
                    r = scn;
                    b = thd;
                    break;
                #endregion

                #region case RBG
                case PaletteOrder.ORDER_RBG:
                    r = fst;
                    b = scn;
                    g = thd;
                    break;
                #endregion

                #region case RGB
                case PaletteOrder.ORDER_RGB:
                    r = fst;
                    g = scn;
                    b = thd;
                    break;
                #endregion

                default: throw new Exception("Unknown error: invalid palOrder " + palOrder.ToString());
            }
            #endregion

            return (a << 24) | (r << 16) | (g << 8) | b;
        }
        #endregion

        internal int[] getFullPalette()
        {
            int[] fullpal = new int[256];
            long currIdx = Offset;
            int bt;
            byte fst, scn, thd, a;
            switch (PalFormat)
            {
                #region case 2BPP
                case PaletteFormat.FORMAT_2BPP:
                    for (int i = 0; i < 256; i++)
                    {
                        if (IsBigEndian)
                        {
                            try { fst = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { fst = 0; }
                            try { scn = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { scn = 0; }
                        }
                        else
                        {
                            try { scn = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { scn = 0; }
                            try { fst = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { fst = 0; }
                        }
                        bt = fst | (scn << 8);

                        switch (alphaLoc)
                        {
                            case AlphaLocation.NONE:
                            case AlphaLocation.START:
                                a = (byte)((bt >> 15) * 0xFF);
                                fst = (byte)((bt & 0x7C00) >> 10);
                                scn = (byte)((bt & 0x03E0) >> 5);
                                thd = (byte)((bt & 0x001F));
                                break;
                            case AlphaLocation.END:
                                a = (byte)((bt & 0x001) * 0xFF);
                                fst = (byte)((bt & 0xF800) >> 11);
                                scn = (byte)((bt & 0x07C0) >> 6);
                                thd = (byte)((bt & 0x003E) >> 1);
                                break;
                            default: throw new Exception("Unkown exception: invalid AlphaLocation " + alphaLoc.ToString());
                        }
                        parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[i]);
                    }
                    break;
                #endregion

                #region case 3BPP
                case PaletteFormat.FORMAT_3BPP:
                    for (int i = 0; i < 256; i++)
                    {
                        a = 0xFF;
                        if (IsBigEndian)
                        {
                            try { fst = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { fst = 0; }
                            try { scn = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { scn = 0; }
                            try { thd = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { thd = 0; }
                        }
                        else
                        {
                            try { thd = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { thd = 0; }
                            try { scn = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { scn = 0; }
                            try { fst = Data[currIdx++]; }
                            catch (IndexOutOfRangeException) { fst = 0; }
                        }
                        parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[i]);
                    }
                    break;
                #endregion

                #region case 4BPP
                case PaletteFormat.FORMAT_4BPP:
                    for (int i = 0; i < 256; i++)
                    {
                        try { thd = Data[currIdx++]; }
                        catch (IndexOutOfRangeException) { thd = 0; }
                        try { scn = Data[currIdx++]; }
                        catch (IndexOutOfRangeException) { scn = 0; }
                        try { fst = Data[currIdx++]; }
                        catch (IndexOutOfRangeException) { fst = 0; }
                        try { a = Data[currIdx++]; }
                        catch (IndexOutOfRangeException) { a = 0; }
                        if (IsBigEndian)
                        {
                            switch (alphaLoc)
                            {
                                case AlphaLocation.NONE:
                                case AlphaLocation.START:
                                    //thd = Data[currIdx++]; scn = Data[currIdx++]; fst = Data[currIdx++]; a = Data[currIdx++];
                                    parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[i]);
                                    break;
                                case AlphaLocation.END:
                                    //a = Data[currIdx++]; thd = Data[currIdx++]; scn = Data[currIdx++]; fst = Data[currIdx++];
                                    parsePalOrder(ref scn, ref thd, ref a, ref fst, out fullpal[i]);
                                    break;
                                default: throw new Exception("Unkown exception: invalid AlphaLocation " + alphaLoc.ToString());
                            }
                        }
                        else
                        {
                            switch (alphaLoc)
                            {
                                case AlphaLocation.NONE:
                                case AlphaLocation.START:
                                    //a = Data[currIdx++]; fst = Data[currIdx++]; scn = Data[currIdx++]; thd = Data[currIdx++];
                                    parsePalOrder(ref a, ref thd, ref scn, ref fst, out fullpal[i]);
                                    break;
                                case AlphaLocation.END:
                                    //fst = Data[currIdx++]; scn = Data[currIdx++]; thd = Data[currIdx++]; a = Data[currIdx++];
                                    parsePalOrder(ref fst, ref a, ref thd, ref scn, out fullpal[i]);
                                    break;
                                default: throw new Exception("Unkown exception: invalid AlphaLocation " + alphaLoc.ToString());
                            }
                        }
                        //parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[i]);
                    }
                    break;
                #endregion

                default: throw new Exception("Unkown exception: invalid PaletteFormat " + palFormat.ToString());
            }
            return fullpal;
        }

        internal Color[] getFullPaletteAsColor()
        {
            int[] intpal = getFullPalette();
            Color[] colpal = new Color[256];
            for (int i = 0; i < 256; i++)
                colpal[i] = Color.FromArgb(intpal[i]);
            return colpal;
        }

        internal void parsePalOrder(ref byte fst, ref byte scn, ref byte thd, ref byte a, out int argb)
        {
            switch (palOrder)
            {
                #region case BGR
                case PaletteOrder.ORDER_BGR:
                    //b = fst; g = scn; r = thd;
                    argb = (a << 24) | (thd << 16) | (scn << 8) | thd;
                    return;
                #endregion

                #region case BRG
                case PaletteOrder.ORDER_BRG:
                    //b = fst; r = scn; g = thd;
                    argb = (a << 24) | (scn << 16) | (thd << 8) | fst;
                    return;
                #endregion

                #region case GBR
                case PaletteOrder.ORDER_GBR:
                    //g = fst; b = scn; r = thd;
                    argb = (a << 24) | (thd << 16) | (fst << 8) | scn;
                    return;
                #endregion

                #region case GRB
                case PaletteOrder.ORDER_GRB:
                    //g = fst; r = scn; b = thd;
                    argb = (a << 24) | (scn << 16) | (fst << 8) | thd;
                    return;
                #endregion

                #region case RBG
                case PaletteOrder.ORDER_RBG:
                    //r = fst; b = scn; g = thd;
                    argb = (a << 24) | (fst << 16) | (thd << 8) | scn;
                    return;
                #endregion

                #region case RGB
                case PaletteOrder.ORDER_RGB:
                    //r = fst; g = scn; b = thd;
                    argb = (a << 24) | (fst << 16) | (scn << 8) | thd;
                    return;
                #endregion

                default: throw new Exception("Unknown error: invalid palOrder " + palOrder.ToString());
            }
        }

        #region Methods: TogglePaletteOrder()
        /// <summary>
        /// Toggle the palette order. Will automatically redraw the window. Use TogglePaletteOrder(false) to surpress the redrawing.
        /// </summary>
        internal void TogglePaletteOrder()
        {
            this.TogglePaletteOrder(true);
        }
        /// <summary>
        /// Toggle the palette order.
        /// </summary>
        /// <param name="doRepaint">If the window needs to be repainted</param>
        internal void TogglePaletteOrder(bool doRepaint)
        {
            int palord = (int)PalOrder;
            palord = (palord + 1) % 6;
            if(doRepaint)
                PalOrder = (PaletteOrder)palord;
            else
                palOrder = (PaletteOrder)palord;
        }
        #endregion
    }

    #region Palette enums (PaletteFormat, PaletteOrder, AlphaLocation
    public enum PaletteFormat : int
    {
        FORMAT_2BPP = 5,
        FORMAT_3BPP = 6,
        FORMAT_4BPP = 7
    }
    public enum PaletteOrder : int
    {
        ORDER_BGR = 0,
        ORDER_RGB = 1,
        ORDER_RBG = 2,
        ORDER_GRB = 3,
        ORDER_GBR = 4,
        ORDER_BRG = 5
    }
    public enum AlphaLocation : int
    {
        START,
        END,
        NONE
    }
    #endregion
}
