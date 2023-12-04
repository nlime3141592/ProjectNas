﻿namespace NAS.Client
{
    partial class AuthForm
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
            this.lbTitle = new System.Windows.Forms.Label();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.btLogin = new System.Windows.Forms.Button();
            this.lklbFoundPw = new System.Windows.Forms.LinkLabel();
            this.lklbNewAccount = new System.Windows.Forms.LinkLabel();
            this.lbPw = new System.Windows.Forms.Label();
            this.lbId = new System.Windows.Forms.Label();
            this.txtLoginPw = new System.Windows.Forms.TextBox();
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.pnRegistration = new System.Windows.Forms.Panel();
            this.lbRegistrationMessage = new System.Windows.Forms.Label();
            this.btBackToLogin = new System.Windows.Forms.Button();
            this.btRegistration = new System.Windows.Forms.Button();
            this.lbRegistrationPw = new System.Windows.Forms.Label();
            this.lbRegistrationId = new System.Windows.Forms.Label();
            this.txtRegistrationPw = new System.Windows.Forms.TextBox();
            this.txtRegistrationId = new System.Windows.Forms.TextBox();
            this.btStart = new System.Windows.Forms.Button();
            this.lbWaiting = new System.Windows.Forms.Label();
            this.pnLogin.SuspendLayout();
            this.pnRegistration.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.Font = new System.Drawing.Font("굴림", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(19, 16);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(1446, 399);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Local Storage For Client";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnLogin
            // 
            this.pnLogin.Controls.Add(this.btLogin);
            this.pnLogin.Controls.Add(this.lklbFoundPw);
            this.pnLogin.Controls.Add(this.lklbNewAccount);
            this.pnLogin.Controls.Add(this.lbPw);
            this.pnLogin.Controls.Add(this.lbId);
            this.pnLogin.Controls.Add(this.txtLoginPw);
            this.pnLogin.Controls.Add(this.txtLoginId);
            this.pnLogin.Location = new System.Drawing.Point(346, 420);
            this.pnLogin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(817, 245);
            this.pnLogin.TabIndex = 1;
            // 
            // btLogin
            // 
            this.btLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btLogin.Location = new System.Drawing.Point(611, 152);
            this.btLogin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(118, 40);
            this.btLogin.TabIndex = 6;
            this.btLogin.Text = "로그인";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // lklbFoundPw
            // 
            this.lklbFoundPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lklbFoundPw.AutoSize = true;
            this.lklbFoundPw.Location = new System.Drawing.Point(286, 163);
            this.lklbFoundPw.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lklbFoundPw.Name = "lklbFoundPw";
            this.lklbFoundPw.Size = new System.Drawing.Size(259, 21);
            this.lklbFoundPw.TabIndex = 5;
            this.lklbFoundPw.TabStop = true;
            this.lklbFoundPw.Text = "비밀번호를 잊어버렸나요?";
            // 
            // lklbNewAccount
            // 
            this.lklbNewAccount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lklbNewAccount.AutoSize = true;
            this.lklbNewAccount.Location = new System.Drawing.Point(143, 163);
            this.lklbNewAccount.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lklbNewAccount.Name = "lklbNewAccount";
            this.lklbNewAccount.Size = new System.Drawing.Size(150, 21);
            this.lklbNewAccount.TabIndex = 4;
            this.lklbNewAccount.TabStop = true;
            this.lklbNewAccount.Text = "새 계정 만들기";
            this.lklbNewAccount.Click += new System.EventHandler(this.lklbNewAccount_Click);
            // 
            // lbPw
            // 
            this.lbPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbPw.AutoSize = true;
            this.lbPw.Location = new System.Drawing.Point(88, 112);
            this.lbPw.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbPw.Name = "lbPw";
            this.lbPw.Size = new System.Drawing.Size(53, 21);
            this.lbPw.TabIndex = 3;
            this.lbPw.Text = "PW :";
            this.lbPw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbId
            // 
            this.lbId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(99, 65);
            this.lbId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(40, 21);
            this.lbId.TabIndex = 2;
            this.lbId.Text = "ID :";
            this.lbId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLoginPw
            // 
            this.txtLoginPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLoginPw.Location = new System.Drawing.Point(146, 102);
            this.txtLoginPw.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtLoginPw.Name = "txtLoginPw";
            this.txtLoginPw.PasswordChar = '*';
            this.txtLoginPw.Size = new System.Drawing.Size(581, 32);
            this.txtLoginPw.TabIndex = 1;
            // 
            // txtLoginId
            // 
            this.txtLoginId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLoginId.Location = new System.Drawing.Point(146, 54);
            this.txtLoginId.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.Size = new System.Drawing.Size(581, 32);
            this.txtLoginId.TabIndex = 0;
            // 
            // pnRegistration
            // 
            this.pnRegistration.Controls.Add(this.lbRegistrationMessage);
            this.pnRegistration.Controls.Add(this.btBackToLogin);
            this.pnRegistration.Controls.Add(this.btRegistration);
            this.pnRegistration.Controls.Add(this.lbRegistrationPw);
            this.pnRegistration.Controls.Add(this.lbRegistrationId);
            this.pnRegistration.Controls.Add(this.txtRegistrationPw);
            this.pnRegistration.Controls.Add(this.txtRegistrationId);
            this.pnRegistration.Location = new System.Drawing.Point(19, 611);
            this.pnRegistration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnRegistration.Name = "pnRegistration";
            this.pnRegistration.Size = new System.Drawing.Size(817, 245);
            this.pnRegistration.TabIndex = 7;
            // 
            // lbRegistrationMessage
            // 
            this.lbRegistrationMessage.AutoSize = true;
            this.lbRegistrationMessage.Location = new System.Drawing.Point(145, 163);
            this.lbRegistrationMessage.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbRegistrationMessage.Name = "lbRegistrationMessage";
            this.lbRegistrationMessage.Size = new System.Drawing.Size(325, 21);
            this.lbRegistrationMessage.TabIndex = 8;
            this.lbRegistrationMessage.Text = "아이디와 비밀번호를 입력하세요.";
            // 
            // btBackToLogin
            // 
            this.btBackToLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btBackToLogin.Location = new System.Drawing.Point(484, 152);
            this.btBackToLogin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btBackToLogin.Name = "btBackToLogin";
            this.btBackToLogin.Size = new System.Drawing.Size(118, 40);
            this.btBackToLogin.TabIndex = 7;
            this.btBackToLogin.Text = "뒤로";
            this.btBackToLogin.UseVisualStyleBackColor = true;
            this.btBackToLogin.Click += new System.EventHandler(this.btBackToLogin_Click);
            // 
            // btRegistration
            // 
            this.btRegistration.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btRegistration.Location = new System.Drawing.Point(611, 152);
            this.btRegistration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btRegistration.Name = "btRegistration";
            this.btRegistration.Size = new System.Drawing.Size(118, 40);
            this.btRegistration.TabIndex = 6;
            this.btRegistration.Text = "회원가입";
            this.btRegistration.UseVisualStyleBackColor = true;
            // 
            // lbRegistrationPw
            // 
            this.lbRegistrationPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbRegistrationPw.AutoSize = true;
            this.lbRegistrationPw.Location = new System.Drawing.Point(88, 112);
            this.lbRegistrationPw.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbRegistrationPw.Name = "lbRegistrationPw";
            this.lbRegistrationPw.Size = new System.Drawing.Size(53, 21);
            this.lbRegistrationPw.TabIndex = 3;
            this.lbRegistrationPw.Text = "PW :";
            this.lbRegistrationPw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbRegistrationId
            // 
            this.lbRegistrationId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbRegistrationId.AutoSize = true;
            this.lbRegistrationId.Location = new System.Drawing.Point(50, 65);
            this.lbRegistrationId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbRegistrationId.Name = "lbRegistrationId";
            this.lbRegistrationId.Size = new System.Drawing.Size(88, 21);
            this.lbRegistrationId.TabIndex = 2;
            this.lbRegistrationId.Text = "NEW ID :";
            this.lbRegistrationId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRegistrationPw
            // 
            this.txtRegistrationPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtRegistrationPw.Location = new System.Drawing.Point(146, 102);
            this.txtRegistrationPw.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtRegistrationPw.Name = "txtRegistrationPw";
            this.txtRegistrationPw.PasswordChar = '*';
            this.txtRegistrationPw.Size = new System.Drawing.Size(581, 32);
            this.txtRegistrationPw.TabIndex = 1;
            // 
            // txtRegistrationId
            // 
            this.txtRegistrationId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtRegistrationId.Location = new System.Drawing.Point(146, 54);
            this.txtRegistrationId.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtRegistrationId.Name = "txtRegistrationId";
            this.txtRegistrationId.Size = new System.Drawing.Size(581, 32);
            this.txtRegistrationId.TabIndex = 0;
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(1201, 749);
            this.btStart.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(220, 70);
            this.btStart.TabIndex = 8;
            this.btStart.Text = "시작하기";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // lbWaiting
            // 
            this.lbWaiting.Font = new System.Drawing.Font("굴림", 9F);
            this.lbWaiting.Location = new System.Drawing.Point(943, 763);
            this.lbWaiting.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbWaiting.Name = "lbWaiting";
            this.lbWaiting.Size = new System.Drawing.Size(220, 70);
            this.lbWaiting.TabIndex = 9;
            this.lbWaiting.Text = "서버에 연결 중...";
            this.lbWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1483, 877);
            this.Controls.Add(this.lbWaiting);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.pnRegistration);
            this.Controls.Add(this.pnLogin);
            this.Controls.Add(this.lbTitle);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "AuthForm";
            this.Text = "NAS Authentication";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AuthForm_FormClosed);
            this.Load += new System.EventHandler(this.AuthForm_Load);
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            this.pnRegistration.ResumeLayout(false);
            this.pnRegistration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel pnLogin;
        private System.Windows.Forms.Label lbPw;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.TextBox txtLoginPw;
        private System.Windows.Forms.TextBox txtLoginId;
        private System.Windows.Forms.LinkLabel lklbFoundPw;
        private System.Windows.Forms.LinkLabel lklbNewAccount;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Panel pnRegistration;
        private System.Windows.Forms.Button btBackToLogin;
        private System.Windows.Forms.Button btRegistration;
        private System.Windows.Forms.Label lbRegistrationPw;
        private System.Windows.Forms.Label lbRegistrationId;
        private System.Windows.Forms.TextBox txtRegistrationPw;
        private System.Windows.Forms.TextBox txtRegistrationId;
        private System.Windows.Forms.Label lbRegistrationMessage;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Label lbWaiting;
    }
}
