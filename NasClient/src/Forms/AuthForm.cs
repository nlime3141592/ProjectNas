﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NAS
{
    public sealed partial class AuthForm : Form
    {
        private static AuthForm s_m_authForm;

        private AuthForm.FormMode m_formMode;

        public enum FormMode
        {
            Login,
            Registration,
            Start,
            Waiting
        }

        public static AuthForm GetForm(AuthForm.FormMode _formMode)
        {
            if (s_m_authForm == null)
                s_m_authForm = new AuthForm(_formMode);

            s_m_authForm.Focus();
            return s_m_authForm;
        }

        private AuthForm(AuthForm.FormMode _formMode)
        {
            InitializeComponent();

            m_formMode = _formMode;
        }

        private void m_ShowLoginMode()
        {
            txtLoginId.Clear();
            txtLoginPw.Clear();

            pnLogin.Location = new Point(220, 240);
            pnLogin.Show();
            pnRegistration.Hide();
            btStart.Hide();
            lbWaiting.Hide();
        }

        private void m_ShowRegistrationMode()
        {
            txtRegistrationId.Clear();
            txtRegistrationPw.Clear();

            pnRegistration.Location = new Point(220, 240);
            pnLogin.Hide();
            pnRegistration.Show();
            btStart.Hide();
            lbWaiting.Hide();
        }

        private void m_ShowStartMode()
        {
            btStart.Location = new Point(410, 280);
            pnLogin.Hide();
            pnRegistration.Hide();
            lbWaiting.Hide();
            btStart.Show();
        }

        private void m_ShowWaitingMode()
        {
            lbWaiting.Location = new Point(410, 280);
            pnLogin.Hide();
            pnRegistration.Hide();
            btStart.Hide();
            lbWaiting.Show();

            // ConnectionTask conTask = new ConnectionTask(m_OnTriedConnectionToServer);
            // conTask.thread.Start();
        }

        private void m_OnTriedConnectionToServer(bool _isSuccess)
        {
            if (_isSuccess)
                m_ShowLoginMode();
            else
            {
                m_ShowStartMode();
                MessageBox.Show(this, "서버 연결에 실패했습니다.", "NAS Authentication", MessageBoxButtons.OK);
            }
        }

        private void btBackToLogin_Click(object sender, EventArgs e)
        {
            m_ShowLoginMode();
        }

        private void lklbNewAccount_Click(object sender, EventArgs e)
        {
            m_ShowRegistrationMode();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            m_ShowWaitingMode();
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            switch(m_formMode)
            {
                case AuthForm.FormMode.Login:
                    m_ShowLoginMode();
                    break;
                case AuthForm.FormMode.Registration:
                    m_ShowRegistrationMode();
                    break;
                case AuthForm.FormMode.Start:
                    m_ShowStartMode();
                    break;
                case AuthForm.FormMode.Waiting:
                    m_ShowWaitingMode();
                    break;
                default:
                    break;
            }
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (s_m_authForm == this)
                s_m_authForm = null;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string id = txtLoginId.Text;
            string pw = txtLoginPw.Text;

            SvLogin service = new SvLogin(null, id, pw); // TODO: 클라이언트를 집어넣어야 함.
            FileStream fs = new FileStream(@"C:\NAS\tttt.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter wr = new StreamWriter(fs);
            wr.WriteLine(service.Execute());
            wr.WriteLine(service.response);
            wr.WriteLine(service.uuid);
            wr.Close();
            fs.Close();
            // string message = string.Format("수신 결과: {0} ({1})", service.response, service.uuid);
            // MessageBox.Show(message);
            // NasClientThread.GetInstance().RequestService(service);
            // new TestForm().Show(); // ok
        }
    }
}
