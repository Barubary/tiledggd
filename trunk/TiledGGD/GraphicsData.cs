using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;

namespace TiledGGD
{
    class GraphicsData : BrowseableData
    {
        #region Fields

        #region Field: Width
        /// <summary>
        /// The width of the shown data.
        /// </summary>
        private static uint width = 64;
        /// <summary>
        /// Get or set the width of the shown data. Will automatically redraw the screen.
        /// </summary>
        internal static uint Width
        {
            get { return width; }
            set
            {
                uint newW = value;
                if (Tiled && newW % TileSize.X != 0)
                    newW += (uint)TileSize.X - newW % (uint)TileSize.X;
                if (newW < WidthSkipSizeUInt)
                    newW = WidthSkipSizeUInt;
                if (width != newW)
                {
                    width = newW;
                    MainWindow.DoRefresh();
                }
            }
        }

        /// <summary>
        /// How many pixels the width should change with the press of a button
        /// </summary>
        private static HWSkipSize widthSkipSize = HWSkipSize.SKIPSIZE_4PIX;
        /// <summary>
        /// How many pixels the width should change with the press of a button
        /// </summary>
        internal static HWSkipSize WidthSkipSize { get { return widthSkipSize; } set { widthSkipSize = value; } }
        internal static uint WidthSkipSizeUInt { get { return WidthSkipSize == HWSkipSize.SKIPSIZE_1TILE ? (uint)TileSize.X : (uint)(Math.Pow(2, (int)WidthSkipSize)); } }

        #endregion

        #region Field: Height
        /// <summary>
        /// The height of the shown data
        /// </summary>
        private static uint height = 128;
        /// <summary>
        /// The height of the shown data
        /// </summary>
        internal static uint Height
        {
            get { return height; }
            set
            {
                uint newH = value;
                if (Tiled && newH % TileSize.Y != 0)
                    newH += (uint)TileSize.Y - newH % (uint)TileSize.Y;
                if (newH < HeightSkipSizeUInt)
                    newH = HeightSkipSizeUInt;
                if (height != newH)
                {
                    height = newH;
                    MainWindow.DoRefresh();
                }
            }
        }

        /// <summary>
        /// How many pixels the height should change with the press of a button
        /// </summary>
        private static HWSkipSize heightSkipSize = HWSkipSize.SKIPSIZE_8PIX;
        /// <summary>
        /// How many pixels the height should change with the press of a button
        /// </summary>
        internal static HWSkipSize HeightSkipSize { get { return heightSkipSize; } set { heightSkipSize = value; } }
        internal static uint HeightSkipSizeUInt { get { return (uint)(HeightSkipSize == HWSkipSize.SKIPSIZE_1TILE ? TileSize.Y : Math.Pow(2, (int)HeightSkipSize)); } }
        #endregion

        #region Field: TileSize
        /// <summary>
        /// The size of a tile
        /// </summary>
        private static Point tileSize = new Point(8, 8);
        /// <summary>
        /// Get or set the size of a tile. Negative values will be made positive.
        /// </summary>
        public static Point TileSize
        {
            get { return tileSize; }
            set
            {
                tileSize = value;
                tileSize.X = Math.Abs(tileSize.X);
                tileSize.Y = Math.Abs(tileSize.Y);
                Height = Height;
                Width = Width;
                if (Tiled)
                    MainWindow.DoRefresh();
            }
        }
        #endregion

        #region Field: Tiled
        /// <summary>
        /// If the data is tiled or not
        /// </summary>
        private static bool tiled = false;
        /// <summary>
        /// Get or set if the data is tiled or not
        /// </summary>
        internal static bool Tiled
        {
            get { return tiled; }
            set
            {
                if (value != tiled)
                {
                    tiled = value;
                    if (tiled)
                    {
                        // make sure the width & height are correct with the current tilesize
                        Width = Width;
                        Height = Height;
                        // make sure the w/h skip sizes aren't too small
                        if (WidthSkipSizeUInt < TileSize.X)
                            WidthSkipSize = HWSkipSize.SKIPSIZE_1TILE;
                        if (HeightSkipSizeUInt < TileSize.Y)
                            HeightSkipSize = HWSkipSize.SKIPSIZE_1TILE;
                    }
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: Zoom
        /// <summary>
        /// The zoom of the graphics data
        /// </summary>
        private static int zoom = 1;
        /// <summary>
        /// The zoom of the graphics data
        /// </summary>
        internal static int Zoom
        {
            get { return zoom; }
            set
            {
                zoom = Math.Max(1, Math.Min(8, value));
                MainWindow.DoRefresh();
            }
        }
        #endregion

        #region Field: GraphicsFormat
        /// <summary>
        /// The format of the Graphics window
        /// </summary>
        private static GraphicsFormat graphFormat = GraphicsFormat.FORMAT_4BPP;
        /// <summary>
        /// Get or Set the format of the Graphics Window. Will automatically redraw when altered.
        /// </summary>
        internal static GraphicsFormat GraphFormat
        {
            get { return graphFormat; }
            set {
                if (graphFormat != value)
                {
                    graphFormat = value;
                    MainWindow.DoRefresh();
                }
            }
        }
        #endregion

        #region Field: IsBigEndian
        /// <summary>
        /// If the graphics data is BigEndian (or LittleEndian otherwise).
        /// </summary>
        private static bool isBigEndian = true;
        /// <summary>
        /// If the graphics data is BigEndian (or LittleEndian otherwise). Will automatically repaint when changed.
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

        #region Field: PaletteData
        /// <summary>
        /// The palette data
        /// </summary>
        private PaletteData paletteData;
        #endregion


        #region Field: SkipSize
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        private static GraphicsSkipSize skipSize = GraphicsSkipSize.SKIPSIZE_1BYTE;
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        internal static GraphicsSkipSize SkipSize { get { return skipSize; } set { skipSize = value; } }
        #endregion

        private static string fname = "";
        /// <summary>
        /// The name of the loaded file
        /// </summary>
        public static string Filename { get { return fname; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Make a new GraphicsData object. Use load(String) to load data.
        /// </summary>
        /// <param name="isTiled">If the data is tiled or not.</param>
        public GraphicsData(bool isTiled, PaletteData palData)
            : base()
        {
            tiled = isTiled;
            tileSize = new Point(8, 8);
            width = 64;
            height = 128;
            zoom = 1;
            this.paletteData = palData;
            isBigEndian = true;
            graphFormat = GraphicsFormat.FORMAT_4BPP;
        }

        /// <summary>
        /// Make a new (linear) GraphicsData object. Use load(String) to load data.
        /// </summary>
        public GraphicsData(PaletteData palData) : this(false, palData) { }

        /// <summary>
        /// Make a new (linear) GraphicsData object, with the specified file as data.
        /// </summary>
        /// <param name="filename">The filename of the file to load.</param>
        public GraphicsData(String filename, PaletteData palData)
            : this(false, palData)
        {
            this.load(filename);
        }

        /// <summary>
        /// Make a new GraphicsData object, with the specified file as data.
        /// </summary>
        /// <param name="filename">The filename of the file to load.</param>
        /// <param name="tiled">If the data is tiled or not.</param>
        public GraphicsData(String filename, bool tiled, PaletteData palData)
            : this(tiled, palData)
        {
            this.load(filename);
        }

        #endregion

        #region Load methods
        internal override void load(String filename)
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

        #region NCGR
        public void loadFileAsNCGR(String filename)
        {
            #region typedef
            /*
		     * typedef struct {
		     * 	char* magHeader; // 4 bytes: RGCN
		     *  DWORD magConst; // FF FE 01 01
		     *  DWORD fileSize;
		     *  WORD headerSize; // should be 10 (i.e. 10 00)
		     *  WORD nSections; // should be 1 or 2 (i.e. 01 00 or 02 00)
		     * 	CHARSection charSection;
		     * } NCGRFile
		     * 
		     * typedef struct {
		     *  char* magHeader; // 4 bytes: RAHC
		     *  DWORD sectSize; // section size; should be fileSize - NCGRHeader.headerSize = fileSize - 0x10
		     *  WORD height; // FF FF if tiled image. otherwise is height / 8
		     *  WORD width; // FF FF if tiled image. otherwise is width / 8
		     *  DWORD palType; // 04 00 -> 8bpp, 03 00 -> 4bpp
		     *  WORD linearFlag; // 1 => linear, 0 => tiled
		     *  WORD unkn2; // ???? part of linearFlag?
		     *  WORD unkn3; // ???? Seen 01 01 here with elebits NPCs
		     *  WORD unkn4; // ????
		     *  DWORD imageDataSize; // length of imageData (in bytes)
		     *  DWORD unkn5; // ???? seems to be constant 18 00 00 00
		     *  byte* imageData;
		     * } CHARSection
		     * 
		     */
            #endregion
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
            // skip 4 bytes; they are the already checked magic header RGCN (or NCGR)
            br.ReadInt32();

            if (br.ReadInt32() != 0x0101FEFF)
            {
                MainWindow.showError("Given file " + filename + " is not a valid NCGR file.\n It does not have the magic constant 0x0101FEFF at 0x04");
                return;
            }
            int fileSize = br.ReadInt32();
            int headerSize = br.ReadInt16();
            int nSections = br.ReadInt16();
            if (nSections > 2 || nSections == 0)
            {
                MainWindow.showError("Given file " + filename + " is not a valid NCGR file or of an unsupported type.\n The amount of sections is invalid (" + nSections + ")");
                return;
            }
            // should now be at CHAR section
            if (br.ReadChar() != 'R' || br.ReadChar() != 'A' || br.ReadChar() != 'H' || br.ReadChar() != 'C')
            {
                MainWindow.showError("Given file " + filename + " is not a valid NCGR file or of an unsupported type.\n The CHAR section does not follow the NCGR header");
                return;
            }

            int charSize = br.ReadInt32();
            int h = br.ReadInt16(), w = br.ReadInt16();

            int ptype = br.ReadInt32();
            switch (ptype)
            {
                case 6: GraphFormat = GraphicsFormat.FORMAT_32BPP; break;
                case 5: GraphFormat = GraphicsFormat.FORMAT_16BPP; break;
                case 4: GraphFormat = GraphicsFormat.FORMAT_8BPP; break;
                case 3: GraphFormat = GraphicsFormat.FORMAT_4BPP; break;
                case 2: GraphFormat = GraphicsFormat.FORMAT_2BPP; break;
                case 1: GraphFormat = GraphicsFormat.FORMAT_1BPP; break;
                default: MainWindow.showError("Unknown GraphicsFormat in NCGR file: " + ptype); break;
            }
            Tiled = br.ReadInt32() == 0;
            if (Tiled)
            {
                h *= 8; w *= 8; TileSize = new Point(8, 8);
            }

            if (h > 0 && w > 0)
            {
                Height = (uint)h; Width = (uint)w;
            }

            // 2*WORD = 8 bytes unknown => skip int32
            int t = br.ReadInt32();
#if DEBUG
            if (t > 1)
                MainWindow.showError("Padding in NCGR>CHAR is not padding");
#endif
            int imLength = br.ReadInt32();
            
#if DEBUG
            if (br.ReadInt32() != 0x18)
                MainWindow.showError("Value at end of CHAR header isn't 0x18");
#else
            br.ReadInt32();
#endif

            this.Data = br.ReadBytes(imLength);

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
             *   DWORD fileSize;
             *   char* version; // '1.02'
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

            if (br.ReadInt32() != br.BaseStream.Length)
            {
                MainWindow.showError(String.Format("Invalid GFNT file {0:s}; value at 0x04 is not filesizeLoading as generic file instead.", filename));
                br.Close();
                base.loadGenericData(filename);
                return;
            }

            char c4;
            if ((char)br.ReadByte() != '1' || (char)br.ReadByte() != '.' || (char)br.ReadByte() != '0' || ((c4 = (char)br.ReadByte()) != '2' && c4 != '1'))
            {
                MainWindow.showError(String.Format("Unsupported GFNT file {0:s}; does not have version 1.02Loading as generic file instead.", filename));
                br.Close();
                base.loadGenericData(filename);
                return;
            }

            int imbpp = br.ReadInt32();
            int imwidth = 8 << br.ReadInt32();
            int imheight = 8 << br.ReadInt32();
            int nPals = br.ReadInt32();

            int nBytesForIm = imwidth * imheight;

            switch (imbpp)
            {
                case 1: nBytesForIm /= 8; break;
                case 2: nBytesForIm /= 4; break;
                case 3: nBytesForIm /= 2; break;
                case 4: nBytesForIm *= 1; break;
                case 5: nBytesForIm *= 2; break;
                case 6: nBytesForIm *= 3; break;
                case 7: nBytesForIm *= 4; break;
                default: MainWindow.showError("Possibly invalid GFNT file: unknown GraphicsFormat " + imbpp); br.Close(); return;
            }

            this.Data = br.ReadBytes(nBytesForIm);

            graphFormat = (GraphicsFormat)imbpp;
            isBigEndian = true;
            tiled = false;
            width = (uint)imwidth;
            height = (uint)imheight;

            br.Close();
        }
        #endregion

        #endregion

        #region Methods: paint
        internal override void paint(object sender, PaintEventArgs e)
        {
            Bitmap b = toBitmap();
            Bitmap scaled = new Bitmap((int)(Width * Zoom), (int)(Height * Zoom));
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                g.DrawImage(b, new Rectangle(0, 0, scaled.Width, scaled.Height), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            }
            e.Graphics.DrawImage(scaled, 4, 4);
        }

        #region paint1BPP
        internal unsafe Bitmap paint1BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 8 + (uint)(nNecessBytes % 8 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            uint pixNum = 0, nPixels = (uint)(width * height);

            int[] palette = {-0x7F181818, -0x7FA8A8A8};

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    for (int b = 0; b < 8; b++)
                    {
                        if (++pixNum > nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)(bt & (0x01 << b));
                        else
                            j = (uint)(bt & (0x80 >> b));

                        if (++xintl == TileSize.X)
                        {
                            xintl = 0;
                            if (++yintl == TileSize.Y)
                            {
                                yintl = 0;
                                if (++tx == ntx)
                                {
                                    tx = 0;
                                    if (++ty == nty)
                                        break;
                                }
                            }
                        }
                        x = tx * TileSize.X + xintl;
                        y = ty * TileSize.Y + yintl;

                        *(bmptr + y * width + x) = palette[j > 0 ? 1 : 0];
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);
                    
                    for (int b = 0; b < 8; b++)
                    {
                        if (++pixNum > nPixels) // disregard pixels outside of the screen
                            break;
                        if (IsBigEndian)
                            j = (uint)(bt & (0x01 << b));
                        else
                            j = (uint)(bt & (0x80 >> b));

                        *(bmptr++) = palette[j > 0 ? 1 : 0];
                    }
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;
        }
        #endregion

        #region paint2BPP
        internal unsafe Bitmap paint2BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 4 + (uint)(nNecessBytes % 4 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            int[] palette = paletteData.getFullPalette();

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    for (int b = 0; b < 4; b++)
                    {
                        if (++pixNum > nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x03 << (b * 2))) >> (b * 2));
                        else
                            j = (uint)((bt & (0xC0 >> (b * 2))) >> ((3 - b) * 2));

                        if (++xintl == TileSize.X)
                        {
                            xintl = 0;
                            if (++yintl == TileSize.Y)
                            {
                                yintl = 0;
                                if (++tx == ntx)
                                {
                                    tx = 0;
                                    if (++ty == nty)
                                        break;
                                }
                            }
                        }
                        x = tx * TileSize.X + xintl;
                        y = ty * TileSize.Y + yintl;

                        *(bmptr + y * width + x) = palette[j];
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    for (int b = 0; b < 4; b++)
                    {
                        if (++pixNum > nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x03 << (b * 2))) >> (b * 2));
                        else
                            j = (uint)((bt & (0xC0 >> (b * 2))) >> ((3 - b) * 2));

                        *(bmptr++) = palette[j];
                    }
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;
        }
        #endregion

        #region paint4BPP
        internal unsafe Bitmap paint4BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 2 + (uint)(nNecessBytes % 2 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            int[] palette = paletteData.getFullPalette();

            bool atEnd = false;

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    for (int b = 0; b < 2; b++)
                    {
                        if (++pixNum > nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x0F << (b * 4))) >> (b * 4));
                        else
                            j = (uint)((bt & (0xF0 >> (b * 4))) >> ((1 - b) * 4));

                        if (++xintl == TileSize.X)
                        {
                            xintl = 0;
                            if (++yintl == TileSize.Y)
                            {
                                yintl = 0;
                                if (++tx == ntx)
                                {
                                    tx = 0;
                                    if (++ty == nty)
                                        break;
                                }
                            }
                        }
                        x = tx * TileSize.X + xintl;
                        y = ty * TileSize.Y + yintl;
                        
                        *(bmptr + y * width + x) = palette[j];
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    for (int b = 0; b < 2; b++)
                    {
                        if (++pixNum > nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x0F << (b * 4))) >> (b * 4));
                        else
                            j = (uint)((bt & (0xF0 >> (b * 4))) >> ((1 - b) * 4));

                        *(bmptr++) = palette[j];
                    }
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;
        }
        #endregion

        #region paint8BPP
        internal unsafe Bitmap paint8BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            int[] palette = this.paletteData.getFullPalette();

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;
            
            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;
                
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    if (++xintl == TileSize.X)
                    {
                        xintl = 0;
                        if (++yintl == TileSize.Y)
                        {
                            yintl = 0;
                            if (++tx == ntx) { tx = 0; if (++ty == nty) break; }
                        }
                    }
                    x = tx * TileSize.X + xintl;
                    y = ty * TileSize.Y + yintl;

                    *(bmptr + y * width + x) = palette[Next(out atEnd)];
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    *(bmptr++) = palette[Next(out atEnd)];
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;
        }
        #endregion

        #region paint16BPP
        internal unsafe Bitmap paint16Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length >> 1);
            byte[] bytes = new byte[2];

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);

                    if (++xintl == TileSize.X)
                    {
                        xintl = 0;
                        if (++yintl == TileSize.Y)
                        {
                            yintl = 0;
                            if (++tx == ntx)
                            {
                                tx = 0;
                                if (++ty == nty) break;
                            }
                        }
                    }
                    x = tx * TileSize.X + xintl;
                    y = ty * TileSize.Y + yintl;

                    *(bmptr + y * width + x) = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    int col = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);

                    *(bmptr++) = col;
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;
            
        }
        #endregion

        #region paint24BPP
        internal unsafe Bitmap paint24Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length / 3);
            byte[] bytes = new byte[3];

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y; // x/y in image

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);

                    if (++xintl == TileSize.X)
                    {
                        xintl = 0;
                        if (++yintl == TileSize.Y)
                        {
                            yintl = 0;
                            if (++tx == ntx)
                            {
                                tx = 0;
                                if (++ty == nty) break;
                            }
                        }
                    }
                    x = tx * TileSize.X + xintl;
                    y = ty * TileSize.Y + yintl;

                    *(bmptr + y * width + x) = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);

                    *(bmptr++) = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;

        }
        #endregion

        #region paint32BPP
        internal unsafe Bitmap paint32Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length / 4);
            byte[] bytes = new byte[4];

            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bmptr = (int*)bmd.Scan0.ToPointer();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                int x, y;

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);
                    bytes[3] = Next(out atEnd);

                    if (++xintl == TileSize.X)
                    {
                        xintl = 0;
                        if (++yintl == TileSize.Y)
                        {
                            yintl = 0;
                            if (++tx == ntx) { tx = 0; if (++ty == nty)break; }
                        }
                    }
                    x = tx * TileSize.X + xintl;
                    y = ty * TileSize.Y + yintl;

                    *(bmptr + y * width + x) = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);
                    bytes[3] = Next(out atEnd);

                    *(bmptr++) = (int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian);
                }
                #endregion
            }
            bitmap.UnlockBits(bmd);
            return bitmap;

        }
        #endregion

        #endregion

        internal override void copyToClipboard()
        {
            Clipboard.SetImage(toBitmap());
        }

        #region methods: increase/decrease width/height
        internal void increaseWidth() { Width += WidthSkipSizeUInt; }
        internal void decreaseWidth() { Width -= WidthSkipSizeUInt; }
        internal void increaseHeight() { Height += HeightSkipSizeUInt; }
        internal void decreaseHeight() { Height -= HeightSkipSizeUInt; }
        #endregion

        #region Toggle methods
        internal void toggleGraphicsFormat()
        {
            GraphFormat = (GraphicsFormat)((int)GraphFormat % 7 + 1);
        }

        internal void toggleTiled()
        {
            Tiled = !Tiled;
        }

        internal void toggleEndianness()
        {
            IsBigEndian = !IsBigEndian;
        }

        internal void toggleSkipSize()
        {
            int ss = (int)SkipSize;
            ss = (ss + 1) % 7;
            SkipSize = (GraphicsSkipSize)ss;
        }

        internal void toggleWidthSkipSize()
        {
            int wss = (int)WidthSkipSize;
            wss = (wss + 1) % 6;
            WidthSkipSize = (HWSkipSize)wss;
        }

        internal void toggleHeightSkipSize()
        {
            int hss = (int)HeightSkipSize;
            hss = (hss + 1) % 6;
            HeightSkipSize = (HWSkipSize)hss;
        }
        #endregion

        #region Methods: DoSkip
        internal override void DoSkip(bool positive)
        {
            long bytesToSkip;
            switch (GraphFormat)
            {
                case GraphicsFormat.FORMAT_1BPP: bytesToSkip = width / 8; break;
                case GraphicsFormat.FORMAT_2BPP: bytesToSkip = width / 4; break;
                case GraphicsFormat.FORMAT_4BPP: bytesToSkip = width / 2; break;
                case GraphicsFormat.FORMAT_8BPP: bytesToSkip = width; break;
                case GraphicsFormat.FORMAT_16BPP: bytesToSkip = width * 2; break;
                case GraphicsFormat.FORMAT_24BPP: bytesToSkip = width * 3; break;
                case GraphicsFormat.FORMAT_32BPP: bytesToSkip = width * 4; break;
                default: throw new Exception("Unkown error: invalid Graphics Format");
            }
            switch (SkipSize)
            {
                case GraphicsSkipSize.SKIPSIZE_1BYTE: bytesToSkip = 1; break;
                case GraphicsSkipSize.SKIPSIZE_2BYTES: bytesToSkip = 2; break;
                case GraphicsSkipSize.SKIPSIZE_4BYTES: bytesToSkip = 4; break;
                case GraphicsSkipSize.SKIPSIZE_1PIXROW: break;
                case GraphicsSkipSize.SKIPSIZE_1TILEROW: bytesToSkip *= TileSize.Y; break;
                case GraphicsSkipSize.SKIPSIZE_WIDTHROWS: bytesToSkip *= Width; break;
                case GraphicsSkipSize.SKIPSIZE_HEIGHTROWS: bytesToSkip *= Height; break;
                default: throw new Exception("Invalid Graphics Skip Size " + skipSize.ToString());
            }
            DoSkip(positive, bytesToSkip);
        }
        #endregion

        internal override Bitmap toBitmap()
        {
            if (!HasData)
                return new Bitmap(1, 1);
            switch (GraphFormat)
            {
                case GraphicsFormat.FORMAT_1BPP: return paint1BPP();
                case GraphicsFormat.FORMAT_2BPP: return paint2BPP();
                case GraphicsFormat.FORMAT_4BPP: return paint4BPP();
                case GraphicsFormat.FORMAT_8BPP: return paint8BPP();
                case GraphicsFormat.FORMAT_16BPP: return paint16Bpp();
                case GraphicsFormat.FORMAT_24BPP: return paint24Bpp();
                case GraphicsFormat.FORMAT_32BPP: return paint32Bpp();
                default: throw new Exception("Unknown error; invalid Graphics Format " + graphFormat.ToString());
            }
        }
    }

    #region Graphics enums (GraphicsFormat, GraphicsSkipMetric, HWSkipSize)
    public enum GraphicsFormat : int
    {
        FORMAT_1BPP = 1,
        FORMAT_2BPP = 2,
        FORMAT_4BPP = 3,
        FORMAT_8BPP = 4,
        FORMAT_16BPP = 5,
        FORMAT_24BPP = 6,
        FORMAT_32BPP = 7
    }
    public enum GraphicsSkipSize
    {
        SKIPSIZE_1BYTE = 0,
        SKIPSIZE_2BYTES = 1,
        SKIPSIZE_4BYTES = 2,
        SKIPSIZE_1PIXROW = 3,
        SKIPSIZE_1TILEROW = 4,
        SKIPSIZE_WIDTHROWS = 5,
        SKIPSIZE_HEIGHTROWS = 6
    }
    public enum HWSkipSize
    {
        SKIPSIZE_1PIX = 0,
        SKIPSIZE_2PIX = 1,
        SKIPSIZE_4PIX = 2,
        SKIPSIZE_8PIX = 3,
        SKIPSIZE_16PIX = 4,
        SKIPSIZE_1TILE = 5
    }
    #endregion
}
