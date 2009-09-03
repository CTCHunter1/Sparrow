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
            this.numDownsampledPtsPow2Numeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.resistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numDownsapledPointsLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.downsampleFactorLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.downsampleFactorPow2Numeric = new System.Windows.Forms.NumericUpDown();
            this.numDecadesNumeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numDownsampledPtsPow2Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downsampleFactorPow2Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDecadesNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(11, 333);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(154, 333);
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
            "dBmV",
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
            "dBmV",
            "dBm"});
            this.narrowSpecUnitsComboBox.Location = new System.Drawing.Point(11, 91);
            this.narrowSpecUnitsComboBox.Name = "narrowSpecUnitsComboBox";
            this.narrowSpecUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.narrowSpecUnitsComboBox.TabIndex = 3;
            // 
            // numDownsampledPtsPow2Numeric
            // 
            this.numDownsampledPtsPow2Numeric.Location = new System.Drawing.Point(47, 187);
            this.numDownsampledPtsPow2Numeric.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numDownsampledPtsPow2Numeric.Name = "numDownsampledPtsPow2Numeric";
            this.numDownsampledPtsPow2Numeric.Size = new System.Drawing.Size(60, 20);
            this.numDownsampledPtsPow2Numeric.TabIndex = 4;
            this.numDownsampledPtsPow2Numeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numDownsampledPtsPow2Numeric.ValueChanged += new System.EventHandler(this.numDownsampledPtsPow2Numeric_ValueChanged);
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
            this.label3.Location = new System.Drawing.Point(151, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Downsampled Points";
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
            // numDownsapledPointsLabel
            // 
            this.numDownsapledPointsLabel.AutoSize = true;
            this.numDownsapledPointsLabel.Location = new System.Drawing.Point(161, 189);
            this.numDownsapledPointsLabel.Name = "numDownsapledPointsLabel";
            this.numDownsapledPointsLabel.Size = new System.Drawing.Size(19, 13);
            this.numDownsapledPointsLabel.TabIndex = 10;
            this.numDownsapledPointsLabel.Text = "32";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Downsampled Points Pow 2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 223);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Downsample Factor Pow 2";
            // 
            // downsampleFactorLabel
            // 
            this.downsampleFactorLabel.AutoSize = true;
            this.downsampleFactorLabel.Location = new System.Drawing.Point(161, 243);
            this.downsampleFactorLabel.Name = "downsampleFactorLabel";
            this.downsampleFactorLabel.Size = new System.Drawing.Size(19, 13);
            this.downsampleFactorLabel.TabIndex = 14;
            this.downsampleFactorLabel.Text = "32";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(151, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Downsample Factor";
            // 
            // downsampleFactorPow2Numeric
            // 
            this.downsampleFactorPow2Numeric.Location = new System.Drawing.Point(47, 241);
            this.downsampleFactorPow2Numeric.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.downsampleFactorPow2Numeric.Name = "downsampleFactorPow2Numeric";
            this.downsampleFactorPow2Numeric.Size = new System.Drawing.Size(60, 20);
            this.downsampleFactorPow2Numeric.TabIndex = 12;
            this.downsampleFactorPow2Numeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.downsampleFactorPow2Numeric.ValueChanged += new System.EventHandler(this.downsampleFactorPow2Numeric_ValueChanged);
            // 
            // numDecadesNumeric
            // 
            this.numDecadesNumeric.Location = new System.Drawing.Point(14, 294);
            this.numDecadesNumeric.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numDecadesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDecadesNumeric.Name = "numDecadesNumeric";
            this.numDecadesNumeric.Size = new System.Drawing.Size(120, 20);
            this.numDecadesNumeric.TabIndex = 16;
            this.numDecadesNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Downsampled Decades";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(169, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "per Decade";
            // 
            // Sparrow_Options
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(265, 368);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numDecadesNumeric);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.downsampleFactorLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.downsampleFactorPow2Numeric);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numDownsapledPointsLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resistanceNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numDownsampledPtsPow2Numeric);
            this.Controls.Add(this.narrowSpecUnitsComboBox);
            this.Controls.Add(this.broadSpecUnitsComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Sparrow_Options";
            this.Text = "Sparrow Options";
            this.Shown += new System.EventHandler(this.View_Options_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numDownsampledPtsPow2Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downsampleFactorPow2Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDecadesNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox broadSpecUnitsComboBox;
        private System.Windows.Forms.ComboBox narrowSpecUnitsComboBox;
        private System.Windows.Forms.NumericUpDown numDownsampledPtsPow2Numeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown resistanceNumeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label numDownsapledPointsLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label downsampleFactorLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown downsampleFactorPow2Numeric;
        private System.Windows.Forms.NumericUpDown numDecadesNumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
    }
}