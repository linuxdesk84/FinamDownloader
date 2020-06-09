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
            this.chEqual = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butSearch = new System.Windows.Forms.Button();
            this.chOverwrite = new System.Windows.Forms.CheckBox();
            this.chSkipUnfinished = new System.Windows.Forms.CheckBox();
            this.butDownload = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fldPathIcharts = new System.Windows.Forms.TextBox();
            this.butOpen = new System.Windows.Forms.Button();
            this.butUpdate = new System.Windows.Forms.Button();
            this.chFutures = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIssuersCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDownloaded = new System.Windows.Forms.ToolStripStatusLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.chAllTime = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fldLog
            // 
            this.fldLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldLog.Location = new System.Drawing.Point(13, 176);
            this.fldLog.Multiline = true;
            this.fldLog.Name = "fldLog";
            this.fldLog.Size = new System.Drawing.Size(399, 185);
            this.fldLog.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "issuer name";
            // 
            // fldIssuerName
            // 
            this.fldIssuerName.Location = new System.Drawing.Point(82, 10);
            this.fldIssuerName.Name = "fldIssuerName";
            this.fldIssuerName.Size = new System.Drawing.Size(100, 20);
            this.fldIssuerName.TabIndex = 3;
            // 
            // chEqual
            // 
            this.chEqual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chEqual.AutoSize = true;
            this.chEqual.Location = new System.Drawing.Point(271, 12);
            this.chEqual.Name = "chEqual";
            this.chEqual.Size = new System.Drawing.Size(52, 17);
            this.chEqual.TabIndex = 4;
            this.chEqual.Text = "equal";
            this.chEqual.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chAllTime);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.chFutures);
            this.groupBox1.Controls.Add(this.butDownload);
            this.groupBox1.Controls.Add(this.chSkipUnfinished);
            this.groupBox1.Controls.Add(this.chOverwrite);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 79);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "downloads";
            // 
            // butSearch
            // 
            this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearch.Location = new System.Drawing.Point(337, 8);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(75, 23);
            this.butSearch.TabIndex = 6;
            this.butSearch.Text = "Search";
            this.butSearch.UseVisualStyleBackColor = true;
            // 
            // chOverwrite
            // 
            this.chOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chOverwrite.AutoSize = true;
            this.chOverwrite.Location = new System.Drawing.Point(238, 52);
            this.chOverwrite.Name = "chOverwrite";
            this.chOverwrite.Size = new System.Drawing.Size(69, 17);
            this.chOverwrite.TabIndex = 2;
            this.chOverwrite.Text = "overwrite";
            this.chOverwrite.UseVisualStyleBackColor = true;
            this.chOverwrite.CheckedChanged += new System.EventHandler(this.chOverWrite_CheckedChanged);
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
            this.chSkipUnfinished.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // butDownload
            // 
            this.butDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butDownload.Location = new System.Drawing.Point(313, 48);
            this.butDownload.Name = "butDownload";
            this.butDownload.Size = new System.Drawing.Size(75, 23);
            this.butDownload.TabIndex = 4;
            this.butDownload.Text = "Download";
            this.butDownload.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.butUpdate);
            this.groupBox2.Controls.Add(this.butOpen);
            this.groupBox2.Controls.Add(this.fldPathIcharts);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 48);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "icharts.js";
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
            // fldPathIcharts
            // 
            this.fldPathIcharts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldPathIcharts.Location = new System.Drawing.Point(41, 17);
            this.fldPathIcharts.Name = "fldPathIcharts";
            this.fldPathIcharts.Size = new System.Drawing.Size(188, 20);
            this.fldPathIcharts.TabIndex = 1;
            // 
            // butOpen
            // 
            this.butOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butOpen.Location = new System.Drawing.Point(235, 15);
            this.butOpen.Name = "butOpen";
            this.butOpen.Size = new System.Drawing.Size(75, 23);
            this.butOpen.TabIndex = 2;
            this.butOpen.Text = "Open";
            this.butOpen.UseVisualStyleBackColor = true;
            // 
            // butUpdate
            // 
            this.butUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butUpdate.Location = new System.Drawing.Point(316, 15);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 23);
            this.butUpdate.TabIndex = 3;
            this.butUpdate.Text = "Update";
            this.butUpdate.UseVisualStyleBackColor = true;
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
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblIssuersCount,
            this.toolStripStatusLabel2,
            this.lblDownloaded});
            this.statusStrip1.Location = new System.Drawing.Point(0, 380);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(424, 22);
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
            // lblDownloaded
            // 
            this.lblDownloaded.AutoSize = false;
            this.lblDownloaded.Name = "lblDownloaded";
            this.lblDownloaded.Size = new System.Drawing.Size(40, 17);
            this.lblDownloaded.Text = "0";
            this.lblDownloaded.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Location = new System.Drawing.Point(239, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(149, 20);
            this.dateTimePicker1.TabIndex = 9;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.Location = new System.Drawing.Point(71, 20);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(149, 20);
            this.dateTimePicker2.TabIndex = 10;
            // 
            // chAllTime
            // 
            this.chAllTime.AutoSize = true;
            this.chAllTime.Checked = true;
            this.chAllTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chAllTime.Location = new System.Drawing.Point(11, 20);
            this.chAllTime.Name = "chAllTime";
            this.chAllTime.Size = new System.Drawing.Size(58, 17);
            this.chAllTime.TabIndex = 11;
            this.chAllTime.Text = "all time";
            this.chAllTime.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 402);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.butSearch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chEqual);
            this.Controls.Add(this.fldIssuerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fldLog);
            this.MinimumSize = new System.Drawing.Size(440, 440);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fldLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fldIssuerName;
        private System.Windows.Forms.CheckBox chEqual;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butDownload;
        private System.Windows.Forms.CheckBox chSkipUnfinished;
        private System.Windows.Forms.CheckBox chOverwrite;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.Button butOpen;
        private System.Windows.Forms.TextBox fldPathIcharts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chFutures;
        private System.Windows.Forms.CheckBox chAllTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblIssuersCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblDownloaded;
    }
}

