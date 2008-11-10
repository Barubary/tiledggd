namespace TiledGGD
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGraphicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataPanel = new System.Windows.Forms.Panel();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPerPixelToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GraphicsPanel = new TiledGGD.DoubleBufferedPanel();
            this.PalettePanel = new TiledGGD.DoubleBufferedPanel();
            this.linearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tiledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bytesPerColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bytesPerColourToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bytesPerColourToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.alphaLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beginningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colourOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bGRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gRBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bRGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.imageToolStripMenuItem,
            this.paletteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveGraphicsToolStripMenuItem,
            this.toolStripSeparator2,
            this.openPaletteToolStripMenuItem,
            this.savePaletteToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // saveGraphicsToolStripMenuItem
            // 
            this.saveGraphicsToolStripMenuItem.Name = "saveGraphicsToolStripMenuItem";
            this.saveGraphicsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveGraphicsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.saveGraphicsToolStripMenuItem.Text = "Save Graphics...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(223, 6);
            // 
            // openPaletteToolStripMenuItem
            // 
            this.openPaletteToolStripMenuItem.Name = "openPaletteToolStripMenuItem";
            this.openPaletteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.openPaletteToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.openPaletteToolStripMenuItem.Text = "Open Palette...";
            // 
            // savePaletteToolStripMenuItem
            // 
            this.savePaletteToolStripMenuItem.Name = "savePaletteToolStripMenuItem";
            this.savePaletteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.savePaletteToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.savePaletteToolStripMenuItem.Text = "Save Palette...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatToolStripMenuItem,
            this.modeToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // DataPanel
            // 
            this.DataPanel.Location = new System.Drawing.Point(516, 27);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new System.Drawing.Size(256, 263);
            this.DataPanel.TabIndex = 2;
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bitPerPixelToolStripMenuItem,
            this.bitPerPixelToolStripMenuItem1,
            this.bitPerPixelToolStripMenuItem2,
            this.bitPerPixelToolStripMenuItem3,
            this.bitPerPixelToolStripMenuItem4,
            this.bitPerPixelToolStripMenuItem5,
            this.bitPerPixelToolStripMenuItem6});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // bitPerPixelToolStripMenuItem
            // 
            this.bitPerPixelToolStripMenuItem.Name = "bitPerPixelToolStripMenuItem";
            this.bitPerPixelToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem.Text = "1 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem1
            // 
            this.bitPerPixelToolStripMenuItem1.Name = "bitPerPixelToolStripMenuItem1";
            this.bitPerPixelToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem1.Text = "2 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem2
            // 
            this.bitPerPixelToolStripMenuItem2.Name = "bitPerPixelToolStripMenuItem2";
            this.bitPerPixelToolStripMenuItem2.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem2.Text = "4 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem3
            // 
            this.bitPerPixelToolStripMenuItem3.Name = "bitPerPixelToolStripMenuItem3";
            this.bitPerPixelToolStripMenuItem3.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem3.Text = "8 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem4
            // 
            this.bitPerPixelToolStripMenuItem4.Name = "bitPerPixelToolStripMenuItem4";
            this.bitPerPixelToolStripMenuItem4.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem4.Text = "16 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem5
            // 
            this.bitPerPixelToolStripMenuItem5.Name = "bitPerPixelToolStripMenuItem5";
            this.bitPerPixelToolStripMenuItem5.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem5.Text = "24 Bit per pixel";
            // 
            // bitPerPixelToolStripMenuItem6
            // 
            this.bitPerPixelToolStripMenuItem6.Name = "bitPerPixelToolStripMenuItem6";
            this.bitPerPixelToolStripMenuItem6.Size = new System.Drawing.Size(150, 22);
            this.bitPerPixelToolStripMenuItem6.Text = "32 Bit per pixel";
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linearToolStripMenuItem,
            this.tiledToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // GraphicsPanel
            // 
            this.GraphicsPanel.AllowDrop = true;
            this.GraphicsPanel.AutoScroll = true;
            this.GraphicsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GraphicsPanel.Location = new System.Drawing.Point(0, 27);
            this.GraphicsPanel.Name = "GraphicsPanel";
            this.GraphicsPanel.Size = new System.Drawing.Size(510, 525);
            this.GraphicsPanel.TabIndex = 3;
            // 
            // PalettePanel
            // 
            this.PalettePanel.Location = new System.Drawing.Point(516, 296);
            this.PalettePanel.Name = "PalettePanel";
            this.PalettePanel.Size = new System.Drawing.Size(256, 256);
            this.PalettePanel.TabIndex = 1;
            // 
            // linearToolStripMenuItem
            // 
            this.linearToolStripMenuItem.Name = "linearToolStripMenuItem";
            this.linearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.linearToolStripMenuItem.Text = "Linear";
            // 
            // tiledToolStripMenuItem
            // 
            this.tiledToolStripMenuItem.Name = "tiledToolStripMenuItem";
            this.tiledToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tiledToolStripMenuItem.Text = "Tiled";
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatToolStripMenuItem1,
            this.alphaLocationToolStripMenuItem,
            this.colourOrderToolStripMenuItem});
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.paletteToolStripMenuItem.Text = "Palette";
            // 
            // formatToolStripMenuItem1
            // 
            this.formatToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bytesPerColourToolStripMenuItem,
            this.bytesPerColourToolStripMenuItem1,
            this.bytesPerColourToolStripMenuItem2});
            this.formatToolStripMenuItem1.Name = "formatToolStripMenuItem1";
            this.formatToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.formatToolStripMenuItem1.Text = "Format";
            // 
            // bytesPerColourToolStripMenuItem
            // 
            this.bytesPerColourToolStripMenuItem.Name = "bytesPerColourToolStripMenuItem";
            this.bytesPerColourToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.bytesPerColourToolStripMenuItem.Text = "2 Bytes per colour";
            // 
            // bytesPerColourToolStripMenuItem1
            // 
            this.bytesPerColourToolStripMenuItem1.Name = "bytesPerColourToolStripMenuItem1";
            this.bytesPerColourToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.bytesPerColourToolStripMenuItem1.Text = "3 Bytes per colour";
            // 
            // bytesPerColourToolStripMenuItem2
            // 
            this.bytesPerColourToolStripMenuItem2.Name = "bytesPerColourToolStripMenuItem2";
            this.bytesPerColourToolStripMenuItem2.Size = new System.Drawing.Size(168, 22);
            this.bytesPerColourToolStripMenuItem2.Text = "4 Bytes per colour";
            // 
            // alphaLocationToolStripMenuItem
            // 
            this.alphaLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.endToolStripMenuItem,
            this.beginningToolStripMenuItem});
            this.alphaLocationToolStripMenuItem.Name = "alphaLocationToolStripMenuItem";
            this.alphaLocationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.alphaLocationToolStripMenuItem.Text = "Alpha location";
            // 
            // endToolStripMenuItem
            // 
            this.endToolStripMenuItem.Name = "endToolStripMenuItem";
            this.endToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.endToolStripMenuItem.Text = "End";
            // 
            // beginningToolStripMenuItem
            // 
            this.beginningToolStripMenuItem.Name = "beginningToolStripMenuItem";
            this.beginningToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.beginningToolStripMenuItem.Text = "Beginning";
            // 
            // colourOrderToolStripMenuItem
            // 
            this.colourOrderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bGRToolStripMenuItem,
            this.rGBToolStripMenuItem,
            this.rBGToolStripMenuItem,
            this.gRBToolStripMenuItem,
            this.gBRToolStripMenuItem,
            this.bRGToolStripMenuItem});
            this.colourOrderToolStripMenuItem.Name = "colourOrderToolStripMenuItem";
            this.colourOrderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colourOrderToolStripMenuItem.Text = "Colour order";
            // 
            // bGRToolStripMenuItem
            // 
            this.bGRToolStripMenuItem.Name = "bGRToolStripMenuItem";
            this.bGRToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bGRToolStripMenuItem.Text = "BGR";
            // 
            // rGBToolStripMenuItem
            // 
            this.rGBToolStripMenuItem.Name = "rGBToolStripMenuItem";
            this.rGBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rGBToolStripMenuItem.Text = "RGB";
            // 
            // rBGToolStripMenuItem
            // 
            this.rBGToolStripMenuItem.Name = "rBGToolStripMenuItem";
            this.rBGToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rBGToolStripMenuItem.Text = "RBG";
            // 
            // gRBToolStripMenuItem
            // 
            this.gRBToolStripMenuItem.Name = "gRBToolStripMenuItem";
            this.gRBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gRBToolStripMenuItem.Text = "GRB";
            // 
            // gBRToolStripMenuItem
            // 
            this.gBRToolStripMenuItem.Name = "gBRToolStripMenuItem";
            this.gBRToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gBRToolStripMenuItem.Text = "GBR";
            // 
            // bRGToolStripMenuItem
            // 
            this.bRGToolStripMenuItem.Name = "bRGToolStripMenuItem";
            this.bRGToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bRGToolStripMenuItem.Text = "BRG";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this.GraphicsPanel);
            this.Controls.Add(this.DataPanel);
            this.Controls.Add(this.PalettePanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGraphicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem openPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private DoubleBufferedPanel PalettePanel;
        private System.Windows.Forms.Panel DataPanel;
        private DoubleBufferedPanel GraphicsPanel;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem bitPerPixelToolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tiledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bytesPerColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bytesPerColourToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bytesPerColourToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem alphaLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beginningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colourOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bGRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rBGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gRBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gBRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bRGToolStripMenuItem;
    }
}

