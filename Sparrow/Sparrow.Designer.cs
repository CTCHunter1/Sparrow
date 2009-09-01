namespace Sparrow
{
    partial class Sparrow
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
            this.fastTimeGraph = new GraphControl.GraphControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nI6251ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.broadFreqGraph = new GraphControl.GraphControl();
            this.slowTimeGraph = new GraphControl.GraphControl();
            this.narrowFreqGraph = new GraphControl.GraphControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.narrowAmpUnitsLabel = new System.Windows.Forms.Label();
            this.broadAmpUnitsLabel = new System.Windows.Forms.Label();
            this.max1Label = new System.Windows.Forms.Label();
            this.max2Label = new System.Windows.Forms.Label();
            this.max3Label = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fastTimeGraph
            // 
            this.fastTimeGraph.AutoScale = true;
            this.fastTimeGraph.Location = new System.Drawing.Point(32, 15);
            this.fastTimeGraph.Name = "fastTimeGraph";
            this.fastTimeGraph.Size = new System.Drawing.Size(393, 295);
            this.fastTimeGraph.TabIndex = 0;
            this.fastTimeGraph.XLim = new float[] {
        -10F,
        10F};
            this.fastTimeGraph.YLim = new float[] {
        -10F,
        10F};
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1034, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nI6251ToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // nI6251ToolStripMenuItem
            // 
            this.nI6251ToolStripMenuItem.Name = "nI6251ToolStripMenuItem";
            this.nI6251ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.nI6251ToolStripMenuItem.Text = "NI-6251";
            this.nI6251ToolStripMenuItem.Click += new System.EventHandler(this.nI6251ToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // broadFreqGraph
            // 
            this.broadFreqGraph.AutoScale = true;
            this.broadFreqGraph.Location = new System.Drawing.Point(27, 15);
            this.broadFreqGraph.Name = "broadFreqGraph";
            this.broadFreqGraph.Size = new System.Drawing.Size(395, 295);
            this.broadFreqGraph.TabIndex = 2;
            this.broadFreqGraph.XLim = new float[] {
        -10F,
        10F};
            this.broadFreqGraph.YLim = new float[] {
        -10F,
        10F};
            // 
            // slowTimeGraph
            // 
            this.slowTimeGraph.AutoScale = true;
            this.slowTimeGraph.Location = new System.Drawing.Point(32, 334);
            this.slowTimeGraph.Name = "slowTimeGraph";
            this.slowTimeGraph.Size = new System.Drawing.Size(393, 295);
            this.slowTimeGraph.TabIndex = 3;
            this.slowTimeGraph.XLim = new float[] {
        -10F,
        10F};
            this.slowTimeGraph.YLim = new float[] {
        -10F,
        10F};
            // 
            // narrowFreqGraph
            // 
            this.narrowFreqGraph.AutoScale = true;
            this.narrowFreqGraph.Location = new System.Drawing.Point(27, 328);
            this.narrowFreqGraph.Name = "narrowFreqGraph";
            this.narrowFreqGraph.Size = new System.Drawing.Size(395, 295);
            this.narrowFreqGraph.TabIndex = 4;
            this.narrowFreqGraph.XLim = new float[] {
        -10F,
        10F};
            this.narrowFreqGraph.YLim = new float[] {
        -10F,
        10F};
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.slowTimeGraph);
            this.groupBox1.Controls.Add(this.fastTimeGraph);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 662);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time Domain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 469);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "(V)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "(V)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 313);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time (s)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(221, 632);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Time (s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 626);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Frequency (Hz)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(174, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Frequency (Hz)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.max3Label);
            this.groupBox2.Controls.Add(this.max2Label);
            this.groupBox2.Controls.Add(this.max1Label);
            this.groupBox2.Controls.Add(this.narrowAmpUnitsLabel);
            this.groupBox2.Controls.Add(this.broadAmpUnitsLabel);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.narrowFreqGraph);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.broadFreqGraph);
            this.groupBox2.Location = new System.Drawing.Point(456, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(486, 661);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frequency Domain";
            // 
            // narrowAmpUnitsLabel
            // 
            this.narrowAmpUnitsLabel.AutoSize = true;
            this.narrowAmpUnitsLabel.Location = new System.Drawing.Point(6, 459);
            this.narrowAmpUnitsLabel.Name = "narrowAmpUnitsLabel";
            this.narrowAmpUnitsLabel.Size = new System.Drawing.Size(20, 13);
            this.narrowAmpUnitsLabel.TabIndex = 11;
            this.narrowAmpUnitsLabel.Text = "(V)";
            // 
            // broadAmpUnitsLabel
            // 
            this.broadAmpUnitsLabel.AutoSize = true;
            this.broadAmpUnitsLabel.Location = new System.Drawing.Point(6, 145);
            this.broadAmpUnitsLabel.Name = "broadAmpUnitsLabel";
            this.broadAmpUnitsLabel.Size = new System.Drawing.Size(20, 13);
            this.broadAmpUnitsLabel.TabIndex = 10;
            this.broadAmpUnitsLabel.Text = "(V)";
            // 
            // max1Label
            // 
            this.max1Label.AutoSize = true;
            this.max1Label.Location = new System.Drawing.Point(419, 44);
            this.max1Label.Name = "max1Label";
            this.max1Label.Size = new System.Drawing.Size(22, 13);
            this.max1Label.TabIndex = 12;
            this.max1Label.Text = "0.0";
            // 
            // max2Label
            // 
            this.max2Label.AutoSize = true;
            this.max2Label.Location = new System.Drawing.Point(419, 67);
            this.max2Label.Name = "max2Label";
            this.max2Label.Size = new System.Drawing.Size(22, 13);
            this.max2Label.TabIndex = 13;
            this.max2Label.Text = "0.0";
            // 
            // max3Label
            // 
            this.max3Label.AutoSize = true;
            this.max3Label.Location = new System.Drawing.Point(419, 90);
            this.max3Label.Name = "max3Label";
            this.max3Label.Size = new System.Drawing.Size(22, 13);
            this.max3Label.TabIndex = 14;
            this.max3Label.Text = "0.0";
            // 
            // Sparrow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 719);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Sparrow";
            this.Text = "Sparrow";
            this.Shown += new System.EventHandler(this.Sparrow_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GraphControl.GraphControl fastTimeGraph;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nI6251ToolStripMenuItem;
        private GraphControl.GraphControl broadFreqGraph;
        private GraphControl.GraphControl slowTimeGraph;
        private GraphControl.GraphControl narrowFreqGraph;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.Label narrowAmpUnitsLabel;
        private System.Windows.Forms.Label broadAmpUnitsLabel;
        private System.Windows.Forms.Label max3Label;
        private System.Windows.Forms.Label max2Label;
        private System.Windows.Forms.Label max1Label;
    }
}

