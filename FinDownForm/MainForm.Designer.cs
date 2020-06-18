namespace FinDownForm
{
    partial class MainForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIssuersCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDownloadedCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chMatchCase = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fldIssuerId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fldIssuerMarket = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fldIssuerName = new System.Windows.Forms.TextBox();
            this.butSearch = new System.Windows.Forms.Button();
            this.chFutures = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.chAllTime = new System.Windows.Forms.CheckBox();
            this.dtpPeriodFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpPeriodTo = new System.Windows.Forms.DateTimePicker();
            this.butDownload = new System.Windows.Forms.Button();
            this.chSkipUnfinished = new System.Windows.Forms.CheckBox();
            this.chOverwrite = new System.Windows.Forms.CheckBox();
            this.chExactMatchName = new System.Windows.Forms.CheckBox();
            this.fldLog = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.butHistDataDirChoose = new System.Windows.Forms.Button();
            this.fldHistDataDir = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chIChartsAutoUpdate = new System.Windows.Forms.CheckBox();
            this.butIChartsUpdate = new System.Windows.Forms.Button();
            this.butChooseICharts = new System.Windows.Forms.Button();
            this.fldIChartsPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblIssuersCount,
            this.toolStripStatusLabel2,
            this.lblDownloadedCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 460);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(464, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "issuers count:";
            // 
            // lblIssuersCount
            // 
            this.lblIssuersCount.AutoSize = false;
            this.lblIssuersCount.Name = "lblIssuersCount";
            this.lblIssuersCount.Size = new System.Drawing.Size(40, 17);
            this.lblIssuersCount.Text = "0";
            this.lblIssuersCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(76, 17);
            this.toolStripStatusLabel2.Text = "downloaded:";
            // 
            // lblDownloadedCount
            // 
            this.lblDownloadedCount.AutoSize = false;
            this.lblDownloadedCount.Name = "lblDownloadedCount";
            this.lblDownloadedCount.Size = new System.Drawing.Size(40, 17);
            this.lblDownloadedCount.Text = "0";
            this.lblDownloadedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.MinimumSize = new System.Drawing.Size(440, 440);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 440);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chMatchCase);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.butSearch);
            this.tabPage1.Controls.Add(this.chFutures);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.chExactMatchName);
            this.tabPage1.Controls.Add(this.fldLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(432, 414);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chMatchCase
            // 
            this.chMatchCase.AutoSize = true;
            this.chMatchCase.Location = new System.Drawing.Point(162, 51);
            this.chMatchCase.Name = "chMatchCase";
            this.chMatchCase.Size = new System.Drawing.Size(81, 17);
            this.chMatchCase.TabIndex = 16;
            this.chMatchCase.Text = "match case";
            this.chMatchCase.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.fldIssuerId);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.fldIssuerMarket);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.fldIssuerName);
            this.groupBox2.Location = new System.Drawing.Point(10, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(130, 94);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "issuer";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "id";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fldIssuerId
            // 
            this.fldIssuerId.Location = new System.Drawing.Point(53, 65);
            this.fldIssuerId.Name = "fldIssuerId";
            this.fldIssuerId.Size = new System.Drawing.Size(69, 20);
            this.fldIssuerId.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "market";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fldIssuerMarket
            // 
            this.fldIssuerMarket.Location = new System.Drawing.Point(53, 39);
            this.fldIssuerMarket.Name = "fldIssuerMarket";
            this.fldIssuerMarket.Size = new System.Drawing.Size(69, 20);
            this.fldIssuerMarket.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fldIssuerName
            // 
            this.fldIssuerName.Location = new System.Drawing.Point(53, 13);
            this.fldIssuerName.Name = "fldIssuerName";
            this.fldIssuerName.Size = new System.Drawing.Size(69, 20);
            this.fldIssuerName.TabIndex = 3;
            // 
            // butSearch
            // 
            this.butSearch.Location = new System.Drawing.Point(339, 77);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(75, 23);
            this.butSearch.TabIndex = 14;
            this.butSearch.Text = "Search";
            this.butSearch.UseVisualStyleBackColor = true;
            // 
            // chFutures
            // 
            this.chFutures.AutoSize = true;
            this.chFutures.Location = new System.Drawing.Point(162, 76);
            this.chFutures.Name = "chFutures";
            this.chFutures.Size = new System.Drawing.Size(58, 17);
            this.chFutures.TabIndex = 8;
            this.chFutures.Text = "futures";
            this.chFutures.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butCancel);
            this.groupBox1.Controls.Add(this.chAllTime);
            this.groupBox1.Controls.Add(this.dtpPeriodFrom);
            this.groupBox1.Controls.Add(this.dtpPeriodTo);
            this.groupBox1.Controls.Add(this.butDownload);
            this.groupBox1.Controls.Add(this.chSkipUnfinished);
            this.groupBox1.Controls.Add(this.chOverwrite);
            this.groupBox1.Location = new System.Drawing.Point(10, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 79);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "downloads";
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(248, 48);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 12;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // chAllTime
            // 
            this.chAllTime.AutoSize = true;
            this.chAllTime.Location = new System.Drawing.Point(10, 23);
            this.chAllTime.Name = "chAllTime";
            this.chAllTime.Size = new System.Drawing.Size(58, 17);
            this.chAllTime.TabIndex = 11;
            this.chAllTime.Text = "all time";
            this.chAllTime.UseVisualStyleBackColor = true;
            // 
            // dtpPeriodFrom
            // 
            this.dtpPeriodFrom.Location = new System.Drawing.Point(152, 20);
            this.dtpPeriodFrom.Name = "dtpPeriodFrom";
            this.dtpPeriodFrom.Size = new System.Drawing.Size(123, 20);
            this.dtpPeriodFrom.TabIndex = 10;
            // 
            // dtpPeriodTo
            // 
            this.dtpPeriodTo.Location = new System.Drawing.Point(281, 20);
            this.dtpPeriodTo.Name = "dtpPeriodTo";
            this.dtpPeriodTo.Size = new System.Drawing.Size(123, 20);
            this.dtpPeriodTo.TabIndex = 9;
            // 
            // butDownload
            // 
            this.butDownload.Location = new System.Drawing.Point(329, 48);
            this.butDownload.Name = "butDownload";
            this.butDownload.Size = new System.Drawing.Size(75, 23);
            this.butDownload.TabIndex = 4;
            this.butDownload.Text = "Download";
            this.butDownload.UseVisualStyleBackColor = true;
            // 
            // chSkipUnfinished
            // 
            this.chSkipUnfinished.AutoSize = true;
            this.chSkipUnfinished.Location = new System.Drawing.Point(152, 52);
            this.chSkipUnfinished.Name = "chSkipUnfinished";
            this.chSkipUnfinished.Size = new System.Drawing.Size(96, 17);
            this.chSkipUnfinished.TabIndex = 3;
            this.chSkipUnfinished.Text = "skip unfinished";
            this.chSkipUnfinished.UseVisualStyleBackColor = true;
            // 
            // chOverwrite
            // 
            this.chOverwrite.AutoSize = true;
            this.chOverwrite.Location = new System.Drawing.Point(10, 52);
            this.chOverwrite.Name = "chOverwrite";
            this.chOverwrite.Size = new System.Drawing.Size(69, 17);
            this.chOverwrite.TabIndex = 2;
            this.chOverwrite.Text = "overwrite";
            this.chOverwrite.UseVisualStyleBackColor = true;
            // 
            // chExactMatchName
            // 
            this.chExactMatchName.AutoSize = true;
            this.chExactMatchName.Location = new System.Drawing.Point(162, 25);
            this.chExactMatchName.Name = "chExactMatchName";
            this.chExactMatchName.Size = new System.Drawing.Size(113, 17);
            this.chExactMatchName.TabIndex = 12;
            this.chExactMatchName.Text = "exact match name";
            this.chExactMatchName.UseVisualStyleBackColor = true;
            // 
            // fldLog
            // 
            this.fldLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldLog.Location = new System.Drawing.Point(10, 194);
            this.fldLog.MaxLength = 4194305;
            this.fldLog.Multiline = true;
            this.fldLog.Name = "fldLog";
            this.fldLog.ReadOnly = true;
            this.fldLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.fldLog.Size = new System.Drawing.Size(411, 211);
            this.fldLog.TabIndex = 11;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(432, 414);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.butHistDataDirChoose);
            this.groupBox3.Controls.Add(this.fldHistDataDir);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(6, 88);
            this.groupBox3.MinimumSize = new System.Drawing.Size(260, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(420, 46);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "history data directory";
            // 
            // butHistDataDirChoose
            // 
            this.butHistDataDirChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butHistDataDirChoose.Location = new System.Drawing.Point(389, 15);
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
            this.fldHistDataDir.Size = new System.Drawing.Size(342, 20);
            this.fldHistDataDir.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "path";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chIChartsAutoUpdate);
            this.groupBox4.Controls.Add(this.butIChartsUpdate);
            this.groupBox4.Controls.Add(this.butChooseICharts);
            this.groupBox4.Controls.Add(this.fldIChartsPath);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.MinimumSize = new System.Drawing.Size(260, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(420, 76);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "icharts.js";
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
            // butIChartsUpdate
            // 
            this.butIChartsUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butIChartsUpdate.Location = new System.Drawing.Point(339, 44);
            this.butIChartsUpdate.Name = "butIChartsUpdate";
            this.butIChartsUpdate.Size = new System.Drawing.Size(75, 23);
            this.butIChartsUpdate.TabIndex = 3;
            this.butIChartsUpdate.Text = "Update";
            this.butIChartsUpdate.UseVisualStyleBackColor = true;
            // 
            // butChooseICharts
            // 
            this.butChooseICharts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butChooseICharts.Location = new System.Drawing.Point(389, 15);
            this.butChooseICharts.Name = "butChooseICharts";
            this.butChooseICharts.Size = new System.Drawing.Size(25, 23);
            this.butChooseICharts.TabIndex = 2;
            this.butChooseICharts.Text = "...";
            this.butChooseICharts.UseVisualStyleBackColor = true;
            // 
            // fldIChartsPath
            // 
            this.fldIChartsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldIChartsPath.Location = new System.Drawing.Point(41, 17);
            this.fldIChartsPath.Name = "fldIChartsPath";
            this.fldIChartsPath.Size = new System.Drawing.Size(342, 20);
            this.fldIChartsPath.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "path";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 482);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(480, 520);
            this.Name = "MainForm";
            this.Text = "FinamDownloader";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblIssuersCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblDownloadedCount;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fldIssuerId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fldIssuerMarket;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fldIssuerName;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chAllTime;
        private System.Windows.Forms.DateTimePicker dtpPeriodFrom;
        private System.Windows.Forms.DateTimePicker dtpPeriodTo;
        private System.Windows.Forms.CheckBox chFutures;
        private System.Windows.Forms.Button butDownload;
        private System.Windows.Forms.CheckBox chSkipUnfinished;
        private System.Windows.Forms.CheckBox chOverwrite;
        private System.Windows.Forms.CheckBox chExactMatchName;
        private System.Windows.Forms.TextBox fldLog;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button butHistDataDirChoose;
        private System.Windows.Forms.TextBox fldHistDataDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chIChartsAutoUpdate;
        private System.Windows.Forms.Button butIChartsUpdate;
        private System.Windows.Forms.Button butChooseICharts;
        private System.Windows.Forms.TextBox fldIChartsPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chMatchCase;
        private System.Windows.Forms.Button butCancel;
    }
}

