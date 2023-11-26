namespace NAS.Client
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
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "11",
            "22"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("2");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("3");
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvFileBrowser = new System.Windows.Forms.ListView();
            this.hdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdGenDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdCapacity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cxtMenuDirectoryViewer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.루트경로로이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.상위경로로이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btRoot = new System.Windows.Forms.Button();
            this.btBack = new System.Windows.Forms.Button();
            this.ctxMenuFileSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuFolderSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.다운로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이동ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.삭제ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.파일업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar.SuspendLayout();
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
            this.파일업로드ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
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
            this.lvFileBrowser.FullRowSelect = true;
            this.lvFileBrowser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileBrowser.HideSelection = false;
            this.lvFileBrowser.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.lvFileBrowser.Location = new System.Drawing.Point(174, 55);
            this.lvFileBrowser.MultiSelect = false;
            this.lvFileBrowser.Name = "lvFileBrowser";
            this.lvFileBrowser.Size = new System.Drawing.Size(758, 434);
            this.lvFileBrowser.TabIndex = 1;
            this.lvFileBrowser.UseCompatibleStateImageBehavior = false;
            this.lvFileBrowser.View = System.Windows.Forms.View.Details;
            this.lvFileBrowser.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFileBrowser_ItemSelectionChanged);
            // 
            // hdName
            // 
            this.hdName.Text = "이름";
            this.hdName.Width = 500;
            // 
            // hdGenDate
            // 
            this.hdGenDate.Text = "생성 날짜";
            this.hdGenDate.Width = 126;
            // 
            // hdCapacity
            // 
            this.hdCapacity.Text = "파일 크기";
            this.hdCapacity.Width = 126;
            // 
            // textBox1
            // 
            this.textBox1.ContextMenuStrip = this.cxtMenuDirectoryViewer;
            this.textBox1.Location = new System.Drawing.Point(174, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(758, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "C:\\NAS\\";
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
            // 
            // 상위경로로이동ToolStripMenuItem
            // 
            this.상위경로로이동ToolStripMenuItem.Name = "상위경로로이동ToolStripMenuItem";
            this.상위경로로이동ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.상위경로로이동ToolStripMenuItem.Text = "상위 경로로 이동";
            // 
            // btRoot
            // 
            this.btRoot.Location = new System.Drawing.Point(12, 26);
            this.btRoot.Name = "btRoot";
            this.btRoot.Size = new System.Drawing.Size(75, 23);
            this.btRoot.TabIndex = 3;
            this.btRoot.Text = "Root";
            this.btRoot.UseVisualStyleBackColor = true;
            // 
            // btBack
            // 
            this.btBack.Location = new System.Drawing.Point(93, 26);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 4;
            this.btBack.Text = "Back";
            this.btBack.UseVisualStyleBackColor = true;
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
            // ctxMenuFolderSelected
            // 
            this.ctxMenuFolderSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.이동ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.삭제ToolStripMenuItem1});
            this.ctxMenuFolderSelected.Name = "ctxMenuFileSelected";
            this.ctxMenuFolderSelected.Size = new System.Drawing.Size(99, 54);
            // 
            // 다운로드ToolStripMenuItem
            // 
            this.다운로드ToolStripMenuItem.Name = "다운로드ToolStripMenuItem";
            this.다운로드ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.다운로드ToolStripMenuItem.Text = "다운로드";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // 삭제ToolStripMenuItem
            // 
            this.삭제ToolStripMenuItem.Name = "삭제ToolStripMenuItem";
            this.삭제ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.삭제ToolStripMenuItem.Text = "삭제";
            // 
            // 이동ToolStripMenuItem
            // 
            this.이동ToolStripMenuItem.Name = "이동ToolStripMenuItem";
            this.이동ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.이동ToolStripMenuItem.Text = "이동";
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
            // 
            // 파일업로드ToolStripMenuItem
            // 
            this.파일업로드ToolStripMenuItem.Name = "파일업로드ToolStripMenuItem";
            this.파일업로드ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.파일업로드ToolStripMenuItem.Text = "파일 업로드";
            // 
            // FileBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.btRoot);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lvFileBrowser);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "FileBrowserForm";
            this.Text = "NAS File Browser";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
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
        private System.Windows.Forms.TextBox textBox1;
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
    }
}