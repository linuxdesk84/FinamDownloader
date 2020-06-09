namespace FinDownForm
{
    partial class Settings
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.butIChartsUpdate = new System.Windows.Forms.Button();
            this.butChooseICharts = new System.Windows.Forms.Button();
            this.fldPathIcharts = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chIChartsAutoUpdate = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butHistDataDirChoose = new System.Windows.Forms.Button();
            this.fldHistDataDir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chIChartsAutoUpdate);
            this.groupBox2.Controls.Add(this.butIChartsUpdate);
            this.groupBox2.Controls.Add(this.butChooseICharts);
            this.groupBox2.Controls.Add(this.fldPathIcharts);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.MinimumSize = new System.Drawing.Size(260, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 76);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "icharts.js";
            // 
            // butIChartsUpdate
            // 
            this.butIChartsUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butIChartsUpdate.Location = new System.Drawing.Point(181, 44);
            this.butIChartsUpdate.Name = "butIChartsUpdate";
            this.butIChartsUpdate.Size = new System.Drawing.Size(75, 23);
            this.butIChartsUpdate.TabIndex = 3;
            this.butIChartsUpdate.Text = "Update";
            this.butIChartsUpdate.UseVisualStyleBackColor = true;
            // 
            // butChooseICharts
            // 
            this.butChooseICharts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butChooseICharts.Location = new System.Drawing.Point(231, 15);
            this.butChooseICharts.Name = "butChooseICharts";
            this.butChooseICharts.Size = new System.Drawing.Size(25, 23);
            this.butChooseICharts.TabIndex = 2;
            this.butChooseICharts.Text = "...";
            this.butChooseICharts.UseVisualStyleBackColor = true;
            // 
            // fldPathIcharts
            // 
            this.fldPathIcharts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldPathIcharts.Location = new System.Drawing.Point(41, 17);
            this.fldPathIcharts.Name = "fldPathIcharts";
            this.fldPathIcharts.Size = new System.Drawing.Size(184, 20);
            this.fldPathIcharts.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "path";
            // 
            // chIChartsAutoUpdate
            // 
            this.chIChartsAutoUpdate.AutoSize = true;
            this.chIChartsAutoUpdate.Location = new System.Drawing.Point(10, 48);
            this.chIChartsAutoUpdate.Name = "chIChartsAutoUpdate";
            this.chIChartsAutoUpdate.Size = new System.Drawing.Size(83, 17);
            this.chIChartsAutoUpdate.TabIndex = 4;
            this.chIChartsAutoUpdate.Text = "auto update";
            this.chIChartsAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.butHistDataDirChoose);
            this.groupBox1.Controls.Add(this.fldHistDataDir);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 94);
            this.groupBox1.MinimumSize = new System.Drawing.Size(260, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 46);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "history data directory";
            // 
            // butHistDataDirChoose
            // 
            this.butHistDataDirChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butHistDataDirChoose.Location = new System.Drawing.Point(231, 15);
            this.butHistDataDirChoose.Name = "butHistDataDirChoose";
            this.butHistDataDirChoose.Size = new System.Drawing.Size(25, 23);
            this.butHistDataDirChoose.TabIndex = 2;
            this.butHistDataDirChoose.Text = "...";
            this.butHistDataDirChoose.UseVisualStyleBackColor = true;
            // 
            // fldHistDataDir
            // 
            this.fldHistDataDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldHistDataDir.Location = new System.Drawing.Point(41, 17);
            this.fldHistDataDir.Name = "fldHistDataDir";
            this.fldHistDataDir.Size = new System.Drawing.Size(184, 20);
            this.fldHistDataDir.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "path";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 152);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(300, 190);
            this.Name = "Settings";
            this.Text = "Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butIChartsUpdate;
        private System.Windows.Forms.Button butChooseICharts;
        private System.Windows.Forms.TextBox fldPathIcharts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chIChartsAutoUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butHistDataDirChoose;
        private System.Windows.Forms.TextBox fldHistDataDir;
        private System.Windows.Forms.Label label3;
    }
}