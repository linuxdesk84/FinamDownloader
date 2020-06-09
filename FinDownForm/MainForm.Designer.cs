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
            this.fldLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fldIssuerName = new System.Windows.Forms.TextBox();
            this.chEqualName = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chAllTime = new System.Windows.Forms.CheckBox();
            this.dtpPeriodBeg = new System.Windows.Forms.DateTimePicker();
            this.dtpPeriodEnd = new System.Windows.Forms.DateTimePicker();
            this.chFutures = new System.Windows.Forms.CheckBox();
            this.butDownload = new System.Windows.Forms.Button();
            this.chSkipUnfinished = new System.Windows.Forms.CheckBox();
            this.chOverwrite = new System.Windows.Forms.CheckBox();
            this.butSearch = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIssuersCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDownloadedCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fldIssuerMarket = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fldIssuerId = new System.Windows.Forms.TextBox();
            this.butSettings = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fldLog
            // 
            this.fldLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldLog.Location = new System.Drawing.Point(12, 197);
            this.fldLog.MaxLength = 4194305;
            this.fldLog.Multiline = true;
            this.fldLog.Name = "fldLog";
            this.fldLog.ReadOnly = true;
            this.fldLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.fldLog.Size = new System.Drawing.Size(360, 132);
            this.fldLog.TabIndex = 0;
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
            // chEqualName
            // 
            this.chEqualName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chEqualName.AutoSize = true;
            this.chEqualName.Location = new System.Drawing.Point(203, 84);
            this.chEqualName.Name = "chEqualName";
            this.chEqualName.Size = new System.Drawing.Size(81, 17);
            this.chEqualName.TabIndex = 4;
            this.chEqualName.Text = "equal name";
            this.chEqualName.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chAllTime);
            this.groupBox1.Controls.Add(this.dtpPeriodBeg);
            this.groupBox1.Controls.Add(this.dtpPeriodEnd);
            this.groupBox1.Controls.Add(this.chFutures);
            this.groupBox1.Controls.Add(this.butDownload);
            this.groupBox1.Controls.Add(this.chSkipUnfinished);
            this.groupBox1.Controls.Add(this.chOverwrite);
            this.groupBox1.Location = new System.Drawing.Point(12, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 79);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "downloads";
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
            this.chAllTime.CheckedChanged += new System.EventHandler(this.chAllTime_CheckedChanged);
            // 
            // dtpPeriodBeg
            // 
            this.dtpPeriodBeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPeriodBeg.Location = new System.Drawing.Point(101, 20);
            this.dtpPeriodBeg.Name = "dtpPeriodBeg";
            this.dtpPeriodBeg.Size = new System.Drawing.Size(123, 20);
            this.dtpPeriodBeg.TabIndex = 10;
            // 
            // dtpPeriodEnd
            // 
            this.dtpPeriodEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPeriodEnd.Location = new System.Drawing.Point(230, 20);
            this.dtpPeriodEnd.Name = "dtpPeriodEnd";
            this.dtpPeriodEnd.Size = new System.Drawing.Size(123, 20);
            this.dtpPeriodEnd.TabIndex = 9;
            // 
            // chFutures
            // 
            this.chFutures.AutoSize = true;
            this.chFutures.Location = new System.Drawing.Point(11, 52);
            this.chFutures.Name = "chFutures";
            this.chFutures.Size = new System.Drawing.Size(58, 17);
            this.chFutures.TabIndex = 8;
            this.chFutures.Text = "futures";
            this.chFutures.UseVisualStyleBackColor = true;
            this.chFutures.CheckedChanged += new System.EventHandler(this.chFutures_CheckedChanged);
            // 
            // butDownload
            // 
            this.butDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butDownload.Location = new System.Drawing.Point(278, 48);
            this.butDownload.Name = "butDownload";
            this.butDownload.Size = new System.Drawing.Size(75, 23);
            this.butDownload.TabIndex = 4;
            this.butDownload.Text = "Download";
            this.butDownload.UseVisualStyleBackColor = true;
            // 
            // chSkipUnfinished
            // 
            this.chSkipUnfinished.AutoSize = true;
            this.chSkipUnfinished.Location = new System.Drawing.Point(70, 52);
            this.chSkipUnfinished.Name = "chSkipUnfinished";
            this.chSkipUnfinished.Size = new System.Drawing.Size(96, 17);
            this.chSkipUnfinished.TabIndex = 3;
            this.chSkipUnfinished.Text = "skip unfinished";
            this.chSkipUnfinished.UseVisualStyleBackColor = true;
            this.chSkipUnfinished.CheckedChanged += new System.EventHandler(this.chSkipUnfinished_CheckedChanged);
            // 
            // chOverwrite
            // 
            this.chOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chOverwrite.AutoSize = true;
            this.chOverwrite.Location = new System.Drawing.Point(203, 52);
            this.chOverwrite.Name = "chOverwrite";
            this.chOverwrite.Size = new System.Drawing.Size(69, 17);
            this.chOverwrite.TabIndex = 2;
            this.chOverwrite.Text = "overwrite";
            this.chOverwrite.UseVisualStyleBackColor = true;
            this.chOverwrite.CheckedChanged += new System.EventHandler(this.chOverWrite_CheckedChanged);
            // 
            // butSearch
            // 
            this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearch.Location = new System.Drawing.Point(290, 80);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(75, 23);
            this.butSearch.TabIndex = 6;
            this.butSearch.Text = "Search";
            this.butSearch.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblIssuersCount,
            this.toolStripStatusLabel2,
            this.lblDownloadedCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(384, 22);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.fldIssuerId);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.fldIssuerMarket);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.fldIssuerName);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(130, 94);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "issuer";
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
            // butSettings
            // 
            this.butSettings.Location = new System.Drawing.Point(290, 18);
            this.butSettings.Name = "butSettings";
            this.butSettings.Size = new System.Drawing.Size(75, 23);
            this.butSettings.TabIndex = 11;
            this.butSettings.Text = "Settings";
            this.butSettings.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 362);
            this.Controls.Add(this.butSettings);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.butSearch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chEqualName);
            this.Controls.Add(this.fldLog);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.Text = "FinamDownloader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fldLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fldIssuerName;
        private System.Windows.Forms.CheckBox chEqualName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butDownload;
        private System.Windows.Forms.CheckBox chSkipUnfinished;
        private System.Windows.Forms.CheckBox chOverwrite;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.CheckBox chFutures;
        private System.Windows.Forms.CheckBox chAllTime;
        private System.Windows.Forms.DateTimePicker dtpPeriodBeg;
        private System.Windows.Forms.DateTimePicker dtpPeriodEnd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblIssuersCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblDownloadedCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fldIssuerId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fldIssuerMarket;
        private System.Windows.Forms.Button butSettings;
    }
}

