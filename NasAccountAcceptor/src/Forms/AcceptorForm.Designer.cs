namespace NAS
{
    partial class AcceptorForm
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
            this.lvAccounts = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbAccountInfo = new System.Windows.Forms.Label();
            this.btDeny = new System.Windows.Forms.Button();
            this.btAccept = new System.Windows.Forms.Button();
            this.cbxLevel = new System.Windows.Forms.ComboBox();
            this.cbxDepartment = new System.Windows.Forms.ComboBox();
            this.lbDepartment = new System.Windows.Forms.Label();
            this.lbLevel = new System.Windows.Forms.Label();
            this.lbAcceptInfo = new System.Windows.Forms.Label();
            this.txtUuid = new System.Windows.Forms.TextBox();
            this.lbUuid = new System.Windows.Forms.Label();
            this.btRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvAccounts
            // 
            this.lvAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvAccounts.LabelWrap = false;
            this.lvAccounts.Location = new System.Drawing.Point(12, 24);
            this.lvAccounts.MultiSelect = false;
            this.lvAccounts.Name = "lvAccounts";
            this.lvAccounts.Size = new System.Drawing.Size(685, 465);
            this.lvAccounts.TabIndex = 0;
            this.lvAccounts.TabStop = false;
            this.lvAccounts.UseCompatibleStateImageBehavior = false;
            this.lvAccounts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "계정 고유 번호";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "이름";
            this.columnHeader2.Width = 180;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "아이디";
            this.columnHeader3.Width = 180;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "가입 날짜";
            this.columnHeader4.Width = 180;
            // 
            // lbAccountInfo
            // 
            this.lbAccountInfo.AutoSize = true;
            this.lbAccountInfo.Location = new System.Drawing.Point(10, 9);
            this.lbAccountInfo.Name = "lbAccountInfo";
            this.lbAccountInfo.Size = new System.Drawing.Size(141, 12);
            this.lbAccountInfo.TabIndex = 1;
            this.lbAccountInfo.Text = "가입 승인 대기 계정 목록";
            // 
            // btDeny
            // 
            this.btDeny.Location = new System.Drawing.Point(857, 466);
            this.btDeny.Name = "btDeny";
            this.btDeny.Size = new System.Drawing.Size(75, 23);
            this.btDeny.TabIndex = 2;
            this.btDeny.Text = "가입 거절";
            this.btDeny.UseVisualStyleBackColor = true;
            this.btDeny.Click += new System.EventHandler(this.btDeny_Click);
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(857, 437);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 3;
            this.btAccept.Text = "가입 승인";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // cbxLevel
            // 
            this.cbxLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLevel.FormattingEnabled = true;
            this.cbxLevel.Location = new System.Drawing.Point(778, 100);
            this.cbxLevel.Name = "cbxLevel";
            this.cbxLevel.Size = new System.Drawing.Size(154, 20);
            this.cbxLevel.TabIndex = 4;
            // 
            // cbxDepartment
            // 
            this.cbxDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDepartment.FormattingEnabled = true;
            this.cbxDepartment.Location = new System.Drawing.Point(778, 74);
            this.cbxDepartment.Name = "cbxDepartment";
            this.cbxDepartment.Size = new System.Drawing.Size(154, 20);
            this.cbxDepartment.TabIndex = 5;
            // 
            // lbDepartment
            // 
            this.lbDepartment.AutoSize = true;
            this.lbDepartment.Location = new System.Drawing.Point(706, 79);
            this.lbDepartment.Name = "lbDepartment";
            this.lbDepartment.Size = new System.Drawing.Size(29, 12);
            this.lbDepartment.TabIndex = 6;
            this.lbDepartment.Text = "부서";
            // 
            // lbLevel
            // 
            this.lbLevel.AutoSize = true;
            this.lbLevel.Location = new System.Drawing.Point(706, 105);
            this.lbLevel.Name = "lbLevel";
            this.lbLevel.Size = new System.Drawing.Size(57, 12);
            this.lbLevel.TabIndex = 7;
            this.lbLevel.Text = "권한 레벨";
            // 
            // lbAcceptInfo
            // 
            this.lbAcceptInfo.Location = new System.Drawing.Point(778, 27);
            this.lbAcceptInfo.Name = "lbAcceptInfo";
            this.lbAcceptInfo.Size = new System.Drawing.Size(154, 17);
            this.lbAcceptInfo.TabIndex = 8;
            this.lbAcceptInfo.Text = "<가입 승인자 정보 입력>";
            this.lbAcceptInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtUuid
            // 
            this.txtUuid.Location = new System.Drawing.Point(778, 47);
            this.txtUuid.Name = "txtUuid";
            this.txtUuid.Size = new System.Drawing.Size(154, 21);
            this.txtUuid.TabIndex = 9;
            // 
            // lbUuid
            // 
            this.lbUuid.AutoSize = true;
            this.lbUuid.Location = new System.Drawing.Point(706, 51);
            this.lbUuid.Name = "lbUuid";
            this.lbUuid.Size = new System.Drawing.Size(57, 12);
            this.lbUuid.TabIndex = 10;
            this.lbUuid.Text = "고유 번호";
            // 
            // btRefresh
            // 
            this.btRefresh.Location = new System.Drawing.Point(708, 466);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(75, 23);
            this.btRefresh.TabIndex = 11;
            this.btRefresh.Text = "새로 고침";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // AcceptorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.lbUuid);
            this.Controls.Add(this.txtUuid);
            this.Controls.Add(this.lbAcceptInfo);
            this.Controls.Add(this.lbLevel);
            this.Controls.Add(this.lbDepartment);
            this.Controls.Add(this.cbxDepartment);
            this.Controls.Add(this.cbxLevel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.btDeny);
            this.Controls.Add(this.lbAccountInfo);
            this.Controls.Add(this.lvAccounts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AcceptorForm";
            this.Text = "NAS Account Acceptor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AcceptorForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvAccounts;
        private System.Windows.Forms.Label lbAccountInfo;
        private System.Windows.Forms.Button btDeny;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.ComboBox cbxLevel;
        private System.Windows.Forms.ComboBox cbxDepartment;
        private System.Windows.Forms.Label lbDepartment;
        private System.Windows.Forms.Label lbLevel;
        private System.Windows.Forms.Label lbAcceptInfo;
        private System.Windows.Forms.TextBox txtUuid;
        private System.Windows.Forms.Label lbUuid;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btRefresh;
    }
}