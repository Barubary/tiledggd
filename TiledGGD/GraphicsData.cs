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
        private static uint zoom = 1;
        /// <summary>
        /// The zoom of the graphics data
        /// </summary>
        internal static uint Zoom
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

        internal override void load(String filename)
        {
            base.loadGenericData(filename);
            if (filename.Contains("/"))
                fname = filename.Substring(filename.LastIndexOf("/")+1);
            else if (filename.Contains("\\"))
                fname = filename.Substring(filename.LastIndexOf("\\")+1);
            else
                fname = filename;
        }

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
        internal Bitmap paint1BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 8 + (uint)(nNecessBytes % 8 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            uint pixNum = 0, nPixels = (uint)(width * height);

            int x, y;
            Color dark = Color.FromArgb(-0x7F181818), light = Color.FromArgb(-0x7FA8A8A8);
            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

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
                        x = (int)(tx * TileSize.X + xintl);
                        y = (int)(ty * TileSize.Y + yintl);

                        bitmap.SetPixel(x, y, j > 0 ? light : dark);
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

                        y = (int)((pixNum - 1) / width);
                        x = (int)((pixNum - 1) % width);

                        bitmap.SetPixel(x, y, j > 0 ? light : dark);
                    }
                }
                #endregion
            }
            return bitmap;
        }
        #endregion

        #region paint2BPP
        internal Bitmap paint2BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 4 + (uint)(nNecessBytes % 4 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);
            int x, y;

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            Color[] palette = paletteData.getFullPaletteAsColor();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

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
                        x = (int)(tx * TileSize.X + xintl);
                        y = (int)(ty * TileSize.Y + yintl);

                        bitmap.SetPixel(x, y, palette[j]);
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

                        x = (int)((pixNum - 1) % width);
                        y = (int)((pixNum - 1) / width);

                        bitmap.SetPixel(x, y, palette[j]);
                    }
                }
                #endregion
            }
            return bitmap;
        }
        #endregion

        #region paint4BPP
        internal Bitmap paint4BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 2 + (uint)(nNecessBytes % 2 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);
            int x, y;

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            Color[] palette = paletteData.getFullPaletteAsColor();

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

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
                        x = (int)(tx * TileSize.X + xintl);
                        y = (int)(ty * TileSize.Y + yintl);
                        
                        bitmap.SetPixel(x, y, palette[j]);
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

                        x = (int)((pixNum - 1) % width);
                        y = (int)((pixNum - 1) / width);

                        bitmap.SetPixel(x, y, palette[j]);
                    }
                }
                #endregion
            }
            return bitmap;
        }
        #endregion

        #region paint8BPP
        internal Bitmap paint8BPP()
        {
            ResetPtr();
            uint nNecessBytes = width * height;

            int x, y;
            byte bt;
            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            Color[] palette = this.paletteData.getFullPaletteAsColor();

            bool atEnd = false;
            
            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile
                

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
                    x = (int)(tx * TileSize.X + xintl);
                    y = (int)(ty * TileSize.Y + yintl);
                    bt = Next(out atEnd);
                    bitmap.SetPixel(x, y, palette[bt]);

                }
                #endregion
            }
            else
            {
                #region linear

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = Next(out atEnd);

                    x = (int)(i % width);
                    y = (int)(i / width);

                    bitmap.SetPixel(x, y, palette[bt]);
                }
                
                #endregion
            }
            return bitmap;
        }
        #endregion

        #region paint16BPP
        internal Bitmap paint16Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length >> 1);
            uint nNecessBytes = nPixels * 2;
            int x, y;
            byte[] bytes = new byte[2];

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

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
                    x = (int)(tx * TileSize.X + xintl);
                    y = (int)(ty * TileSize.Y + yintl);

                    bitmap.SetPixel(x, y, pal);

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
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(i % width);
                    y = (int)(i / width);

                    bitmap.SetPixel(x, y, pal);

                }
                #endregion
            }

            return bitmap;
            
        }
        #endregion

        #region paint24BPP
        internal Bitmap paint24Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length / 3);
            uint nNecessBytes = nPixels * 3;
            int x, y;
            byte[] bytes = new byte[3];

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

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
                    x = (int)(tx * TileSize.X + xintl);
                    y = (int)(ty * TileSize.Y + yintl);

                    bitmap.SetPixel(x, y, pal);

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
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(i % width);
                    y = (int)(i / width);

                    bitmap.SetPixel(x, y, pal);

                }
                #endregion
            }

            return bitmap;

        }
        #endregion

        #region paint32BPP
        internal Bitmap paint32Bpp()
        {
            ResetPtr();

            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            if (!this.HasData)
                return bitmap;

            uint nPixels = (uint)Math.Min(width * height, this.Length / 4);
            uint nNecessBytes = nPixels * 4;
            int x, y;
            byte[] bytes = new byte[4];

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = Next(out atEnd);
                    bytes[1] = Next(out atEnd);
                    bytes[2] = Next(out atEnd);
                    bytes[3] = Next(out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    if (++xintl == TileSize.X)
                    {
                        xintl = 0;
                        if (++yintl == TileSize.Y)
                        {
                            yintl = 0;
                            if (++tx == ntx) { tx = 0; if (++ty == nty)break; }
                        }
                    }
                    x = (int)(tx * TileSize.X + xintl);
                    y = (int)(ty * TileSize.Y + yintl);

                    bitmap.SetPixel(x, y, pal);

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
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(i % width);
                    y = (int)(i / width);

                    bitmap.SetPixel(x, y, pal);

                }
                #endregion
            }

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
    public enum GraphicsFormat
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
