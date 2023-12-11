namespace NAS
{
    partial class WriteFileNameForm
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.lbInfo = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.rbtDepartment = new System.Windows.Forms.RadioButton();
            this.rbtAll = new System.Windows.Forms.RadioButton();
            this.cbxPermissionLevel = new System.Windows.Forms.ComboBox();
            this.lvPermissionLevel = new System.Windows.Forms.Label();
            this.gbPermission = new System.Windows.Forms.GroupBox();
            this.gbPermission.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(347, 138);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "취소";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(347, 109);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "확인";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(12, 19);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(264, 12);
            this.lbInfo.TabIndex = 2;
            this.lbInfo.Text = "새로운 파일 이름을 입력하세요. (1~128자 이내)";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(12, 34);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(410, 21);
            this.txtFileName.TabIndex = 3;
            // 
            // rbtDepartment
            // 
            this.rbtDepartment.AutoSize = true;
            this.rbtDepartment.Checked = true;
            this.rbtDepartment.Location = new System.Drawing.Point(6, 20);
            this.rbtDepartment.Name = "rbtDepartment";
            this.rbtDepartment.Size = new System.Drawing.Size(167, 16);
            this.rbtDepartment.TabIndex = 4;
            this.rbtDepartment.TabStop = true;
            this.rbtDepartment.Text = "같은 부서의 직원에게 공유";
            this.rbtDepartment.UseVisualStyleBackColor = true;
            // 
            // rbtAll
            // 
            this.rbtAll.AutoSize = true;
            this.rbtAll.Location = new System.Drawing.Point(6, 62);
            this.rbtAll.Name = "rbtAll";
            this.rbtAll.Size = new System.Drawing.Size(127, 16);
            this.rbtAll.TabIndex = 5;
            this.rbtAll.Text = "모든 직원에게 공유";
            this.rbtAll.UseVisualStyleBackColor = true;
            // 
            // cbxPermissionLevel
            // 
            this.cbxPermissionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPermissionLevel.FormattingEnabled = true;
            this.cbxPermissionLevel.Location = new System.Drawing.Point(92, 37);
            this.cbxPermissionLevel.Name = "cbxPermissionLevel";
            this.cbxPermissionLevel.Size = new System.Drawing.Size(76, 20);
            this.cbxPermissionLevel.TabIndex = 7;
            // 
            // lvPermissionLevel
            // 
            this.lvPermissionLevel.AutoSize = true;
            this.lvPermissionLevel.Location = new System.Drawing.Point(23, 42);
            this.lvPermissionLevel.Name = "lvPermissionLevel";
            this.lvPermissionLevel.Size = new System.Drawing.Size(65, 12);
            this.lvPermissionLevel.TabIndex = 8;
            this.lvPermissionLevel.Text = "권한 레벨 :";
            // 
            // gbPermission
            // 
            this.gbPermission.Controls.Add(this.rbtDepartment);
            this.gbPermission.Controls.Add(this.rbtAll);
            this.gbPermission.Controls.Add(this.cbxPermissionLevel);
            this.gbPermission.Controls.Add(this.lvPermissionLevel);
            this.gbPermission.Location = new System.Drawing.Point(14, 70);
            this.gbPermission.Name = "gbPermission";
            this.gbPermission.Size = new System.Drawing.Size(200, 91);
            this.gbPermission.TabIndex = 9;
            this.gbPermission.TabStop = false;
            this.gbPermission.Text = "권한 설정";
            // 
            // WriteFileNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 173);
            this.Controls.Add(this.gbPermission);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "WriteFileNameForm";
            this.Text = "파일 이름 입력";
            this.gbPermission.ResumeLayout(false);
            this.gbPermission.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.RadioButton rbtDepartment;
        private System.Windows.Forms.RadioButton rbtAll;
        private System.Windows.Forms.ComboBox cbxPermissionLevel;
        private System.Windows.Forms.Label lvPermissionLevel;
        private System.Windows.Forms.GroupBox gbPermission;
    }
}