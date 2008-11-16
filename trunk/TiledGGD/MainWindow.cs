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

        private static Font fnt;
        public override Font Font { get { return base.Font; } set { base.Font = value; fnt = value; } }
        public static Font MenuFont { get { return fnt; } }

        private static MainWindow mainWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            mainWindow = this;

            fnt = this.Font;

            GraphicsPanel.Paint += new PaintEventHandler(GraphicsPanel_Paint);
            PalettePanel.Paint += new PaintEventHandler(PalettePanel_Paint);
            DataPanel.Paint += new PaintEventHandler(DataPanel_Paint);

            paletteData = new PaletteData(PaletteFormat.FORMAT_3BPP, PaletteOrder.ORDER_BGR);
            graphicsData = new GraphicsData(paletteData);
            GraphicsData.GraphFormat = GraphicsFormat.FORMAT_4BPP;
            graphicsData.Tiled = false;
            graphicsData.WidthSkipSize = 8;
            graphicsData.Zoom = 2;

            //this.GraphicsPanel.Height = 1000;

            GraphicsPanel.DragEnter += new DragEventHandler(palGraphDragEnter);
            PalettePanel.DragEnter += new DragEventHandler(palGraphDragEnter);

            GraphicsPanel.DragDrop += new DragEventHandler(GraphicsPanel_DragDrop);
            PalettePanel.DragDrop += new DragEventHandler(PalettePanel_DragDrop);

            paletteData.load("D:/Sprites/Sonic/PAL1A.dat");
            graphicsData.load("D:/Sprites/Sonic/OVL1A.BIN");
            paletteData.SkipSize = 3;

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            setupMenuActions();

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
                case Keys.C: if (e.Control) graphicsData.copyToClipboard(); else if (e.Alt) paletteData.copyToClipboard(); break;
                case Keys.Up: graphicsData.decreaseHeight(); break;
                case Keys.Down: graphicsData.increaseHeight(); break;
                case Keys.Left: graphicsData.decreaseWidth(); break;
                case Keys.Right: graphicsData.increaseWidth(); break;
                case Keys.Subtract: graphicsData.Zoom /= 2; break;
                case Keys.Add: graphicsData.Zoom *= 2; break;
                case Keys.PageDown: graphicsData.DoSkip(true); break;
                case Keys.PageUp: graphicsData.DoSkip(false); break;
            }

        }

        void PalettePanel_DragDrop(object sender, DragEventArgs e)
        {
            paletteData.load(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
        }

        void GraphicsPanel_DragDrop(object sender, DragEventArgs e)
        {
            graphicsData.load(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
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
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.aboutBox.IsDisposed)
                this.aboutBox = new AboutBox();
            this.aboutBox.Visible = true;
        }
        
    }
}