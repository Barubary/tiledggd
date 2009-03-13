using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD
{
    public partial class GoToOffsetDialog : Form
    {
        public GoToOffsetDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the result of the input
        /// </summary>
        /// <param name="relative">If the offset is relative. Absolute otherwise</param>
        /// <param name="offset">The offset in bytes</param>
        /// <returns><code>true</code> iff  the input is proper</returns>
        internal bool getResult(out bool relative, out long offset)
        {
            relative = false;
            offset = 0;
            if (this.textBox1.Text.Length == 0)
                return false;

            relative = this.radioBtn_rel.Checked;
            if (relative == this.radioBtn_abs.Checked)
                throw new Exception("Highly improbable Exception");

            if (this.radioBtn_dec.Checked)
            {
                if (long.TryParse(this.textBox1.Text, out offset))
                    return true;
                else
                    return false; // user entered an invalid decimal number
            }
            try
            {
                offset = long.Parse(this.textBox1.Text, System.Globalization.NumberStyles.HexNumber);
                return true;
            }
            catch (Exception)
            {
                return false; // user entered an invalid hexadecimal number
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = ""; // make sure to ignore the input
            this.Hide();
        }

        private void refocusTextbox(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void textBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button_OK_Click(sender, e);
        }
    }
}