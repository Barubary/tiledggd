namespace TiledGGD.UI
{
    partial class AlphaPanel
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
            this.components = new System.ComponentModel.Container();
            this.stretch_group = new System.Windows.Forms.GroupBox();
            this.strech_enable = new System.Windows.Forms.CheckBox();
            this.max_label = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.min_label = new System.Windows.Forms.Label();
            this.max_box = new System.Windows.Forms.TextBox();
            this.min_box = new System.Windows.Forms.TextBox();
            this.location_group = new System.Windows.Forms.GroupBox();
            this.end_radio = new System.Windows.Forms.RadioButton();
            this.start_radio = new System.Windows.Forms.RadioButton();
            this.ignore_group = new System.Windows.Forms.GroupBox();
            this.ignore_check = new System.Windows.Forms.CheckBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.stretch_tt = new System.Windows.Forms.ToolTip(this.components);
            this.stretch_group.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.location_group.SuspendLayout();
            this.ignore_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // stretch_group
            // 
            this.stretch_group.Controls.Add(this.strech_enable);
            this.stretch_group.Controls.Add(this.max_label);
            this.stretch_group.Controls.Add(this.min_label);
            this.stretch_group.Controls.Add(this.max_box);
            this.stretch_group.Controls.Add(this.min_box);
            this.stretch_group.Location = new System.Drawing.Point(12, 86);
            this.stretch_group.Name = "stretch_group";
            this.stretch_group.Size = new System.Drawing.Size(214, 65);
            this.stretch_group.TabIndex = 0;
            this.stretch_group.TabStop = false;
            this.stretch_group.Text = "Alpha Stretch";
            // 
            // strech_enable
            // 
            this.strech_enable.AutoSize = true;
            this.strech_enable.Location = new System.Drawing.Point(6, 19);
            this.strech_enable.Name = "strech_enable";
            this.strech_enable.Size = new System.Drawing.Size(59, 17);
            this.strech_enable.TabIndex = 4;
            this.strech_enable.Text = "Enable";
            this.strech_enable.UseVisualStyleBackColor = true;
            // 
            // max_label
            // 
            this.max_label.AutoSize = true;
            this.max_label.ContextMenuStrip = this.contextMenuStrip1;
            this.max_label.Location = new System.Drawing.Point(103, 39);
            this.max_label.Name = "max_label";
            this.max_label.Size = new System.Drawing.Size(51, 13);
            this.max_label.TabIndex = 3;
            this.max_label.Text = "Maximum";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(242, 48);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.testToolStripMenuItem.Text = "Set To Current Maximum Alpha";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.autodetect_maximum);
            // 
            // min_label
            // 
            this.min_label.AutoSize = true;
            this.min_label.Location = new System.Drawing.Point(3, 39);
            this.min_label.Name = "min_label";
            this.min_label.Size = new System.Drawing.Size(48, 13);
            this.min_label.TabIndex = 2;
            this.min_label.Text = "Minimum";
            // 
            // max_box
            // 
            this.max_box.ContextMenuStrip = this.contextMenuStrip1;
            this.max_box.Location = new System.Drawing.Point(160, 36);
            this.max_box.Name = "max_box";
            this.max_box.Size = new System.Drawing.Size(40, 20);
            this.max_box.TabIndex = 1;
            this.max_box.TextChanged += new System.EventHandler(this.checkText);
            this.max_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkEnter);
            // 
            // min_box
            // 
            this.min_box.Location = new System.Drawing.Point(57, 36);
            this.min_box.Name = "min_box";
            this.min_box.Size = new System.Drawing.Size(40, 20);
            this.min_box.TabIndex = 0;
            this.min_box.TextChanged += new System.EventHandler(this.checkText);
            this.min_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkEnter);
            // 
            // location_group
            // 
            this.location_group.Controls.Add(this.end_radio);
            this.location_group.Controls.Add(this.start_radio);
            this.location_group.Location = new System.Drawing.Point(12, 12);
            this.location_group.Name = "location_group";
            this.location_group.Size = new System.Drawing.Size(214, 68);
            this.location_group.TabIndex = 1;
            this.location_group.TabStop = false;
            this.location_group.Text = "Alpha Location";
            // 
            // end_radio
            // 
            this.end_radio.AutoSize = true;
            this.end_radio.Location = new System.Drawing.Point(7, 44);
            this.end_radio.Name = "end_radio";
            this.end_radio.Size = new System.Drawing.Size(44, 17);
            this.end_radio.TabIndex = 1;
            this.end_radio.TabStop = true;
            this.end_radio.Text = "End";
            this.end_radio.UseVisualStyleBackColor = true;
            // 
            // start_radio
            // 
            this.start_radio.AutoSize = true;
            this.start_radio.Location = new System.Drawing.Point(7, 20);
            this.start_radio.Name = "start_radio";
            this.start_radio.Size = new System.Drawing.Size(47, 17);
            this.start_radio.TabIndex = 0;
            this.start_radio.TabStop = true;
            this.start_radio.Text = "Start";
            this.start_radio.UseVisualStyleBackColor = true;
            // 
            // ignore_group
            // 
            this.ignore_group.Controls.Add(this.ignore_check);
            this.ignore_group.Location = new System.Drawing.Point(12, 158);
            this.ignore_group.Name = "ignore_group";
            this.ignore_group.Size = new System.Drawing.Size(214, 36);
            this.ignore_group.TabIndex = 2;
            this.ignore_group.TabStop = false;
            // 
            // ignore_check
            // 
            this.ignore_check.AutoSize = true;
            this.ignore_check.Location = new System.Drawing.Point(7, 13);
            this.ignore_check.Name = "ignore_check";
            this.ignore_check.Size = new System.Drawing.Size(115, 17);
            this.ignore_check.TabIndex = 0;
            this.ignore_check.Text = "Ignore Alpha value";
            this.ignore_check.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.Location = new System.Drawing.Point(19, 201);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 3;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.click_OK);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(137, 201);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.click_Cancel);
            // 
            // AlphaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 230);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.ignore_group);
            this.Controls.Add(this.location_group);
            this.Controls.Add(this.stretch_group);
            this.Name = "AlphaPanel";
            this.Text = "Alpha Settings";
            this.stretch_group.ResumeLayout(false);
            this.stretch_group.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.location_group.ResumeLayout(false);
            this.location_group.PerformLayout();
            this.ignore_group.ResumeLayout(false);
            this.ignore_group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox stretch_group;
        private System.Windows.Forms.Label min_label;
        private System.Windows.Forms.TextBox max_box;
        private System.Windows.Forms.TextBox min_box;
        private System.Windows.Forms.CheckBox strech_enable;
        private System.Windows.Forms.Label max_label;
        private System.Windows.Forms.GroupBox location_group;
        private System.Windows.Forms.RadioButton end_radio;
        private System.Windows.Forms.RadioButton start_radio;
        private System.Windows.Forms.GroupBox ignore_group;
        private System.Windows.Forms.CheckBox ignore_check;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ToolTip stretch_tt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    }
}