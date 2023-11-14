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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileBrowserForm));
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.다른이름으로저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.파일삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.새폴더만들기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.폴더삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.디렉토리권한정보ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.편집ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.실행취소ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.다시실행ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.모두선택ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.계정관리자ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.계정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.내계정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.도구ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.사용자지정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.옵션ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.도움말ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.내용ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.인덱스ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.검색SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.정보ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_moveRoot = new System.Windows.Forms.Button();
            this.bt_moveBack = new System.Windows.Forms.Button();
            this.itxt_directory = new System.Windows.Forms.TextBox();
            this.list_files = new System.Windows.Forms.ListBox();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.편집ToolStripMenuItem,
            this.계정ToolStripMenuItem,
            this.도구ToolStripMenuItem,
            this.도움말ToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(944, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.저장ToolStripMenuItem,
            this.열기ToolStripMenuItem,
            this.다른이름으로저장ToolStripMenuItem,
            this.파일삭제ToolStripMenuItem,
            this.toolStripSeparator2,
            this.새폴더만들기ToolStripMenuItem,
            this.폴더삭제ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.디렉토리권한정보ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 저장ToolStripMenuItem
            // 
            this.저장ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("저장ToolStripMenuItem.Image")));
            this.저장ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.저장ToolStripMenuItem.Name = "저장ToolStripMenuItem";
            this.저장ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.저장ToolStripMenuItem.Text = "파일 다운로드";
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("열기ToolStripMenuItem.Image")));
            this.열기ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.열기ToolStripMenuItem.Text = "새 파일 업로드...";
            // 
            // 다른이름으로저장ToolStripMenuItem
            // 
            this.다른이름으로저장ToolStripMenuItem.Name = "다른이름으로저장ToolStripMenuItem";
            this.다른이름으로저장ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.다른이름으로저장ToolStripMenuItem.Text = "기존 파일 갱신...";
            // 
            // 파일삭제ToolStripMenuItem
            // 
            this.파일삭제ToolStripMenuItem.Name = "파일삭제ToolStripMenuItem";
            this.파일삭제ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.파일삭제ToolStripMenuItem.Text = "파일 삭제";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
            // 
            // 새폴더만들기ToolStripMenuItem
            // 
            this.새폴더만들기ToolStripMenuItem.Name = "새폴더만들기ToolStripMenuItem";
            this.새폴더만들기ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.새폴더만들기ToolStripMenuItem.Text = "새 폴더 만들기";
            // 
            // 폴더삭제ToolStripMenuItem
            // 
            this.폴더삭제ToolStripMenuItem.Name = "폴더삭제ToolStripMenuItem";
            this.폴더삭제ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.폴더삭제ToolStripMenuItem.Text = "폴더 삭제";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 6);
            // 
            // 디렉토리권한정보ToolStripMenuItem
            // 
            this.디렉토리권한정보ToolStripMenuItem.Name = "디렉토리권한정보ToolStripMenuItem";
            this.디렉토리권한정보ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.디렉토리권한정보ToolStripMenuItem.Text = "폴더 권한 정보";
            // 
            // 편집ToolStripMenuItem
            // 
            this.편집ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.실행취소ToolStripMenuItem,
            this.다시실행ToolStripMenuItem,
            this.toolStripSeparator3,
            this.모두선택ToolStripMenuItem,
            this.계정관리자ToolStripMenuItem});
            this.편집ToolStripMenuItem.Name = "편집ToolStripMenuItem";
            this.편집ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.편집ToolStripMenuItem.Text = "편집";
            // 
            // 실행취소ToolStripMenuItem
            // 
            this.실행취소ToolStripMenuItem.Name = "실행취소ToolStripMenuItem";
            this.실행취소ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.실행취소ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.실행취소ToolStripMenuItem.Text = "실행 취소";
            // 
            // 다시실행ToolStripMenuItem
            // 
            this.다시실행ToolStripMenuItem.Name = "다시실행ToolStripMenuItem";
            this.다시실행ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.다시실행ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.다시실행ToolStripMenuItem.Text = "다시 실행";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(164, 6);
            // 
            // 모두선택ToolStripMenuItem
            // 
            this.모두선택ToolStripMenuItem.Name = "모두선택ToolStripMenuItem";
            this.모두선택ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.모두선택ToolStripMenuItem.Text = "권한 관리자...";
            // 
            // 계정관리자ToolStripMenuItem
            // 
            this.계정관리자ToolStripMenuItem.Name = "계정관리자ToolStripMenuItem";
            this.계정관리자ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.계정관리자ToolStripMenuItem.Text = "계정 관리자...";
            // 
            // 계정ToolStripMenuItem
            // 
            this.계정ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.내계정ToolStripMenuItem});
            this.계정ToolStripMenuItem.Name = "계정ToolStripMenuItem";
            this.계정ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.계정ToolStripMenuItem.Text = "계정";
            // 
            // 내계정ToolStripMenuItem
            // 
            this.내계정ToolStripMenuItem.Name = "내계정ToolStripMenuItem";
            this.내계정ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.내계정ToolStripMenuItem.Text = "사용자 계정 정보";
            // 
            // 도구ToolStripMenuItem
            // 
            this.도구ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.사용자지정ToolStripMenuItem,
            this.옵션ToolStripMenuItem});
            this.도구ToolStripMenuItem.Name = "도구ToolStripMenuItem";
            this.도구ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.도구ToolStripMenuItem.Text = "환경설정";
            // 
            // 사용자지정ToolStripMenuItem
            // 
            this.사용자지정ToolStripMenuItem.Name = "사용자지정ToolStripMenuItem";
            this.사용자지정ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.사용자지정ToolStripMenuItem.Text = "사용자 지정";
            // 
            // 옵션ToolStripMenuItem
            // 
            this.옵션ToolStripMenuItem.Name = "옵션ToolStripMenuItem";
            this.옵션ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.옵션ToolStripMenuItem.Text = "옵션";
            // 
            // 도움말ToolStripMenuItem
            // 
            this.도움말ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.내용ToolStripMenuItem,
            this.인덱스ToolStripMenuItem,
            this.검색SToolStripMenuItem,
            this.toolStripSeparator5,
            this.정보ToolStripMenuItem});
            this.도움말ToolStripMenuItem.Name = "도움말ToolStripMenuItem";
            this.도움말ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.도움말ToolStripMenuItem.Text = "도움말";
            // 
            // 내용ToolStripMenuItem
            // 
            this.내용ToolStripMenuItem.Name = "내용ToolStripMenuItem";
            this.내용ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.내용ToolStripMenuItem.Text = "내용";
            // 
            // 인덱스ToolStripMenuItem
            // 
            this.인덱스ToolStripMenuItem.Name = "인덱스ToolStripMenuItem";
            this.인덱스ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.인덱스ToolStripMenuItem.Text = "인덱스";
            // 
            // 검색SToolStripMenuItem
            // 
            this.검색SToolStripMenuItem.Name = "검색SToolStripMenuItem";
            this.검색SToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.검색SToolStripMenuItem.Text = "검색(&S)";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(110, 6);
            // 
            // 정보ToolStripMenuItem
            // 
            this.정보ToolStripMenuItem.Name = "정보ToolStripMenuItem";
            this.정보ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.정보ToolStripMenuItem.Text = "정보...";
            // 
            // bt_moveRoot
            // 
            this.bt_moveRoot.Location = new System.Drawing.Point(12, 34);
            this.bt_moveRoot.Name = "bt_moveRoot";
            this.bt_moveRoot.Size = new System.Drawing.Size(21, 21);
            this.bt_moveRoot.TabIndex = 1;
            this.bt_moveRoot.Text = "O";
            this.bt_moveRoot.UseVisualStyleBackColor = true;
            this.bt_moveRoot.Click += new System.EventHandler(this.bt_moveRoot_Click);
            // 
            // bt_moveBack
            // 
            this.bt_moveBack.Location = new System.Drawing.Point(39, 34);
            this.bt_moveBack.Name = "bt_moveBack";
            this.bt_moveBack.Size = new System.Drawing.Size(21, 21);
            this.bt_moveBack.TabIndex = 2;
            this.bt_moveBack.Text = "<";
            this.bt_moveBack.UseVisualStyleBackColor = true;
            this.bt_moveBack.Click += new System.EventHandler(this.bt_moveBack_Click);
            // 
            // itxt_directory
            // 
            this.itxt_directory.Location = new System.Drawing.Point(66, 34);
            this.itxt_directory.Name = "itxt_directory";
            this.itxt_directory.ReadOnly = true;
            this.itxt_directory.Size = new System.Drawing.Size(866, 21);
            this.itxt_directory.TabIndex = 3;
            // 
            // list_files
            // 
            this.list_files.FormattingEnabled = true;
            this.list_files.ItemHeight = 12;
            this.list_files.Location = new System.Drawing.Point(12, 65);
            this.list_files.Name = "list_files";
            this.list_files.Size = new System.Drawing.Size(920, 424);
            this.list_files.TabIndex = 4;
            this.list_files.DoubleClick += new System.EventHandler(this.list_files_DoubleClick);
            this.list_files.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_files_KeyDown);
            // 
            // FileBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.list_files);
            this.Controls.Add(this.itxt_directory);
            this.Controls.Add(this.bt_moveBack);
            this.Controls.Add(this.bt_moveRoot);
            this.Controls.Add(this.menuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuBar;
            this.Name = "FileBrowserForm";
            this.Text = "파일 탐색기";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FileBrowserForm_FormClosed);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 열기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 다른이름으로저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 편집ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 실행취소ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 다시실행ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 모두선택ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 도구ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 사용자지정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 옵션ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 도움말ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 내용ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 인덱스ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 검색SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 정보ToolStripMenuItem;
        private System.Windows.Forms.Button bt_moveRoot;
        private System.Windows.Forms.Button bt_moveBack;
        private System.Windows.Forms.TextBox itxt_directory;
        private System.Windows.Forms.ListBox list_files;
        private System.Windows.Forms.ToolStripMenuItem 파일삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 새폴더만들기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 폴더삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 디렉토리권한정보ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 계정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 내계정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 계정관리자ToolStripMenuItem;
    }
}