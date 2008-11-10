using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace TiledGGD
{
    class GraphicsData : BrowseableData
    {
        #region Fields

        #region Field: Width
        /// <summary>
        /// The width of the shown data.
        /// </summary>
        private uint width = 64;
        /// <summary>
        /// Get or set the width of the shown data. Will automatically redraw the screen.
        /// </summary>
        internal uint Width
        {
            get { return this.width; }
            set
            {
                uint newW = value;
                if (this.Tiled && newW % this.TileSize.X != 0)
                    newW += (uint)this.TileSize.X - newW % (uint)this.TileSize.X;
                if (newW < widthSkipSize)
                    newW = widthSkipSize;
                if (this.width != newW)
                {
                    this.width = newW;
                    MainWindow.DoRefresh();
                }
            }
        }

        /// <summary>
        /// How many pixels the width should change with the press of a button
        /// </summary>
        private uint widthSkipSize = 4;
        /// <summary>
        /// How many pixels the width should change with the press of a button
        /// </summary>
        internal uint WidthSkipSize { get { return this.widthSkipSize; } set { this.widthSkipSize = Math.Max(1, value); } }

        #endregion

        #region Field: Height
        /// <summary>
        /// The height of the shown data
        /// </summary>
        private uint height;
        /// <summary>
        /// The height of the shown data
        /// </summary>
        internal uint Height
        {
            get { return this.height; }
            set
            {
                uint newH = value;
                if (this.Tiled && newH % this.TileSize.Y != 0)
                    newH += (uint)this.TileSize.Y - newH % (uint)this.TileSize.Y;
                if (newH < heightSkipSize)
                    newH = heightSkipSize;
                if (this.height != newH)
                {
                    this.height = newH;
                    MainWindow.DoRefresh();
                }
            }
        }

        /// <summary>
        /// How many pixels the height should change with the press of a button
        /// </summary>
        private uint heightSkipSize = 8;
        /// <summary>
        /// How many pixels the height should change with the press of a button
        /// </summary>
        internal uint HeightSkipSize { get { return this.heightSkipSize; } set { this.heightSkipSize = Math.Max(1, value); } }
        #endregion

        #region Field: TileSize
        /// <summary>
        /// The size of a tile
        /// </summary>
        private Point tileSize;
        /// <summary>
        /// Get or set the size of a tile. Negative values will be made positive.
        /// </summary>
        public Point TileSize
        {
            get { return this.tileSize; }
            set
            {
                this.tileSize = value;
                tileSize.X = Math.Abs(tileSize.X);
                tileSize.Y = Math.Abs(tileSize.Y);
                if (this.Tiled)
                    MainWindow.DoRefresh();
            }
        }
        #endregion

        #region Field: Tiled
        /// <summary>
        /// If the data is tiled or not
        /// </summary>
        private bool tiled;
        /// <summary>
        /// Get or set if the data is tiled or not
        /// </summary>
        internal bool Tiled
        {
            get { return this.tiled; }
            set
            {
                if (value != this.tiled)
                {
                    this.tiled = value;
                    if (this.tiled)
                    {
                        // make sure the width & height are correct with the current tilesize
                        this.Width = this.Width;
                        this.Height = this.Height;
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
        private uint zoom;
        /// <summary>
        /// The zoom of the graphics data
        /// </summary>
        internal uint Zoom
        {
            get { return this.zoom; }
            set
            {
                this.zoom = Math.Max(1, Math.Min(8, value));
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

        #endregion

        #region Constructors

        /// <summary>
        /// Make a new GraphicsData object. Use load(String) to load data.
        /// </summary>
        /// <param name="isTiled">If the data is tiled or not.</param>
        public GraphicsData(bool isTiled, PaletteData palData)
            : base()
        {
            this.tiled = isTiled;
            this.tileSize = new Point(8, 8);
            this.width = 64;
            this.height = 128;
            this.zoom = 1;
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
        }

        #region Methods: paint
        internal override void paint(object sender, PaintEventArgs e)
        {
            switch (graphFormat)
            {
                case GraphicsFormat.FORMAT_1BPP: paint1BPP(sender, e); return;
                case GraphicsFormat.FORMAT_2BPP: paint2BPP(sender, e); return;
            }
            // TODO: speed up. no calling lots of methods, just iterate through the data. 
            // may be ugly, but it's faster to have a lot of specialized methods and call one than to call one generalized method lots of times
            Graphics g = e.Graphics;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (!this.Tiled)
                    {
                        Color pixClr = Color.FromArgb((int)getPixel((uint)(y * width + x)));
                        Pen p = new Pen(pixClr);
                        g.FillRectangle(p.Brush, x * Zoom, y * Zoom, Zoom, Zoom);
                    }
                }
        }

        #region paint1BPP
        private void paint1BPP(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 8 + (uint)(nNecessBytes % 8 > 0 ? 1 : 0);

            uint bt, j;
            uint pixNum = 0, nPixels = (uint)(width * height);
            int x, y;
            Color dark = Color.FromArgb(-0x7F202020), light = Color.FromArgb(-0x7FB0B0B0);
            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = 0, yintl = 0; // x & y inside tile

                for (int i = 0; i < nNecessBytes; i++)
                {
                    try { bt = (uint)Data[Offset + i]; }
                    catch (IndexOutOfRangeException) { bt = 0; }

                    for (int b = 0; b < 8; b++)
                    {
                        pixNum++;
                        if (pixNum > nPixels)
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
                for (int i = 0; i < nNecessBytes; i++)
                {
                    try { bt = (uint)Data[Offset + i]; }
                    catch (IndexOutOfRangeException) { bt = 0; }

                    for (int b = 0; b < 8; b++)
                    {
                        pixNum++;
                        if (pixNum > nPixels) // disregard pixels outside of the screen
                            break;
                        if (IsBigEndian)
                            j = (uint)(bt & (0x01 << b));
                        else
                            j = (uint)(bt & (0x80 >> b));
                            
                        y = (int)(pixNum / width);
                        x = (int)(pixNum % width);

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, j > 0 ? light : dark);
                    }
                }
                #endregion
            }
            g.DrawImage(bitmap, new Point(0, 0));
        }
        #endregion

        #region paint2BPP
        internal void paint2BPP(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            uint nNecessBytes = width * height;
            nNecessBytes = nNecessBytes / 4 + (uint)(nNecessBytes % 4 > 0 ? 1 : 0);

            uint bt, j;
            int pixNum = 0, nPixels = (int)(width * height);
            int x, y;

            Bitmap bitmap = new Bitmap((int)(width * Zoom), (int)(height * Zoom), PixelFormat.Format32bppArgb);

            Color[] palette = paletteData.getFullPaletteAsColor();

            if (Tiled)
            {
                #region tiled
                int ntx = (int)(width / TileSize.X); // amount of tiles horizontally
                int nty = (int)(height / TileSize.Y); // amount of tile in vertically
                int tx = 0, ty = 0; // x and y of tile
                int xintl = -1, yintl = 0; // x & y inside tile

                for (int i = 0; i < nNecessBytes; i++)
                {
                    try { bt = (uint)Data[Offset + i]; }
                    catch (IndexOutOfRangeException) { bt = 0; }

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
                for (int i = 0; i < nNecessBytes; i++)
                {
                    try { bt = (uint)Data[Offset + i]; }
                    catch (IndexOutOfRangeException) { bt = 0; }

                    for (int b = 0; b < 4; b++)
                    {
                        pixNum++;
                        if (pixNum >= nPixels)
                            break;
                        if (IsBigEndian)
                            j = (uint)((bt & (0x03 << (b * 2))) >> (b * 2));
                        else
                            j = (uint)((bt & (0xC0 >> (b * 2))) >> ((3 - b) * 2));
                            
                        x = (int)(pixNum % width);
                        y = (int)(pixNum / width);

                        for (int zy = 0; zy < Zoom; zy++)
                            for (int zx = 0; zx < Zoom; zx++)
                                bitmap.SetPixel(x + zx, y + zy, palette[j]);
                    }
                }
                #endregion
            }
            g.DrawImage(bitmap, new Point(0, 0));
        }
        #endregion

        #endregion

        internal override void copyToClipboard()
        {
            throw new Exception("The method GraphicsData.CopyToClipboard is not yet implemented.");
        }

        #region Method: getPixel(idx)
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
                    return this.paletteData.getPalette(idx, this.Data, this.Offset, (int)graphFormat, IsBigEndian);
                #endregion

                default: throw new Exception("Unkown error: invalid GraphicsFormat " + graphFormat.ToString());
            }
        }
        #endregion

        #region methods: increase/decrease width/height
        internal void increaseWidth() { this.Width += this.WidthSkipSize; }
        internal void decreaseWidth() { this.Width -= this.WidthSkipSize; }
        internal void increaseHeight() { this.Height += this.HeightSkipSize; }
        internal void decreaseHeight() { this.Height -= this.HeightSkipSize; }
        #endregion
    }

    #region Graphics enums (GraphicsFormat)
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
    #endregion
}
