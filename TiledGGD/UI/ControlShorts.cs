using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD.UI
{
    public partial class ControlShorts : Form
    {
        public ControlShorts()
        {
            InitializeComponent();

            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            this.dataGridView1.Rows.Add("Toggle Graphics Format", "B");
            this.dataGridView1.Rows.Add("Toggle Palette Format", "O");
            this.dataGridView1.Rows.Add("Toggle Palette Order", "P");
            this.dataGridView1.Rows.Add("Toggle Tiled/Linear for graphics", "F");
            this.dataGridView1.Rows.Add("Toggle Tiled/Linear for palette", "Shift+F");
            this.dataGridView1.Rows.Add("Skip graphics data forward", "PageDown");
            this.dataGridView1.Rows.Add("Skip graphics data backward", "PageUp");
            this.dataGridView1.Rows.Add("Skip palette forward", "End");
            this.dataGridView1.Rows.Add("Skip palette backward", "Home");
            this.dataGridView1.Rows.Add("Increase graphics height", "Down");
            this.dataGridView1.Rows.Add("Decrease graphics height", "Up");
            this.dataGridView1.Rows.Add("Increase graphics width", "Right");
            this.dataGridView1.Rows.Add("Decrease graphics width", "Left");
            this.dataGridView1.Rows.Add("Toggle Width Skip Size", "W");
            this.dataGridView1.Rows.Add("Toggle Height Skip Size", "H");
            this.dataGridView1.Rows.Add("Zoom graphics out", "-");
            this.dataGridView1.Rows.Add("Zoom graphics out", "+");
            this.dataGridView1.Rows.Add("Toggle Graphical Endianness", "Ctrl+E");
            this.dataGridView1.Rows.Add("Toggle Palette Endianness", "Shift+E");
            this.dataGridView1.Rows.Add("Toggle Graphics Skip size", "Ctrl+Z");
            this.dataGridView1.Rows.Add("Toggle Palette Skip size", "Shift+Z");

        }
    }
}