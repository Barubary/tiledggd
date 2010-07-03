using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace TiledGGD
{
    class PaletteData : BrowsableData
    {
        #region Fields

        #region Field: Palette Format
        /// <summary>
        /// The format of the palette
        /// </summary>
        private static PaletteFormat palFormat = PaletteFormat.FORMAT_2BPP;
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
        private static PaletteOrder palOrder = PaletteOrder.BGR;
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

        #region Field: Alpha Setting
        /// <summary>
        /// The location of the Alpha value
        /// </summary>
        private static AlphaSettings alphaSettings = new AlphaSettings();
        /// <summary>
        /// The location of the Alpha value
        /// </summary>
        internal static AlphaSettings AlphaSettings
        {
            get { return alphaSettings; }
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


        #region Field: SkipSize
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        private static long skipSize = 1;
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        internal static long SkipSize { get { return skipSize; } set { skipSize = Math.Abs(value); } }
        #endregion

        #region Field: SkipMetric
        /// <summary>
        /// The metric used to skip data.
        /// </summary>
        private static PaletteSkipMetric skipMetric = PaletteSkipMetric.METRIC_COLOURS;
        internal static PaletteSkipMetric SkipMetric
        {
            get { return skipMetric; }
            set { skipMetric = value; }
        }
        #endregion

        /// <summary>
        /// The size of a colour in the display panel
        /// </summary>
        private Point palPixelSize = new Point(16, 16);

        private static string fname = "";
        /// <summary>
        /// The name of the loaded file
        /// </summary>
        public static string Filename { get { return fname; } }

        #region Field: Tiled
        /// <summary>
        /// If the palette is tiled or not
        /// </summary>
        private static bool tiled = false;
        /// <summary>
        /// If the palette is tiled or not
        /// </summary>
        public static bool Tiled
        {
            get { return tiled; }
            set
            {
                if (value != tiled)
                {
                    tiled = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: TileSize
        private static Point tilesize = new Point(8, 2);
        /// <summary>
        /// The size of a palette tile
        /// </summary>
        public static Point TileSize
        {
            get { return tilesize; }
            set
            {
                if (16 % value.X != 0 || 16 % value.Y != 0)
                    MainWindow.ShowError("16 must be a multiple of the width and height of a palette tile.");
                else
                    tilesize = value;
            }
        }
        #endregion

        #endregion

        #region Constructors

        internal PaletteData()
            : this(PaletteFormat.FORMAT_2BPP, PaletteOrder.BGR)
        {
        }

        internal PaletteData(PaletteFormat pFormat, PaletteOrder pOrder)
        {
            palFormat = pFormat;
            palOrder = pOrder;
        }

        #endregion

        #region Load methods
        internal override void load(string filename)
        {
            base.loadData(filename);

            if (filename.Contains("/"))
                fname = filename.Substring(filename.LastIndexOf("/")+1);
            else if (filename.Contains("\\"))
                fname = filename.Substring(filename.LastIndexOf("\\")+1);
            else
                fname = filename;
            filepath = filename;

            MainWindow.DoRefresh();
        }

        internal void reload(bool asSpecific)
        {
            if (asSpecific)
                load(filepath);
            else
            {
                loadGenericData(filepath);
                MainWindow.DoRefresh();
            }
        }

        #region NCLR
        public void loadFileAsNCLR(string filename)
        {
            #region typedef
            /*
		     * typedef struct {
		     *  char* magHeader; // 4 bytes: RLCN
		     *  DWORD magConst; // FF FE 00 01
		     *  DWORD fileSize;
		     *  WORD headerSize; // = 0x10
		     *  WORD nSections; // = 1
		     *  PLTTSection plttSection;
		     * } NCLRFile
		     * 
		     * typedef struct {
		     *  char* magHeader; // 4 bytes: TTLP
		     *  DWORD sectionSize;
		     *  DWORD paletteType; // could also be a WORD, but next 2 bytes seem to be always 0
		     *  DWORD palStart; // start offset of data after header? seems to be 0 always
		     *  DWORD palEnd; // end offset of data after header? seems to be sectionSize - 0x18 always
		     *  DWORD unkn1; // seems to be 0x10 always
		     *  WORD* palData; 
		     * } PLTTSection
		     * 
		     */
            #endregion
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
            br.ReadInt32(); // skip magic header; it should be LRCN

            if (br.ReadInt32() != 0x0100FEFF)
            {
                MainWindow.ShowError("Given file " + filename + " is not a valid NCLR file.\n It does not have the magic constant 0x0100FEFF at 0x04");
                br.Close();
                return;
            }
            int fileSize = br.ReadInt32();

            int headerSize = br.ReadInt16();
            int nSections = br.ReadInt16();

            // should now be at PLTT section
            if (br.ReadChar() != 'T' || br.ReadChar() != 'T' || br.ReadChar() != 'L' || br.ReadChar() != 'P')
            {
                MainWindow.ShowError("Given file " + filename + " is not a valid NCLR file or of an unsupported type.\n The PLTT section does not follow the NCLR header");
                br.Close();
                return;
            }

            int plttSize = br.ReadInt32();
            int pltype = br.ReadInt32();
            int palStart = br.ReadInt32();
            int palEnd = br.ReadInt32();
            int unkn = br.ReadInt32();
            int palLen = palEnd - palStart;
            if (palLen == 0)
                palLen = plttSize * 0x18; // tbh: I cannot remember why this is actually * 0x18
            this.Data = br.ReadBytes(Math.Min(palLen, plttSize - 0x18));

            switch (pltype)
            {
                case 3:
                case 4: PalFormat = PaletteFormat.FORMAT_2BPP; break;
                default: MainWindow.ShowError("Unknown palette format " + pltype); break;
            }

            br.Close();

        }
        #endregion

        #region GFNT
        public void loadFileAsGFNT(string filename)
        {
            #region typedef
            /*
             * typedef struct {
             *   char* magHeader; // GFNT
             *   DWORD fileSize; // best to ignore; is sometimes size up to first palette
             *   char* version; // '1.02', '1.01' or '1.00' best to ignore
             *   DWORD Imagebpp;
             *   DWORD imageWidthSc; // for real width; 8 << val
             *   DWORD imageHeightSc; // for real height; 8 << val
             *   DWORD nPals;
             *   byte* image;
             *   byte* palette; // seems to be 2Bppal always.
             * } GFNTFile
             */
            #endregion

            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
            br.ReadInt32(); // we should already know the magic header is GFNT
            br.ReadInt32(); // we don't need to know the filesize, and it's not even correct for multipalettes

            br.ReadInt32(); // ignore version-string

            int imbpp = br.ReadInt32();
            int imwidth = 8 << br.ReadInt32();
            int imheight = 8 << br.ReadInt32();
            int nPals = br.ReadInt32();

            int nBytesForIm = imwidth * imheight;
            int nBytesForPal = 2;

            switch (imbpp)
            {
                case 1: nBytesForIm /= 8; nBytesForPal *= 2; break;
                case 2: nBytesForIm /= 4; nBytesForPal *= 4; break;
                case 3: nBytesForIm /= 2; nBytesForPal *= 16; break;
                case 4: nBytesForIm *= 1; nBytesForPal *= 256; break;
                case 5: nBytesForIm *= 2; nBytesForPal = 0; break;
                case 6: nBytesForIm *= 1; imbpp = 4; nBytesForPal *= 256; break;
                case 7: nBytesForIm *= 4; nBytesForPal = 0; break;
                default: MainWindow.ShowError("Possibly invalid GFNT file: unknown GraphicsFormat " + imbpp); br.Close(); return;
            }

            br.BaseStream.Seek(nBytesForIm, SeekOrigin.Current);
            byte[] dt = br.ReadBytes(nBytesForPal);
            this.Data = dt;
            PalFormat = PaletteFormat.FORMAT_2BPP;
            alphaSettings.Location = AlphaLocation.START;
            alphaSettings.IgnoreAlpha = true;
            PalOrder = PaletteOrder.BGR;

            br.Close();
        }
        #endregion

        #endregion

        internal override void paint(object sender, PaintEventArgs e)
        {
            if (!HasData)
                return;

            Graphics g = e.Graphics;
            g.DrawImage(toBitmap(16), 0, 0);
        }

        internal override void copyToClipboard()
        {
            Clipboard.SetImage(toBitmap());
        }

        internal override Bitmap toBitmap()
        {
            return toBitmap(8);
        }

        internal unsafe Bitmap toBitmap(int pixw)
        {
            int[] pal = this.getFullPalette();

            Bitmap bmp = new Bitmap(16 * pixw, 16 * pixw);
            Bitmap unscaled = new Bitmap(16, 16);
            BitmapData bmd = unscaled.LockBits(new Rectangle(0, 0, 16, 16), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            for (int i = 0; i < 16 * 16; i++)
                *(bmptr++) = pal[i];

            unscaled.UnlockBits(bmd);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                g.DrawImage(unscaled, new Rectangle(0, 0, 16 * pixw, 16 * pixw), new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
            }
            return bmp;
        }

        #region Method: getPalette
        /// <summary>
        /// Get the ARGB value of the palette or pixel
        /// </summary>
        /// <param name="data">the data to get the information from</param>
        /// <param name="format">the format of the colour. just cast PaletteFormat or GraphicsFormat to an int</param>
        /// <param name="bendian">If the data is BigEndian or not. (LittleEndian otherwise)</param>
        /// <returns>The ARGB value of the palette or pixel, or pure black if the index is our of range</returns>
        internal uint getPalette(byte[] data, int format, bool bendian)
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
                            b1 = (uint)data[1];
                            b2 = (uint)data[0];
                        }
                        else
                        {
                            b1 = (uint)data[0];
                            b2 = (uint)data[1];
                        }
                        bt = (b1 << 8) | b2;
                        // if alphalocation == none, assume alpha at start
                        // default: (a)bgr(a)
                        switch (AlphaSettings.Location)
                        {
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
                            default: throw new Exception("Unknown Error: invalid alpha position " + AlphaSettings.Location.ToString());
                        }
                        if (AlphaSettings.IgnoreAlpha)
                            a = 1;
                        a *= 0xFF;
                        break;
                    #endregion

                    #region case 3BPP
                    case PaletteFormat.FORMAT_3BPP:
                        a = 0xFF;
                        // default: bgr
                        if (bendian)
                        {
                            fst = b1 = (uint)data[0];
                            scn = b2 = (uint)data[1];
                            thd = b3 = (uint)data[2];
                        }
                        else
                        {
                            fst = b3 = (uint)data[0];
                            scn = b2 = (uint)data[1];
                            thd = b1 = (uint)data[2];
                        }
                        break;
                    #endregion

                    #region case 4BPP
                    case PaletteFormat.FORMAT_4BPP:
                        if (!bendian)
                        {
                            b1 = (UInt32)data[0];
                            b2 = (UInt32)data[1];
                            b3 = (UInt32)data[2];
                            b4 = (UInt32)data[3];
                        }
                        else
                        {
                            b4 = (UInt32)data[0];
                            b3 = (UInt32)data[1];
                            b2 = (UInt32)data[2];
                            b1 = (UInt32)data[3];
                        }
                        // deafult: (a)bgr(a)
                        // assume argb with a = 0xFF always if no a
                        switch (AlphaSettings.Location)
                        {
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
                            default: throw new Exception("Unknown Error: invalid alpha position " + AlphaSettings.Location.ToString());
                        }
                        if (AlphaSettings.IgnoreAlpha)
                            a = 0xFF;
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
                case PaletteOrder.BGR:
                    b = fst;
                    g = scn;
                    r = thd;
                    break;
                #endregion

                #region case BRG
                case PaletteOrder.BRG:
                    b = fst;
                    r = scn;
                    g = thd;
                    break;
                #endregion

                #region case GBR
                case PaletteOrder.GBR:
                    g = fst;
                    b = scn;
                    r = thd;
                    break;
                #endregion

                #region case GRB
                case PaletteOrder.GRB:
                    g = fst;
                    r = scn;
                    b = thd;
                    break;
                #endregion

                #region case RBG
                case PaletteOrder.RBG:
                    r = fst;
                    b = scn;
                    g = thd;
                    break;
                #endregion

                #region case RGB
                case PaletteOrder.RGB:
                    r = fst;
                    g = scn;
                    b = thd;
                    break;
                #endregion

                default: throw new Exception("Unknown error: invalid palOrder " + palOrder.ToString());
            }
            #endregion

            int[] col = new int[]{(int)((a << 24) | (r << 16) | (g << 8) | b)};
            apply_alphasettings(col, 1);
            return (uint)col[0];
        }
        #endregion

        #region Methods: getFullPalette
        internal int[] getFullPalette()
        {
            ResetPtr();
            int[] fullpal = new int[256];
            if (!this.HasData)
                return fullpal;

            int bt;
            byte fst, scn, thd, fth, a;
            bool atEnd = false;
            int palNo;
            switch (PalFormat)
            {
                #region case 2BPP
                case PaletteFormat.FORMAT_2BPP:
                    for (palNo = 0; palNo < 256 && !atEnd; palNo++)
                    {
                        if (IsBigEndian)
                        {
                            fst = Next(out atEnd);
                            if (atEnd) break;
                            scn = Next(out atEnd);
                        }
                        else
                        {
                            scn = Next(out atEnd);
                            if (atEnd) break;
                            fst = Next(out atEnd);
                        }
                        bt = fst | (scn << 8);

                        switch (AlphaSettings.Location)
                        {
                            case AlphaLocation.START:
                                a = (byte)((bt & 0x8000) >> 15);
                                fst = (byte)((bt & 0x7C00) >> 7);//>> 10) << 3;
                                scn = (byte)((bt & 0x03E0) >> 2);//>> 5) << 3;
                                thd = (byte)((bt & 0x001F) << 3);//>> 0) << 3;
                                break;
                            case AlphaLocation.END:
                                fst = (byte)((bt & 0xF800) >> 8);//>> 11) << 3;
                                scn = (byte)((bt & 0x07C0) >> 3);//>> 6) << 3;
                                thd = (byte)((bt & 0x003E) << 2);//>> 1) << 3;
                                a = (byte)((bt & 0x0001) >> 0);
                                break;
                            default: throw new Exception("Unkown exception: invalid AlphaLocation " + AlphaSettings.Location.ToString());
                        }
                        a *= 0xFF;
                        parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[palNo]);
                    }
                    break;
                #endregion

                #region case 3BPP
                case PaletteFormat.FORMAT_3BPP:
                    for (palNo = 0; palNo < 256 && !atEnd; palNo++)
                    {
                        a = 0xFF;
                        if (IsBigEndian)
                        {
                            fst = Next(out atEnd);
                            if (atEnd) break;
                            scn = Next(out atEnd);
                            thd = Next(out atEnd);
                        }
                        else
                        {
                            thd = Next(out atEnd);
                            if (atEnd) break;
                            scn = Next(out atEnd);
                            fst = Next(out atEnd);
                        }
                        parsePalOrder(ref fst, ref scn, ref thd, ref a, out fullpal[palNo]);
                    }
                    break;
                #endregion

                #region case 4BPP
                case PaletteFormat.FORMAT_4BPP:
                    for (palNo = 0; palNo < 256 && !atEnd; palNo++)
                    {
                        if (isBigEndian)
                        {
                            fth = Next(out atEnd);
                            if (atEnd) break;
                            thd = Next(out atEnd);
                            scn = Next(out atEnd);
                            fst = Next(out atEnd);
                        }
                        else
                        {
                            fst = Next(out atEnd);
                            if (atEnd) break;
                            scn = Next(out atEnd);
                            thd = Next(out atEnd);
                            fth = Next(out atEnd);
                        }
                        switch (AlphaSettings.Location)
                        {
                            case AlphaLocation.START:
                                parsePalOrder(ref scn, ref thd, ref fth, ref fst, out fullpal[palNo]);
                                break;
                            case AlphaLocation.END:
                                parsePalOrder(ref fst, ref scn, ref thd, ref fth, out fullpal[palNo]);
                                break;
                            default: throw new Exception("Unkown exception: invalid AlphaLocation " + AlphaSettings.Location.ToString());
                        }
                    }
                    break;
                #endregion

                default: throw new Exception("Unkown exception: invalid PaletteFormat " + palFormat.ToString());
            }
            apply_alphasettings(fullpal, palNo);
            return Tiled ? tile(fullpal) : fullpal;
        }

        internal void apply_alphasettings(int[] pal, int palCount)
        {
            float rangesize = AlphaSettings.Maximum-AlphaSettings.Minimum;
            for (int p = 0; p < palCount; p++)
            {
                if (AlphaSettings.IgnoreAlpha)
                {
                    pal[p] |= 0xFF << 24;
                    continue;
                }
                if (AlphaSettings.Stretch)
                {
                    byte alpha = (byte)(pal[p] >> 24);
                    if (alpha >= AlphaSettings.Maximum)
                        alpha = 0xFF;
                    else if (alpha <= AlphaSettings.Minimum)
                        alpha = 0;
                    else
                        alpha = (byte)(0xFF * ((float)(alpha - AlphaSettings.Minimum) / rangesize));
                    pal[p] = (pal[p] & 0xFFFFFF) | (alpha << 24);
                }
            }
        }

        internal int[] tile(int[] pal)
        {
            int[] outpal = new int[256];
            int ntx = 16 / tilesize.X,
                nty = 16 / tilesize.Y;
            int i=0;
            for (int ty = 0; ty < nty; ty++)
                for (int tx = 0; tx < ntx; tx++)
                    for (int y = 0; y < tilesize.Y; y++)
                        for (int x = 0; x < tilesize.X; x++)
                            outpal[(ty * tilesize.Y + y) * 16 + (tx * tilesize.X + x)] = pal[i++];

            return outpal;
        }

        internal Color[] getFullPaletteAsColor()
        {
            int[] intpal = getFullPalette();
            Color[] colpal = new Color[256];
            for (int i = 0; i < 256; i++)
                colpal[i] = Color.FromArgb(intpal[i]);
            return colpal;
        }
        #endregion

        #region method: parsePalOrder()
        internal void parsePalOrder(ref byte fst, ref byte scn, ref byte thd, ref byte a, out int argb)
        {
            switch (palOrder)
            {
                #region case BGR
                case PaletteOrder.BGR:
                    //bitmap = fst; g = scn; r = thd;
                    argb = (a << 24) | (thd << 16) | (scn << 8) | fst;
                    return;
                #endregion

                #region case BRG
                case PaletteOrder.BRG:
                    //bitmap = fst; r = scn; g = thd;
                    argb = (a << 24) | (scn << 16) | (thd << 8) | fst;
                    return;
                #endregion

                #region case GBR
                case PaletteOrder.GBR:
                    //g = fst; bitmap = scn; r = thd;
                    argb = (a << 24) | (thd << 16) | (fst << 8) | scn;
                    return;
                #endregion

                #region case GRB
                case PaletteOrder.GRB:
                    //g = fst; r = scn; bitmap = thd;
                    argb = (a << 24) | (scn << 16) | (fst << 8) | thd;
                    return;
                #endregion

                #region case RBG
                case PaletteOrder.RBG:
                    //r = fst; bitmap = scn; g = thd;
                    argb = (a << 24) | (fst << 16) | (thd << 8) | scn;
                    return;
                #endregion

                #region case RGB
                case PaletteOrder.RGB:
                    //r = fst; g = scn; bitmap = thd;
                    argb = (a << 24) | (fst << 16) | (scn << 8) | thd;
                    return;
                #endregion

                default: throw new Exception("Unknown error: invalid palOrder " + palOrder.ToString());
            }
        }
        #endregion

        #region Toggle Methods
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
        internal void toggleSkipSize()
        {
            switch (SkipMetric)
            {
                case PaletteSkipMetric.METRIC_BYTES:
                    switch (SkipSize)
                    {
                        case 1: SkipMetric = PaletteSkipMetric.METRIC_COLOURS; break;
                        case 0x10000: SkipSize = 1; break;
                    } break;
                case PaletteSkipMetric.METRIC_COLOURS:
                    switch (SkipSize)
                    {
                        case 1: SkipSize = 16; break;
                        case 16: SkipSize = 256; break;
                        case 256: SkipMetric = PaletteSkipMetric.METRIC_BYTES; SkipSize = 0x10000; break;
                    } break;
            }
        }
        internal void toggleTiled()
        {
            Tiled = !Tiled;
        }
        internal void toggleEndianness()
        {
            IsBigEndian = !IsBigEndian;
        }
        internal void toggleFormat()
        {
            PalFormat = (PaletteFormat)(((int)PalFormat - 4) % 3 + 5);
        }
        #endregion

        internal override void DoSkip(bool positive)
        {
            switch (SkipMetric)
            {
                case PaletteSkipMetric.METRIC_BYTES:
                    DoSkip(positive, SkipSize); break;
                case PaletteSkipMetric.METRIC_COLOURS:
                    switch (PalFormat)
                    {
                        case PaletteFormat.FORMAT_2BPP: DoSkip(positive, SkipSize * 2); break;
                        case PaletteFormat.FORMAT_3BPP: DoSkip(positive, SkipSize * 3); break;
                        case PaletteFormat.FORMAT_4BPP: DoSkip(positive, SkipSize * 4); break;
                    } break;
            }
        }
    }

    #region class: AlphaSettings
    public class AlphaSettings
    {
        #region Fields & Properties

        #region Location
        /// <summary>
        /// The location of the alpha value
        /// </summary>
        private AlphaLocation location;
        /// <summary>
        /// The location of the alpha value
        /// </summary>
        public AlphaLocation Location
        {
            get { return this.location; }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region IgnoreAlpha
        /// <summary>
        /// If the alpha value should be ignored
        /// </summary>
        private bool ignoreAlpha;
        /// <summary>
        /// If the alpha value should be ignored
        /// </summary>
        public bool IgnoreAlpha
        {
            get { return this.ignoreAlpha; }
            set
            {
                if (this.ignoreAlpha != value)
                {
                    this.ignoreAlpha = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Stretch
        /// <summary>
        /// If the alpha value should be stretched
        /// </summary>
        private bool stretch;
        /// <summary>
        /// If the alpha value should be stretched
        /// </summary>
        public bool Stretch
        {
            get { return this.stretch; }
            set
            {
                if (this.stretch != value)
                {
                    this.stretch = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Minimum
        /// <summary>
        /// Which value of alpha should be mapped to 0
        /// </summary>
        private byte min;
        /// <summary>
        /// Which value of alpha should be mapped to 0
        /// </summary>
        public byte Minimum
        {
            get { return this.min; }
            set
            {
                if (value > max)
                    MainWindow.ShowError("Alpha minimum cannot be larger than alpha maximum");
                else if (min != value)
                {
                    min = value;
                    if (Stretch)
                        MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Maximum
        /// <summary>
        /// Which value of alpha should be mapped to 255
        /// </summary>
        private byte max;
        /// <summary>
        /// Which value of alpha should be mapped to 255
        /// </summary>
        public byte Maximum
        {
            get { return this.max; }
            set
            {
                if (value < min)
                    MainWindow.ShowError("Alpha maximum cannot be smaller than alpha minimum");
                else if (max != value)
                {
                    max = value;
                    if (Stretch)
                        MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #endregion

        public AlphaSettings() : this(AlphaLocation.START, true, false, 0, 0xFF) { }
        public AlphaSettings(AlphaLocation loc, bool ignore, bool scale, byte min, byte max)
        {
            this.Maximum = max;
            this.Minimum = min;
            this.IgnoreAlpha = ignore;
            this.Stretch = scale;
            this.Location = loc;
        }

    }
    #endregion

    #region Palette enums (PaletteFormat, PaletteOrder, AlphaLocation, PaletteSkipMetric
    public enum PaletteFormat : int
    {
        FORMAT_2BPP = 5,
        FORMAT_3BPP = 6,
        FORMAT_4BPP = 7
    }
    public enum PaletteOrder : int
    {
        BGR = 0,
        RGB = 1,
        RBG = 2,
        GRB = 3,
        GBR = 4,
        BRG = 5
    }
    public enum AlphaLocation : int
    {
        START = 0,
        END = 1
    }
    public enum PaletteSkipMetric
    {
        METRIC_BYTES,
        METRIC_COLOURS
    }
    #endregion
}
