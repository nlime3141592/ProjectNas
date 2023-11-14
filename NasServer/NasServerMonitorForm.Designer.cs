namespace NAS.Server
{
    partial class NasServerMonitorForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_FileBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.itxt_command = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_Console = new System.Windows.Forms.TabPage();
            this.tab_Logger = new System.Windows.Forms.TabPage();
            this.itxt_console = new System.Windows.Forms.TextBox();
            this.itxt_logger = new System.Windows.Forms.TextBox();
            this.menuBar.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_Console.SuspendLayout();
            this.tab_Logger.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(944, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_FileBrowser});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // menuItem_FileBrowser
            // 
            this.menuItem_FileBrowser.Name = "menuItem_FileBrowser";
            this.menuItem_FileBrowser.Size = new System.Drawing.Size(180, 22);
            this.menuItem_FileBrowser.Text = "파일 탐색기...";
            this.menuItem_FileBrowser.Click += new System.EventHandler(this.menuItem_FileBrowser_Click);
            // 
            // itxt_command
            // 
            this.itxt_command.BackColor = System.Drawing.SystemColors.WindowText;
            this.itxt_command.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.itxt_command.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itxt_command.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.itxt_command.Location = new System.Drawing.Point(16, 468);
            this.itxt_command.Name = "itxt_command";
            this.itxt_command.Size = new System.Drawing.Size(912, 26);
            this.itxt_command.TabIndex = 2;
            this.itxt_command.TabStop = false;
            this.itxt_command.Text = "command here...";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_Console);
            this.tabControl1.Controls.Add(this.tab_Logger);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(920, 435);
            this.tabControl1.TabIndex = 3;
            // 
            // tab_Console
            // 
            this.tab_Console.Controls.Add(this.itxt_console);
            this.tab_Console.Location = new System.Drawing.Point(4, 22);
            this.tab_Console.Name = "tab_Console";
            this.tab_Console.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Console.Size = new System.Drawing.Size(912, 409);
            this.tab_Console.TabIndex = 0;
            this.tab_Console.Text = "Console";
            this.tab_Console.UseVisualStyleBackColor = true;
            // 
            // tab_Logger
            // 
            this.tab_Logger.Controls.Add(this.itxt_logger);
            this.tab_Logger.Location = new System.Drawing.Point(4, 22);
            this.tab_Logger.Name = "tab_Logger";
            this.tab_Logger.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Logger.Size = new System.Drawing.Size(912, 409);
            this.tab_Logger.TabIndex = 1;
            this.tab_Logger.Text = "Logger";
            this.tab_Logger.UseVisualStyleBackColor = true;
            // 
            // itxt_console
            // 
            this.itxt_console.BackColor = System.Drawing.SystemColors.WindowText;
            this.itxt_console.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itxt_console.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.itxt_console.Location = new System.Drawing.Point(0, 0);
            this.itxt_console.Multiline = true;
            this.itxt_console.Name = "itxt_console";
            this.itxt_console.ReadOnly = true;
            this.itxt_console.Size = new System.Drawing.Size(912, 409);
            this.itxt_console.TabIndex = 0;
            this.itxt_console.TabStop = false;
            this.itxt_console.Text = "----- NAS SERVER MONITOR -----\r\n| Welcome to NAS Server!     |\r\n-----------------" +
    "-------------";
            // 
            // itxt_logger
            // 
            this.itxt_logger.BackColor = System.Drawing.SystemColors.WindowText;
            this.itxt_logger.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itxt_logger.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.itxt_logger.Location = new System.Drawing.Point(0, 0);
            this.itxt_logger.Multiline = true;
            this.itxt_logger.Name = "itxt_logger";
            this.itxt_logger.ReadOnly = true;
            this.itxt_logger.Size = new System.Drawing.Size(912, 409);
            this.itxt_logger.TabIndex = 0;
            this.itxt_logger.TabStop = false;
            this.itxt_logger.Text = "UPDATED    - C:\\NAS\\fs.txt\r\nDELETED    - C:\\NAS\\fs.txt\r\nCREATED    - C:\\NAS\\fs.tx" +
    "t\r\nDOWNLOADED - C:\\NAS\\fs.txt";
            // 
            // NasServerMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.itxt_command);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "NasServerMonitorForm";
            this.Text = "NAS Server Monitor";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tab_Console.ResumeLayout(false);
            this.tab_Console.PerformLayout();
            this.tab_Logger.ResumeLayout(false);
            this.tab_Logger.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_FileBrowser;
        private System.Windows.Forms.TextBox itxt_command;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_Console;
        private System.Windows.Forms.TextBox itxt_console;
        private System.Windows.Forms.TabPage tab_Logger;
        private System.Windows.Forms.TextBox itxt_logger;
    }
}

