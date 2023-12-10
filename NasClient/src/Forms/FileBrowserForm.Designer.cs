namespace NAS
{
    partial class FileBrowserForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "11",
            "22"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("2");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("3");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "11"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("2");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("3");
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.새폴더ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.파일업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvFileBrowser = new System.Windows.Forms.ListView();
            this.hdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdGenDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdCapacity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctxMenuNoSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.새폴더ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.파일업로드ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFakePath = new System.Windows.Forms.TextBox();
            this.cxtMenuDirectoryViewer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.루트경로로이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.상위경로로이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btRoot = new System.Windows.Forms.Button();
            this.btBack = new System.Windows.Forms.Button();
            this.ctxMenuFileSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.다운로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuFolderSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.삭제ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lbFileCounter = new System.Windows.Forms.Label();
            this.lbDownloadingCounter = new System.Windows.Forms.Label();
            this.lvFileDownloadingBrowser = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuBar.SuspendLayout();
            this.ctxMenuNoSelected.SuspendLayout();
            this.cxtMenuDirectoryViewer.SuspendLayout();
            this.ctxMenuFileSelected.SuspendLayout();
            this.ctxMenuFolderSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.설정ToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(944, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuBar";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.새폴더ToolStripMenuItem1,
            this.파일업로드ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 새폴더ToolStripMenuItem1
            // 
            this.새폴더ToolStripMenuItem1.Name = "새폴더ToolStripMenuItem1";
            this.새폴더ToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.새폴더ToolStripMenuItem1.Text = "새 폴더";
            this.새폴더ToolStripMenuItem1.Click += new System.EventHandler(this.새폴더ToolStripMenuItem1_Click);
            // 
            // 파일업로드ToolStripMenuItem
            // 
            this.파일업로드ToolStripMenuItem.Name = "파일업로드ToolStripMenuItem";
            this.파일업로드ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.파일업로드ToolStripMenuItem.Text = "파일 업로드";
            this.파일업로드ToolStripMenuItem.Click += new System.EventHandler(this.파일업로드ToolStripMenuItem_Click);
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.설정ToolStripMenuItem.Text = "설정";
            // 
            // lvFileBrowser
            // 
            this.lvFileBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdName,
            this.hdGenDate,
            this.hdCapacity});
            this.lvFileBrowser.ContextMenuStrip = this.ctxMenuNoSelected;
            this.lvFileBrowser.FullRowSelect = true;
            this.lvFileBrowser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileBrowser.HideSelection = false;
            this.lvFileBrowser.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvFileBrowser.Location = new System.Drawing.Point(12, 72);
            this.lvFileBrowser.MultiSelect = false;
            this.lvFileBrowser.Name = "lvFileBrowser";
            this.lvFileBrowser.Size = new System.Drawing.Size(920, 261);
            this.lvFileBrowser.TabIndex = 1;
            this.lvFileBrowser.UseCompatibleStateImageBehavior = false;
            this.lvFileBrowser.View = System.Windows.Forms.View.Details;
            this.lvFileBrowser.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFileBrowser_ItemSelectionChanged);
            this.lvFileBrowser.SelectedIndexChanged += new System.EventHandler(this.lvFileBrowser_SelectedIndexChanged);
            this.lvFileBrowser.DoubleClick += new System.EventHandler(this.lvFileBrowser_DoubleClick);
            // 
            // hdName
            // 
            this.hdName.Text = "이름";
            this.hdName.Width = 660;
            // 
            // hdGenDate
            // 
            this.hdGenDate.Text = "파일 유형";
            this.hdGenDate.Width = 126;
            // 
            // hdCapacity
            // 
            this.hdCapacity.Text = "파일 크기";
            this.hdCapacity.Width = 126;
            // 
            // ctxMenuNoSelected
            // 
            this.ctxMenuNoSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.새폴더ToolStripMenuItem,
            this.파일업로드ToolStripMenuItem1});
            this.ctxMenuNoSelected.Name = "ctxMenuNoSelected";
            this.ctxMenuNoSelected.Size = new System.Drawing.Size(139, 48);
            // 
            // 새폴더ToolStripMenuItem
            // 
            this.새폴더ToolStripMenuItem.Name = "새폴더ToolStripMenuItem";
            this.새폴더ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.새폴더ToolStripMenuItem.Text = "새 폴더";
            this.새폴더ToolStripMenuItem.Click += new System.EventHandler(this.새폴더ToolStripMenuItem_Click);
            // 
            // 파일업로드ToolStripMenuItem1
            // 
            this.파일업로드ToolStripMenuItem1.Name = "파일업로드ToolStripMenuItem1";
            this.파일업로드ToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.파일업로드ToolStripMenuItem1.Text = "파일 업로드";
            this.파일업로드ToolStripMenuItem1.Click += new System.EventHandler(this.파일업로드ToolStripMenuItem1_Click);
            // 
            // txtFakePath
            // 
            this.txtFakePath.ContextMenuStrip = this.cxtMenuDirectoryViewer;
            this.txtFakePath.Location = new System.Drawing.Point(174, 27);
            this.txtFakePath.Name = "txtFakePath";
            this.txtFakePath.ReadOnly = true;
            this.txtFakePath.Size = new System.Drawing.Size(758, 21);
            this.txtFakePath.TabIndex = 0;
            this.txtFakePath.TabStop = false;
            this.txtFakePath.Text = "C:\\NAS\\";
            // 
            // cxtMenuDirectoryViewer
            // 
            this.cxtMenuDirectoryViewer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.루트경로로이동ToolStripMenuItem,
            this.상위경로로이동ToolStripMenuItem});
            this.cxtMenuDirectoryViewer.Name = "ctxMenuFileSelected";
            this.cxtMenuDirectoryViewer.Size = new System.Drawing.Size(167, 48);
            // 
            // 루트경로로이동ToolStripMenuItem
            // 
            this.루트경로로이동ToolStripMenuItem.Name = "루트경로로이동ToolStripMenuItem";
            this.루트경로로이동ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.루트경로로이동ToolStripMenuItem.Text = "루트 경로로 이동";
            this.루트경로로이동ToolStripMenuItem.Click += new System.EventHandler(this.루트경로로이동ToolStripMenuItem_Click);
            // 
            // 상위경로로이동ToolStripMenuItem
            // 
            this.상위경로로이동ToolStripMenuItem.Name = "상위경로로이동ToolStripMenuItem";
            this.상위경로로이동ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.상위경로로이동ToolStripMenuItem.Text = "상위 경로로 이동";
            this.상위경로로이동ToolStripMenuItem.Click += new System.EventHandler(this.상위경로로이동ToolStripMenuItem_Click);
            // 
            // btRoot
            // 
            this.btRoot.Location = new System.Drawing.Point(12, 26);
            this.btRoot.Name = "btRoot";
            this.btRoot.Size = new System.Drawing.Size(75, 23);
            this.btRoot.TabIndex = 3;
            this.btRoot.Text = "Root";
            this.btRoot.UseVisualStyleBackColor = true;
            this.btRoot.Click += new System.EventHandler(this.btRoot_Click);
            // 
            // btBack
            // 
            this.btBack.Location = new System.Drawing.Point(93, 26);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 4;
            this.btBack.Text = "Back";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // ctxMenuFileSelected
            // 
            this.ctxMenuFileSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.다운로드ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.삭제ToolStripMenuItem});
            this.ctxMenuFileSelected.Name = "ctxMenuFileSelected";
            this.ctxMenuFileSelected.Size = new System.Drawing.Size(123, 54);
            // 
            // 다운로드ToolStripMenuItem
            // 
            this.다운로드ToolStripMenuItem.Name = "다운로드ToolStripMenuItem";
            this.다운로드ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.다운로드ToolStripMenuItem.Text = "다운로드";
            this.다운로드ToolStripMenuItem.Click += new System.EventHandler(this.다운로드ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(119, 6);
            // 
            // 삭제ToolStripMenuItem
            // 
            this.삭제ToolStripMenuItem.Name = "삭제ToolStripMenuItem";
            this.삭제ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.삭제ToolStripMenuItem.Text = "삭제";
            this.삭제ToolStripMenuItem.Click += new System.EventHandler(this.삭제ToolStripMenuItem_Click);
            // 
            // ctxMenuFolderSelected
            // 
            this.ctxMenuFolderSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.이동ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.삭제ToolStripMenuItem1});
            this.ctxMenuFolderSelected.Name = "ctxMenuFileSelected";
            this.ctxMenuFolderSelected.Size = new System.Drawing.Size(99, 54);
            // 
            // 이동ToolStripMenuItem
            // 
            this.이동ToolStripMenuItem.Name = "이동ToolStripMenuItem";
            this.이동ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.이동ToolStripMenuItem.Text = "이동";
            this.이동ToolStripMenuItem.Click += new System.EventHandler(this.이동ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(95, 6);
            // 
            // 삭제ToolStripMenuItem1
            // 
            this.삭제ToolStripMenuItem1.Name = "삭제ToolStripMenuItem1";
            this.삭제ToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            this.삭제ToolStripMenuItem1.Text = "삭제";
            this.삭제ToolStripMenuItem1.Click += new System.EventHandler(this.삭제ToolStripMenuItem1_Click);
            // 
            // lbFileCounter
            // 
            this.lbFileCounter.Location = new System.Drawing.Point(798, 51);
            this.lbFileCounter.Name = "lbFileCounter";
            this.lbFileCounter.Size = new System.Drawing.Size(134, 18);
            this.lbFileCounter.TabIndex = 5;
            this.lbFileCounter.Text = "폴더: 00개, 파일: 00개";
            this.lbFileCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbDownloadingCounter
            // 
            this.lbDownloadingCounter.Location = new System.Drawing.Point(774, 336);
            this.lbDownloadingCounter.Name = "lbDownloadingCounter";
            this.lbDownloadingCounter.Size = new System.Drawing.Size(158, 18);
            this.lbDownloadingCounter.TabIndex = 6;
            this.lbDownloadingCounter.Text = "다운로드 중 ... (00개)";
            this.lbDownloadingCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvFileDownloadingBrowser
            // 
            this.lvFileDownloadingBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvFileDownloadingBrowser.FullRowSelect = true;
            this.lvFileDownloadingBrowser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileDownloadingBrowser.HideSelection = false;
            this.lvFileDownloadingBrowser.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.lvFileDownloadingBrowser.Location = new System.Drawing.Point(12, 357);
            this.lvFileDownloadingBrowser.MultiSelect = false;
            this.lvFileDownloadingBrowser.Name = "lvFileDownloadingBrowser";
            this.lvFileDownloadingBrowser.Size = new System.Drawing.Size(920, 132);
            this.lvFileDownloadingBrowser.TabIndex = 7;
            this.lvFileDownloadingBrowser.UseCompatibleStateImageBehavior = false;
            this.lvFileDownloadingBrowser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "디렉토리";
            this.columnHeader1.Width = 786;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "진행률";
            this.columnHeader2.Width = 126;
            // 
            // FileBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.lvFileDownloadingBrowser);
            this.Controls.Add(this.lbDownloadingCounter);
            this.Controls.Add(this.lbFileCounter);
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.btRoot);
            this.Controls.Add(this.txtFakePath);
            this.Controls.Add(this.lvFileBrowser);
            this.Controls.Add(this.menuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuBar;
            this.Name = "FileBrowserForm";
            this.Text = "NAS File Browser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FileBrowserForm_FormClosed);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ctxMenuNoSelected.ResumeLayout(false);
            this.cxtMenuDirectoryViewer.ResumeLayout(false);
            this.ctxMenuFileSelected.ResumeLayout(false);
            this.ctxMenuFolderSelected.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.ListView lvFileBrowser;
        private System.Windows.Forms.TextBox txtFakePath;
        private System.Windows.Forms.Button btRoot;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.ColumnHeader hdName;
        private System.Windows.Forms.ColumnHeader hdGenDate;
        private System.Windows.Forms.ColumnHeader hdCapacity;
        private System.Windows.Forms.ContextMenuStrip ctxMenuFileSelected;
        private System.Windows.Forms.ContextMenuStrip ctxMenuFolderSelected;
        private System.Windows.Forms.ContextMenuStrip cxtMenuDirectoryViewer;
        private System.Windows.Forms.ToolStripMenuItem 루트경로로이동ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 상위경로로이동ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 다운로드ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 이동ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 삭제ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 파일업로드ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxMenuNoSelected;
        private System.Windows.Forms.ToolStripMenuItem 새폴더ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 파일업로드ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 새폴더ToolStripMenuItem1;
        private System.Windows.Forms.Label lbFileCounter;
        private System.Windows.Forms.Label lbDownloadingCounter;
        private System.Windows.Forms.ListView lvFileDownloadingBrowser;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}