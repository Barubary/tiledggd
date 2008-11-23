using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD
{
    public partial class MainWindow : Form
    {

        private static GraphicsData graphicsData;
        internal static GraphicsData GraphData { get { return graphicsData; } }
        private static PaletteData paletteData;
        internal static PaletteData PalData { get { return paletteData; } }
        private static DataPanelFiller datafiller;

        private static Font fnt;
        public override Font Font { get { return base.Font; } set { base.Font = value; fnt = value; } }
        public static Font MenuFont { get { return fnt; } }

        private static MainWindow mainWindow;

        private Size previousSize;

        public MainWindow()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            mainWindow = this;

            fnt = this.Font;

            GraphicsPanel.Paint += new PaintEventHandler(GraphicsPanel_Paint);
            PalettePanel.Paint += new PaintEventHandler(PalettePanel_Paint);
            DataPanel.Paint += new PaintEventHandler(DataPanel_Paint);

            datafiller = new DataPanelFiller(this.DataPanel);

            paletteData = new PaletteData(PaletteFormat.FORMAT_3BPP, PaletteOrder.ORDER_BGR);
            graphicsData = new GraphicsData(paletteData);
            GraphicsData.GraphFormat = GraphicsFormat.FORMAT_16BPP;
            GraphicsData.Tiled = false;
            GraphicsData.WidthSkipSize = 8;
            GraphicsData.Zoom = 2;

            

            //this.GraphicsPanel.Height = 1000;

            GraphicsPanel.DragEnter += new DragEventHandler(palGraphDragEnter);
            PalettePanel.DragEnter += new DragEventHandler(palGraphDragEnter);

            GraphicsPanel.DragDrop += new DragEventHandler(GraphicsPanel_DragDrop);
            PalettePanel.DragDrop += new DragEventHandler(PalettePanel_DragDrop);

            paletteData.load("D:/Sprites/Sonic/PAL1A.dat");
            graphicsData.load("D:/Sprites/Sonic/OVL1A.BIN");
            //paletteData.load("H:/PLT/DrScheme.exe");
            //graphicsData.load("H:/PLT/DrScheme.exe");
            paletteData.SkipSize = 3;

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            setupMenuActions();

            this.ResizeEnd += new EventHandler(ReconfigurePanels);

            this.previousSize = this.Size;

        }

        void ReconfigurePanels(object sender, EventArgs e)
        {
            int dw = this.Size.Width - this.previousSize.Width;

            this.PalettePanel.Location = new Point(this.PalettePanel.Location.X + dw, this.PalettePanel.Location.Y);
            this.DataPanel.Location = new Point(this.DataPanel.Location.X + dw, this.DataPanel.Location.Y);
            this.GraphicsPanel.Size = new Size(this.GraphicsPanel.Size.Width + dw, this.GraphicsPanel.Size.Height);

            this.previousSize = this.Size;
        }

        private void setupMenuActions()
        {
            this.quitToolStripMenuItem.Click += new EventHandler(Quit);
        }

        void Quit(object sender, EventArgs e)
        {
            this.Quit();
        }
        void Quit()
        {
            Application.Exit();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.P: paletteData.TogglePaletteOrder(); break;
                case Keys.N: paletteData.DoSkip(true); break;
                case Keys.M: paletteData.DoSkip(false); break;
                case Keys.Up: graphicsData.decreaseHeight(); break;
                case Keys.Down: graphicsData.increaseHeight(); break;
                case Keys.Left: graphicsData.decreaseWidth(); break;
                case Keys.Right: graphicsData.increaseWidth(); break;
                case Keys.Subtract: GraphicsData.Zoom /= 2; break;
                case Keys.Add: GraphicsData.Zoom *= 2; break;
                case Keys.PageDown: graphicsData.DoSkip(true); break;
                case Keys.PageUp: graphicsData.DoSkip(false); break;
                case Keys.B: graphicsData.toggleGraphicsFormat(); break;
                case Keys.F: graphicsData.toggleTiled(); break;
                case Keys.E: if (e.Control) graphicsData.toggleEndianness(); else if (e.Shift) paletteData.toggleEndianness(); break;
                case Keys.L: if (e.Control) graphicsData.toggleSkipSize(); else if (e.Shift) paletteData.toggleSkipSize(); break;
            }
            updateMenu();
        }

        #region method: updateMenu
        private void updateMenu()
        {
            // graphics format
            foreach (ToolStripMenuItem tsme in this.formatToolStripMenuItem.DropDownItems)
                tsme.Checked = false;
            (this.formatToolStripMenuItem.DropDownItems[(int)GraphicsData.GraphFormat - 1] as ToolStripMenuItem).Checked = true;

            // palette format
            foreach (ToolStripMenuItem tsme in this.toolStripMenuItem1.DropDownItems)
                tsme.Checked = false;
            (this.formatToolStripMenuItem1.DropDownItems[(int)PaletteData.PalFormat - 5] as ToolStripMenuItem).Checked = true;

            // graphics endianness
            littleEndianToolStripMenuItem.Checked = !(bigEndianToolStripMenuItem.Checked = GraphicsData.IsBigEndian);

            // palette endianness
            littleEndianToolStripMenuItem1.Checked = !(bigEndianToolStripMenuItem1.Checked = PaletteData.IsBigEndian);

            // graphics mode
            linearToolStripMenuItem.Checked = !(tiledToolStripMenuItem.Checked = GraphicsData.Tiled);

            // graphics skip size
            foreach (ToolStripMenuItem tsme in this.skipSizeToolStripMenuItem1.DropDownItems)
                tsme.Checked = false;
            switch (GraphicsData.SkipMetric)
            {
                case GraphicsSkipMetric.METRIC_BYTES:
                    switch (GraphicsData.SkipSize)
                    {
                        case 1: byteToolStripMenuItem1.Checked = true; break;
                        case 2: bytesToolStripMenuItem.Checked = true; break;
                        case 4: bytesToolStripMenuItem1.Checked = true; break;
                        default: throw new Exception("Unknown graphics skip size: " + GraphicsData.SkipSize + " bytes");
                    }
                    break;
                case GraphicsSkipMetric.METRIC_YPIX:
                    switch (GraphicsData.SkipSize)
                    {
                        case 1: pixelToolStripMenuItem.Checked = true; break;
                        case -1: tileRowToolStripMenuItem.Checked = true; break;
                        default: throw new Exception("Unknown graphics skip size: " + GraphicsData.SkipSize + " rows");
                    }
                    break;
                case GraphicsSkipMetric.METRIC_WIDTH: widthToolStripMenuItem.Checked = true; break;
                case GraphicsSkipMetric.METRIC_HEIGHT: heightRowsToolStripMenuItem.Checked = true; break;
                default: throw new Exception("Unknown graphics skip metric: " + GraphicsData.SkipMetric.ToString());
            }

            // palette alpha location
            endToolStripMenuItem.Checked = !(beginningToolStripMenuItem.Checked = (PaletteData.alphaLoc != AlphaLocation.END));

            // palette order
            foreach (ToolStripMenuItem tsme in colourOrderToolStripMenuItem.DropDownItems)
                tsme.Checked = false;
            (colourOrderToolStripMenuItem.DropDownItems[(int)PaletteData.PalOrder] as ToolStripMenuItem).Checked = true;

            // palette skip size
            foreach (ToolStripMenuItem tsme in this.skipSizeToolStripMenuItem.DropDownItems)
                tsme.Checked = false;
            switch (PaletteData.SkipMetric)
            {
                case PaletteSkipMetric.METRIC_BYTES:
                    switch (PaletteData.SkipSize)
                    {
                        case 1: byteToolStripMenuItem.Checked = true; break;
                        case 0x10000: kBytesToolStripMenuItem.Checked = true; break;
                        default: throw new Exception("Unknown palette skip size: " + PaletteData.SkipSize + " bytes");
                    }
                    break;
                case PaletteSkipMetric.METRIC_COLOURS:
                    switch (PaletteData.SkipSize)
                    {
                        case 1: pixelToolStripMenuItem.Checked = true; break;
                        case 16: pixelsToolStripMenuItem.Checked = true; break;
                        case 256: coloursToolStripMenuItem.Checked = true; break;
                        default: throw new Exception("Unknown palette skip size: " + PaletteData.SkipSize + " colours");
                    }
                    break;
                default: throw new Exception("Unknown palette skip metric: " + PaletteData.SkipMetric.ToString());
            }
        }
        #endregion

        void PalettePanel_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                paletteData.load(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
                DoRefresh();
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        void GraphicsPanel_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                graphicsData.load(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
                DoRefresh();
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        void palGraphDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        void DataPanel_Paint(object sender, PaintEventArgs e)
        {
            // TODO
        }

        void PalettePanel_Paint(object sender, PaintEventArgs e)
        {
            paletteData.paint(this, e);
        }

        void GraphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            graphicsData.paint(this, e);
        }

        /// <summary>
        /// Refresh the Main Window
        /// </summary>
        public static void DoRefresh()
        {
            mainWindow.Refresh();
            datafiller.refresh();
        }

        #region toolstrip response methods

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.aboutBox == null || this.aboutBox.IsDisposed)
                this.aboutBox = new AboutBox();
            this.aboutBox.Visible = true;
        }

        private void imbppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem tsme in this.formatToolStripMenuItem.DropDownItems)
                tsme.Checked = false;
            (sender as ToolStripMenuItem).Checked = true;

            if (sender == bitPerPixelToolStripMenuItem)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_1BPP;
            else if (sender == bitPerPixelToolStripMenuItem1)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_2BPP;
            else if (sender == bitPerPixelToolStripMenuItem2)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_4BPP;
            else if (sender == bitPerPixelToolStripMenuItem3)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_8BPP;
            else if (sender == bitPerPixelToolStripMenuItem4)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_16BPP;
            else if (sender == bitPerPixelToolStripMenuItem5)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_24BPP;
            else if (sender == bitPerPixelToolStripMenuItem6)
                GraphicsData.GraphFormat = GraphicsFormat.FORMAT_32BPP;
            else
                throw new Exception("Invalid Menu Item event");
            DoRefresh();
        }

        private void copyToClipboard(object sender, EventArgs e)
        {
            if (sender == copyGraphicsToolStripMenuItem)
                graphicsData.copyToClipboard();
            else if (sender == copyPaletteToolStripMenuItem)
                paletteData.copyToClipboard();
            else
                throw new Exception("Invalid Copy To Clipboard event");
        }

        private void linearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == linearToolStripMenuItem)
                GraphicsData.Tiled = false;
            else if (sender == tiledToolStripMenuItem)
                GraphicsData.Tiled = true;
            else
                throw new Exception("Invalid Linear/Tiled event");
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.controlShortBox == null || this.controlShortBox.IsDisposed)
                this.controlShortBox = new ControlShorts();
            this.controlShortBox.Visible = true;
        }

        #endregion

    }
}