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
                if (newW < widthSkipSize)
                    newW = widthSkipSize;
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
        private static byte widthSkipSize = 4;
        /// <summary>
        /// How many pixels the width should change with the press of a button
        /// </summary>
        internal static byte WidthSkipSize { get { return widthSkipSize; } set { widthSkipSize = Math.Max((byte)1, value); } }

        #endregion

        #region Field: Height
        /// <summary>
        /// The height of the shown data
        /// </summary>
        private static uint height;
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
                if (newH < heightSkipSize)
                    newH = heightSkipSize;
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
        private static byte heightSkipSize = 8;
        /// <summary>
        /// How many pixels the height should change with the press of a button
        /// </summary>
        internal static byte HeightSkipSize { get { return heightSkipSize; } set { heightSkipSize = Math.Max((byte)1, value); } }
        #endregion

        #region Field: TileSize
        /// <summary>
        /// The size of a tile
        /// </summary>
        private static Point tileSize;
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
                if (Tiled)
                    MainWindow.DoRefresh();
            }
        }
        #endregion

        #region Field: Tiled
        /// <summary>
        /// If the data is tiled or not
        /// </summary>
        private static bool tiled;
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
        private static uint zoom;
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
        private static GraphicsFormat graphFormat;
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
        private static bool isBigEndian;
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
            e.Graphics.DrawImage(toBitmap(), 0, 0);
        }

        #region paint1BPP
        internal Bitmap paint1BPP(/*object sender, PaintEventArgs e*/)
        {
            //Graphics g = e.Graphics;

            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 8 + (uint)(nNecessBytes % 8 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            uint pixNum = 0, nPixels = (uint)(width * height);
            long dataOffset = Offset;
            int x, y;
            Color dark = Color.FromArgb(-0x7F181818), light = Color.FromArgb(-0x7FA8A8A8);
            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = getData(dataOffset++, out atEnd);

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
                        x = (int)(Zoom * (tx * TileSize.X + xintl));
                        y = (int)(Zoom * (ty * TileSize.Y + yintl));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, j > 0 ? light : dark);
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = getData(dataOffset++, out atEnd);

                    for (int b = 0; b < 8; b++)
                    {
                        if (++pixNum >= nPixels) // disregard pixels outside of the screen
                            break;
                        if (IsBigEndian)
                            j = (uint)(bt & (0x01 << b));
                        else
                            j = (uint)(bt & (0x80 >> b));

                        y = (int)(Zoom * ((pixNum - 1) / width));
                        x = (int)(Zoom * ((pixNum - 1) % width));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, j > 0 ? light : dark);
                    }
                }
                #endregion
            }
            return bitmap;// g.DrawImage(bitmap, 0, 0);
        }
        #endregion

        #region paint2BPP
        internal Bitmap paint2BPP(/*object sender, PaintEventArgs e*/)
        {
            //Graphics g = e.Graphics;

            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 4 + (uint)(nNecessBytes % 4 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);
            int x, y;
            long dataOffset = Offset;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

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
                    bt = getData(dataOffset++, out atEnd);

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
                        x = (int)(Zoom * (tx * TileSize.X + xintl));
                        y = (int)(Zoom * (ty * TileSize.Y + yintl));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, palette[j]);
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = getData(dataOffset++, out atEnd);

                    for (int b = 0; b < 4; b++)
                    {
                        if (++pixNum >= nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x03 << (b * 2))) >> (b * 2));
                        else
                            j = (uint)((bt & (0xC0 >> (b * 2))) >> ((3 - b) * 2));

                        x = (int)(Zoom * ((pixNum - 1) % width));
                        y = (int)(Zoom * ((pixNum - 1) / width));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, palette[j]);
                    }
                }
                #endregion
            }
            return bitmap;// g.DrawImage(bitmap, 0, 0);
        }
        #endregion

        #region paint4BPP
        internal Bitmap paint4BPP(/*object sender, PaintEventArgs e*/)
        {
            //Graphics g = e.Graphics;

            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 4 + (uint)(nNecessBytes % 4 > 0 ? 1 : 0);
            nNecessBytes = (uint)Math.Min(nNecessBytes, this.Length);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);
            int x, y;
            long dataOffset = Offset;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

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
                    bt = getData(dataOffset++, out atEnd);

                    for (int b = 0; b < 4; b++)
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
                        x = (int)(Zoom * (tx * TileSize.X + xintl));
                        y = (int)(Zoom * (ty * TileSize.Y + yintl));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, palette[j]);
                    }
                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = getData(dataOffset++, out atEnd);

                    for (int b = 0; b < 4; b++)
                    {
                        pixNum++;
                        if (pixNum >= nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x0F << (b * 4))) >> (b * 4));
                        else
                            j = (uint)((bt & (0xF0 >> (b * 4))) >> ((1 - b) * 4));

                        x = (int)(Zoom * ((pixNum - 1) % width));
                        y = (int)(Zoom * ((pixNum - 1) / width));

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, palette[j]);
                    }
                }
                #endregion
            }
            return bitmap;// g.DrawImage(bitmap, 0, 0);
        }
        #endregion

        #region paint8BPP
        internal Bitmap paint8BPP(/*object sender, PaintEventArgs pea*/)
        {
            //Graphics g = pea.Graphics;

            uint nNecessBytes = width * height;

            int x, y;
            byte bt;
            long dataOffset = Offset;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            Color[] palette = this.paletteData.getFullPaletteAsColor();

            bool atEnd = false;
            
            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile
                

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
                    x = (int)(Zoom * (tx * TileSize.X + xintl));
                    y = (int)(Zoom * (ty * TileSize.Y + yintl));
                    bt = getData(dataOffset++, out atEnd);
                    for (int zy = 0; zy < Zoom; zy++)
                        for (int zx = 0; zx < Zoom; zx++)
                            bitmap.SetPixel(x + zx, y + zy, palette[bt]);

                }
                #endregion
            }
            else
            {
                #region linear

                for (int i = 0; i < nNecessBytes && !atEnd; i++)
                {
                    bt = getData(dataOffset++, out atEnd);

                    x = (int)(Zoom * (i % width));
                    y = (int)(Zoom * (i / width));

                    for (int zy = 0; zy < Zoom; zy++)
                        for (int zx = 0; zx < Zoom; zx++)
                            bitmap.SetPixel(x + zx, y + zy, palette[bt]);
                }
                
                #endregion
            }
            return bitmap;// g.DrawImage(bitmap, 0, 0);
        }
        #endregion

        #region paint16BPP
        internal Bitmap paint16Bpp(/*object sender, PaintEventArgs pea*/)
        {
            //Graphics g = pea.Graphics;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            uint nPixels = (uint)Math.Min(width * height, this.Length >> 1);
            uint nNecessBytes = nPixels * 2;
            int x, y, zx, zy;
            byte[] bytes = new byte[2];
            long dataOffset = Offset;

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
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
                    x = (int)(Zoom * (tx * TileSize.X + xintl));
                    y = (int)(Zoom * (ty * TileSize.Y + yintl));

                    for (zx = 0; zx < Zoom; zx++)
                        for (zy = 0; zy < Zoom; zy++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(Zoom * (i % width));
                    y = (int)(Zoom * (i / width));

                    for (zy = 0; zy < Zoom; zy++)
                        for (zx = 0; zx < Zoom; zx++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }

            return bitmap;// g.DrawImage(bitmap, 0, 0);
            
        }
        #endregion

        #region paint24BPP
        internal Bitmap paint24Bpp(/*object sender, PaintEventArgs pea*/)
        {
            //Graphics g = pea.Graphics;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            uint nPixels = (uint)Math.Min(width * height, this.Length / 3);
            uint nNecessBytes = nPixels * 3;
            int x, y, zx, zy;
            byte[] bytes = new byte[3];
            long dataOffset = Offset;

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
                    bytes[2] = getData(dataOffset++, out atEnd);
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
                    x = (int)(Zoom * (tx * TileSize.X + xintl));
                    y = (int)(Zoom * (ty * TileSize.Y + yintl));

                    for (zx = 0; zx < Zoom; zx++)
                        for (zy = 0; zy < Zoom; zy++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {

                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
                    bytes[2] = getData(dataOffset++, out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(Zoom * (i % width));
                    y = (int)(Zoom * (i / width));

                    for (zy = 0; zy < Zoom; zy++)
                        for (zx = 0; zx < Zoom; zx++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }

            return bitmap;// g.DrawImage(bitmap, 0, 0);

        }
        #endregion

        #region paint32BPP
        internal Bitmap paint32Bpp(/*object sender, PaintEventArgs pea*/)
        {
            //Graphics g = pea.Graphics;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            uint nPixels = (uint)Math.Min(width * height, this.Length / 4);
            uint nNecessBytes = nPixels * 4;
            int x, y, zx, zy;
            byte[] bytes = new byte[4];
            long dataOffset = Offset;

            bool atEnd = false;

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile

                for (int i = 0; i < nPixels && !atEnd; i++)
                {
                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
                    bytes[2] = getData(dataOffset++, out atEnd);
                    bytes[3] = getData(dataOffset++, out atEnd);
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
                    x = (int)(Zoom * (tx * TileSize.X + xintl));
                    y = (int)(Zoom * (ty * TileSize.Y + yintl));

                    for (zx = 0; zx < Zoom; zx++)
                        for (zy = 0; zy < Zoom; zy++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }
            else
            {
                #region linear
                for (int i = 0; i < nPixels && !atEnd; i++)
                {

                    bytes[0] = getData(dataOffset++, out atEnd);
                    bytes[1] = getData(dataOffset++, out atEnd);
                    bytes[2] = getData(dataOffset++, out atEnd);
                    bytes[3] = getData(dataOffset++, out atEnd);
                    Color pal = Color.FromArgb((int)paletteData.getPalette(bytes, (int)GraphFormat, IsBigEndian));

                    x = (int)(Zoom * (i % width));
                    y = (int)(Zoom * (i / width));

                    for (zy = 0; zy < Zoom; zy++)
                        for (zx = 0; zx < Zoom; zx++)
                            bitmap.SetPixel(x + zx, y + zy, pal);

                }
                #endregion
            }

            return bitmap;// g.DrawImage(bitmap, 0, 0);

        }
        #endregion

        #endregion

        internal override void copyToClipboard()
        {
            Clipboard.SetImage(toBitmap());
        }
        
        #region Method: getPixel(idx)
        /*
        /// <summary>
        /// Get the colour of a pixel
        /// </summary>
        /// <param name="idx">The index of the pixel, relative to the current offset</param>
        /// <returns>The colour of the pixel with the given index relative to the current offset, or pure black if the index is out of range.</returns>
        internal uint getPixel(uint idx)
        {
            uint necessByte, necessBit, palidx, bitsetidx;
            switch (graphFormat)
            {
                #region case 1BPP
                case GraphicsFormat.FORMAT_1BPP:
                    necessByte = (uint)this.Data[Offset + idx / 8];
                    necessBit = idx % 8;
                    if(IsBigEndian)
                        palidx = necessByte & (uint)(0x01 << (int)necessBit);
                    else
                        palidx = necessByte & (uint)(0x80 >> (int)necessBit);
                    if (palidx > 0)
                        return 0xFFE0E0E0;
                    else
                        return 0xFF000000;
                #endregion

                #region case 2BPP
                case GraphicsFormat.FORMAT_2BPP:
                    necessByte = (uint)this.Data[Offset + idx / 4];
                    bitsetidx = idx % 4;
                    if(IsBigEndian)
                        palidx = (uint)(necessByte & (0x03 << (int)(bitsetidx * 2))) >> (int)(bitsetidx * 2);
                    else
                        palidx = (uint)(necessByte & (0xC0 >> (int)(bitsetidx * 2))) >> ((int)(3 - bitsetidx) * 2);
                    return this.paletteData.getPalette(palidx);
                #endregion

                #region case 4BPP
                case GraphicsFormat.FORMAT_4BPP:
                    necessByte = (uint)this.Data[Offset + idx / 2];
                    bitsetidx = IsBigEndian ? 1 - (idx % 2) : idx % 2;
                    if (bitsetidx == 0)
                        palidx = (necessByte & 0xF0) >> 4;
                    else // if(bitsetidx == 1)
                        palidx = necessByte & 0x0F;
                    return this.paletteData.getPalette(palidx);
                #endregion

                #region case 8BPP
                case GraphicsFormat.FORMAT_8BPP:
                    return this.paletteData.getPalette((uint)this.Data[this.Offset + idx]);
                #endregion

                #region case 16BPP/24BPP/32BPP
                case GraphicsFormat.FORMAT_16BPP:
                case GraphicsFormat.FORMAT_24BPP:
                case GraphicsFormat.FORMAT_32BPP:
                    //return this.paletteData.getPalette(idx, this.Data, this.Offset, (int)graphFormat, IsBigEndian);
                #endregion

                default: throw new Exception("Unkown error: invalid GraphicsFormat " + graphFormat.ToString());
            }
        }
        */
        #endregion

        #region methods: increase/decrease width/height
        internal void increaseWidth() { Width += WidthSkipSize; }
        internal void decreaseWidth() { Width -= WidthSkipSize; }
        internal void increaseHeight() { Height += HeightSkipSize; }
        internal void decreaseHeight() { Height -= HeightSkipSize; }
        #endregion

        #region Toggle methods
        internal void toggleGraphicsFormat()
        {
            int g = (int)GraphFormat + 1;
            GraphFormat = (GraphicsFormat)(g % 8);
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
            if (widthSkipSize < 16)
                widthSkipSize <<= 1;
            else
                widthSkipSize = 1;
        }

        internal void toggleHeightSkipSize()
        {
            if (heightSkipSize < 16)
                heightSkipSize <<= 1;
            else
                heightSkipSize = 1;
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

    #region Graphics enums (GraphicsFormat, GraphicsSkipMetric)
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
    #endregion
}
