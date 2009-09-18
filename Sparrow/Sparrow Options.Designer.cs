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
            this.timeDec1Numeric = new System.Windows.Forms.NumericUpDown();
            this.timeDec2Numeric = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.fftAveragingCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.singleShotNumPointsLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.singleShotNumberPointsPow2Numeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numDownsampledPtsPow2Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downsampleFactorPow2Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDecadesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeDec1Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeDec2Numeric)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singleShotNumberPointsPow2Numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(15, 563);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(158, 563);
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
            this.broadSpecUnitsComboBox.Location = new System.Drawing.Point(27, 36);
            this.broadSpecUnitsComboBox.Name = "broadSpecUnitsComboBox";
            this.broadSpecUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.broadSpecUnitsComboBox.TabIndex = 2;
            // 
            // numDownsampledPtsPow2Numeric
            // 
            this.numDownsampledPtsPow2Numeric.Location = new System.Drawing.Point(9, 171);
            this.numDownsampledPtsPow2Numeric.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numDownsampledPtsPow2Numeric.Name = "numDownsampledPtsPow2Numeric";
            this.numDownsampledPtsPow2Numeric.Size = new System.Drawing.Size(60, 20);
            this.numDownsampledPtsPow2Numeric.TabIndex = 4;
            this.numDownsampledPtsPow2Numeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDownsampledPtsPow2Numeric.ValueChanged += new System.EventHandler(this.numDownsampledPtsPow2Numeric_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Broad Spectral Units";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Time Decade Graph 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Downsampled Points";
            // 
            // resistanceNumeric
            // 
            this.resistanceNumeric.Location = new System.Drawing.Point(9, 122);
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
            this.label4.Location = new System.Drawing.Point(9, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Resistance (Ohms)";
            // 
            // numDownsapledPointsLabel
            // 
            this.numDownsapledPointsLabel.AutoSize = true;
            this.numDownsapledPointsLabel.Location = new System.Drawing.Point(156, 173);
            this.numDownsapledPointsLabel.Name = "numDownsapledPointsLabel";
            this.numDownsapledPointsLabel.Size = new System.Drawing.Size(19, 13);
            this.numDownsapledPointsLabel.TabIndex = 10;
            this.numDownsapledPointsLabel.Text = "32";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Downsampled Points Pow 2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Downsample Factor Pow 2";
            // 
            // downsampleFactorLabel
            // 
            this.downsampleFactorLabel.AutoSize = true;
            this.downsampleFactorLabel.Location = new System.Drawing.Point(156, 227);
            this.downsampleFactorLabel.Name = "downsampleFactorLabel";
            this.downsampleFactorLabel.Size = new System.Drawing.Size(19, 13);
            this.downsampleFactorLabel.TabIndex = 14;
            this.downsampleFactorLabel.Text = "32";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(156, 207);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Downsample Factor";
            // 
            // downsampleFactorPow2Numeric
            // 
            this.downsampleFactorPow2Numeric.Location = new System.Drawing.Point(9, 225);
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
            this.numDecadesNumeric.Location = new System.Drawing.Point(9, 278);
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
            3,
            0,
            0,
            0});
            this.numDecadesNumeric.ValueChanged += new System.EventHandler(this.numDecadesNumeric_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Downsampled Decades";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "per Decade";
            // 
            // timeDec1Numeric
            // 
            this.timeDec1Numeric.Location = new System.Drawing.Point(9, 32);
            this.timeDec1Numeric.Name = "timeDec1Numeric";
            this.timeDec1Numeric.Size = new System.Drawing.Size(40, 20);
            this.timeDec1Numeric.TabIndex = 19;
            // 
            // timeDec2Numeric
            // 
            this.timeDec2Numeric.Location = new System.Drawing.Point(9, 74);
            this.timeDec2Numeric.Name = "timeDec2Numeric";
            this.timeDec2Numeric.Size = new System.Drawing.Size(40, 20);
            this.timeDec2Numeric.TabIndex = 21;
            this.timeDec2Numeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Time Decade Graph 2";
            // 
            // fftAveragingCheckBox
            // 
            this.fftAveragingCheckBox.AutoSize = true;
            this.fftAveragingCheckBox.Checked = true;
            this.fftAveragingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fftAveragingCheckBox.Location = new System.Drawing.Point(9, 313);
            this.fftAveragingCheckBox.Name = "fftAveragingCheckBox";
            this.fftAveragingCheckBox.Size = new System.Drawing.Size(96, 17);
            this.fftAveragingCheckBox.TabIndex = 22;
            this.fftAveragingCheckBox.Text = "FFT Averaging";
            this.fftAveragingCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fftAveragingCheckBox);
            this.groupBox1.Controls.Add(this.timeDec2Numeric);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.timeDec1Numeric);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numDecadesNumeric);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.downsampleFactorLabel);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.downsampleFactorPow2Numeric);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numDownsapledPointsLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.resistanceNumeric);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numDownsampledPtsPow2Numeric);
            this.groupBox1.Location = new System.Drawing.Point(15, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 348);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Downsampling";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.singleShotNumPointsLabel);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.singleShotNumberPointsPow2Numeric);
            this.groupBox2.Location = new System.Drawing.Point(15, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 110);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single Shot";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(127, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Pow 2";
            // 
            // singleShotNumPointsLabel
            // 
            this.singleShotNumPointsLabel.AutoSize = true;
            this.singleShotNumPointsLabel.Location = new System.Drawing.Point(12, 41);
            this.singleShotNumPointsLabel.Name = "singleShotNumPointsLabel";
            this.singleShotNumPointsLabel.Size = new System.Drawing.Size(13, 13);
            this.singleShotNumPointsLabel.TabIndex = 24;
            this.singleShotNumPointsLabel.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Number of Points";
            // 
            // singleShotNumberPointsPow2Numeric
            // 
            this.singleShotNumberPointsPow2Numeric.Location = new System.Drawing.Point(130, 34);
            this.singleShotNumberPointsPow2Numeric.Name = "singleShotNumberPointsPow2Numeric";
            this.singleShotNumberPointsPow2Numeric.Size = new System.Drawing.Size(62, 20);
            this.singleShotNumberPointsPow2Numeric.TabIndex = 0;
            this.singleShotNumberPointsPow2Numeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.singleShotNumberPointsPow2Numeric.ValueChanged += new System.EventHandler(this.singleShotNumberPointsPow2Numeric_ValueChanged);
            // 
            // Sparrow_Options
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(306, 598);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
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
            ((System.ComponentModel.ISupportInitialize)(this.timeDec1Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeDec2Numeric)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singleShotNumberPointsPow2Numeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox broadSpecUnitsComboBox;
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
        private System.Windows.Forms.NumericUpDown timeDec1Numeric;
        private System.Windows.Forms.NumericUpDown timeDec2Numeric;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox fftAveragingCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown singleShotNumberPointsPow2Numeric;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label singleShotNumPointsLabel;
    }
}