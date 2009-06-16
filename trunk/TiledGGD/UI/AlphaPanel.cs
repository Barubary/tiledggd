using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD.UI
{
    public partial class AlphaPanel : Form
    {
        private AlphaSettings settings;

        public AlphaPanel(AlphaSettings settings)
        {
            InitializeComponent();

            string stretchtttxt = "Using stretch stretches alpha values to the range [0,255].\n"
                                + "The given minimum value will be mapped onto 0 (transparent),\n"
                                + "the given maximum value onto 255 (opaque).";
            this.stretch_tt.SetToolTip(this.stretch_group,  stretchtttxt);
            this.stretch_tt.SetToolTip(this.strech_enable, stretchtttxt);
            this.stretch_tt.SetToolTip(this.min_box, stretchtttxt);
            this.stretch_tt.SetToolTip(this.min_label, stretchtttxt);
            this.stretch_tt.SetToolTip(this.max_box, stretchtttxt);
            this.stretch_tt.SetToolTip(this.max_label, stretchtttxt);

            this.settings = settings;


            // set location
            switch (settings.Location)
            {
                case AlphaLocation.START: start_radio.Checked = true; break;
                case AlphaLocation.END: end_radio.Checked = true; break;
            }

            // set stretch
            this.strech_enable.Checked = settings.Stretch;
            this.min_box.Text = settings.Minimum + "";
            this.max_box.Text = settings.Maximum + "";

            // set ignore
            this.ignore_check.Checked = settings.IgnoreAlpha;
        }

        private void click_OK(object sender, EventArgs e)
        {
            this.Hide();

            // get location
            AlphaLocation loc;
            if (this.start_radio.Checked)
                loc = AlphaLocation.START;
            else if (this.end_radio.Checked)
                loc = AlphaLocation.END;
            else
            {
                MainWindow.showError("Unknown Alpha location");
                return;
            }

            // get stretch
            byte min, max;
            if (!byte.TryParse(min_box.Text, out min) || !byte.TryParse(max_box.Text, out max))
            {
                MainWindow.showError("Minimum and maximum values are only valid in the range [0,255].");
                return;
            }
            if (min > max)
            {
                MainWindow.showError("Minimum value cannot be larger than maximum value");
                return;
            }
            bool stretch = strech_enable.Checked;

            // get ignore
            bool ignore = ignore_check.Checked;

            this.settings.IgnoreAlpha = ignore;
            this.settings.Location = loc;
            this.settings.Maximum = max;
            this.settings.Minimum = min;
            this.settings.Stretch = stretch;

        }

        private void click_Cancel(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void checkEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                click_OK(sender, e);
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
            int val = int.Parse(newstr);
            val = Math.Min(255, Math.Max(0, val));
            newstr = val + "";
            (sender as TextBox).Text = newstr;
            (sender as TextBox).SelectionStart = newstr.Length;
        }

        private void autodetect_maximum(object sender, EventArgs e)
        {
            bool ignore = settings.IgnoreAlpha, stretch = settings.Stretch;
            settings.Stretch = false;
            settings.IgnoreAlpha = false;
            Color[] pal = MainWindow.PalData.getFullPaletteAsColor();
            byte max = 0;
            for(int i=0; i<pal.Length; i++)
                max = Math.Max(max, pal[i].A);
            this.max_box.Text = max + "";
            settings.IgnoreAlpha = ignore;
            settings.Stretch = stretch;
        }
    }
}