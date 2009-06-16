using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD.UI
{
    public partial class TileSizeDialog : Form
    {
        private Point currentTileSize;

        public Point NewTileSize
        {
            get
            {
                if (!accepted)
                    return currentTileSize;
                else
                {
                    Point p = new Point();
                    p.X = int.Parse(this.textBox1.Text);
                    p.Y = int.Parse(this.textBox2.Text);
                    return p;
                }
            }
        }

        private bool accepted = false;
        
        public TileSizeDialog(Point currentTileSize)
        {
            InitializeComponent();

            this.currentTileSize = currentTileSize;
            this.textBox1.Text = currentTileSize.X + "";
            this.textBox2.Text = currentTileSize.Y + "";
        }

        private void click_OK(object sender, EventArgs e)
        {
            accepted = true;
            this.Hide();
        }
        private void click_Cancel(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void checkText(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            string newstr = "";
            foreach (char c in str)
                if (char.IsDigit(c))
                    newstr += c;
            if (newstr.Length == 0)
                newstr = "0";
            while (newstr[0] == '0' && newstr.Length > 1)
                newstr = newstr.Substring(1);
            if (newstr.Length == 0)
                newstr = "0";
            (sender as TextBox).Text = newstr;
            (sender as TextBox).SelectionStart = newstr.Length;
        }

        private void checkEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                click_OK(sender, e);
        }
    }
}