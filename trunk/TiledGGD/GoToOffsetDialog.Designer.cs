namespace TiledGGD
{
    partial class GoToOffsetDialog
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_bytes = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.groupBox_type = new System.Windows.Forms.GroupBox();
            this.radioBtn_hex = new System.Windows.Forms.RadioButton();
            this.radioBtn_dec = new System.Windows.Forms.RadioButton();
            this.groupBox_mode = new System.Windows.Forms.GroupBox();
            this.radioBtn_abs = new System.Windows.Forms.RadioButton();
            this.radioBtn_rel = new System.Windows.Forms.RadioButton();
            this.groupBox_type.SuspendLayout();
            this.groupBox_mode.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 86);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxKeyDown);
            // 
            // label_bytes
            // 
            this.label_bytes.AutoSize = true;
            this.label_bytes.Location = new System.Drawing.Point(29, 89);
            this.label_bytes.Name = "label_bytes";
            this.label_bytes.Size = new System.Drawing.Size(36, 13);
            this.label_bytes.TabIndex = 5;
            this.label_bytes.Text = "Bytes:";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(42, 112);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(63, 23);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "Go";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(117, 112);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(66, 23);
            this.button_Cancel.TabIndex = 7;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // groupBox_type
            // 
            this.groupBox_type.Controls.Add(this.radioBtn_hex);
            this.groupBox_type.Controls.Add(this.radioBtn_dec);
            this.groupBox_type.Location = new System.Drawing.Point(12, 12);
            this.groupBox_type.Name = "groupBox_type";
            this.groupBox_type.Size = new System.Drawing.Size(98, 68);
            this.groupBox_type.TabIndex = 8;
            this.groupBox_type.TabStop = false;
            this.groupBox_type.Text = "Input type";
            // 
            // radioBtn_hex
            // 
            this.radioBtn_hex.AutoSize = true;
            this.radioBtn_hex.Checked = true;
            this.radioBtn_hex.Location = new System.Drawing.Point(7, 43);
            this.radioBtn_hex.Name = "radioBtn_hex";
            this.radioBtn_hex.Size = new System.Drawing.Size(86, 17);
            this.radioBtn_hex.TabIndex = 1;
            this.radioBtn_hex.TabStop = true;
            this.radioBtn_hex.Text = "Hexadecimal";
            this.radioBtn_hex.UseVisualStyleBackColor = true;
            this.radioBtn_hex.Click += new System.EventHandler(this.refocusTextbox);
            // 
            // radioBtn_dec
            // 
            this.radioBtn_dec.AutoSize = true;
            this.radioBtn_dec.Location = new System.Drawing.Point(6, 19);
            this.radioBtn_dec.Name = "radioBtn_dec";
            this.radioBtn_dec.Size = new System.Drawing.Size(63, 17);
            this.radioBtn_dec.TabIndex = 0;
            this.radioBtn_dec.Text = "Decimal";
            this.radioBtn_dec.UseVisualStyleBackColor = true;
            this.radioBtn_dec.Click += new System.EventHandler(this.refocusTextbox);
            // 
            // groupBox_mode
            // 
            this.groupBox_mode.Controls.Add(this.radioBtn_abs);
            this.groupBox_mode.Controls.Add(this.radioBtn_rel);
            this.groupBox_mode.Location = new System.Drawing.Point(117, 13);
            this.groupBox_mode.Name = "groupBox_mode";
            this.groupBox_mode.Size = new System.Drawing.Size(86, 67);
            this.groupBox_mode.TabIndex = 9;
            this.groupBox_mode.TabStop = false;
            this.groupBox_mode.Text = "Mode";
            // 
            // radioBtn_abs
            // 
            this.radioBtn_abs.AutoSize = true;
            this.radioBtn_abs.Checked = true;
            this.radioBtn_abs.Location = new System.Drawing.Point(7, 42);
            this.radioBtn_abs.Name = "radioBtn_abs";
            this.radioBtn_abs.Size = new System.Drawing.Size(66, 17);
            this.radioBtn_abs.TabIndex = 1;
            this.radioBtn_abs.TabStop = true;
            this.radioBtn_abs.Text = "Absolute";
            this.radioBtn_abs.UseVisualStyleBackColor = true;
            this.radioBtn_abs.Click += new System.EventHandler(this.refocusTextbox);
            // 
            // radioBtn_rel
            // 
            this.radioBtn_rel.AutoSize = true;
            this.radioBtn_rel.Location = new System.Drawing.Point(7, 20);
            this.radioBtn_rel.Name = "radioBtn_rel";
            this.radioBtn_rel.Size = new System.Drawing.Size(64, 17);
            this.radioBtn_rel.TabIndex = 0;
            this.radioBtn_rel.Text = "Relative";
            this.radioBtn_rel.UseVisualStyleBackColor = true;
            this.radioBtn_rel.Click += new System.EventHandler(this.refocusTextbox);
            // 
            // GoToOffsetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 141);
            this.Controls.Add(this.groupBox_mode);
            this.Controls.Add(this.groupBox_type);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label_bytes);
            this.Controls.Add(this.textBox1);
            this.Name = "GoToOffsetDialog";
            this.Text = "Go To Offset ...";
            this.groupBox_type.ResumeLayout(false);
            this.groupBox_type.PerformLayout();
            this.groupBox_mode.ResumeLayout(false);
            this.groupBox_mode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_bytes;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.GroupBox groupBox_type;
        private System.Windows.Forms.RadioButton radioBtn_hex;
        private System.Windows.Forms.RadioButton radioBtn_dec;
        private System.Windows.Forms.GroupBox groupBox_mode;
        private System.Windows.Forms.RadioButton radioBtn_abs;
        private System.Windows.Forms.RadioButton radioBtn_rel;
    }
}