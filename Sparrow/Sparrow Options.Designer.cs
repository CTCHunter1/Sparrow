namespace Sparrow
{
    partial class Sparrow_Options
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.broadSpecUnitsComboBox = new System.Windows.Forms.ComboBox();
            this.narrowSpecUnitsComboBox = new System.Windows.Forms.ComboBox();
            this.numSlowTimePtsNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.resistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numSlowTimePtsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(11, 216);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(119, 216);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(78, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // broadSpecUnitsComboBox
            // 
            this.broadSpecUnitsComboBox.FormattingEnabled = true;
            this.broadSpecUnitsComboBox.Items.AddRange(new object[] {
            "V",
            "dBm"});
            this.broadSpecUnitsComboBox.Location = new System.Drawing.Point(12, 42);
            this.broadSpecUnitsComboBox.Name = "broadSpecUnitsComboBox";
            this.broadSpecUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.broadSpecUnitsComboBox.TabIndex = 2;
            // 
            // narrowSpecUnitsComboBox
            // 
            this.narrowSpecUnitsComboBox.FormattingEnabled = true;
            this.narrowSpecUnitsComboBox.Items.AddRange(new object[] {
            "V",
            "dBm"});
            this.narrowSpecUnitsComboBox.Location = new System.Drawing.Point(11, 91);
            this.narrowSpecUnitsComboBox.Name = "narrowSpecUnitsComboBox";
            this.narrowSpecUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.narrowSpecUnitsComboBox.TabIndex = 3;
            // 
            // numSlowTimePtsNumeric
            // 
            this.numSlowTimePtsNumeric.Location = new System.Drawing.Point(11, 177);
            this.numSlowTimePtsNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numSlowTimePtsNumeric.Name = "numSlowTimePtsNumeric";
            this.numSlowTimePtsNumeric.Size = new System.Drawing.Size(120, 20);
            this.numSlowTimePtsNumeric.TabIndex = 4;
            this.numSlowTimePtsNumeric.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Broad Spectral Units";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Narrow Spectral Units";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number of Slow Time Points";
            // 
            // resistanceNumeric
            // 
            this.resistanceNumeric.Location = new System.Drawing.Point(11, 138);
            this.resistanceNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.resistanceNumeric.Name = "resistanceNumeric";
            this.resistanceNumeric.Size = new System.Drawing.Size(120, 20);
            this.resistanceNumeric.TabIndex = 8;
            this.resistanceNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Resistance (Ohms)";
            // 
            // Sparrow_Options
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(220, 260);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resistanceNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numSlowTimePtsNumeric);
            this.Controls.Add(this.narrowSpecUnitsComboBox);
            this.Controls.Add(this.broadSpecUnitsComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Sparrow_Options";
            this.Text = "Sparrow Options";
            this.Shown += new System.EventHandler(this.View_Options_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numSlowTimePtsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox broadSpecUnitsComboBox;
        private System.Windows.Forms.ComboBox narrowSpecUnitsComboBox;
        private System.Windows.Forms.NumericUpDown numSlowTimePtsNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown resistanceNumeric;
        private System.Windows.Forms.Label label4;
    }
}