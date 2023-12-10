using System;
using System.Drawing;
using System.Threading.Tasks;
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

        public static AuthForm GetForm()
        {
            if (s_m_authForm == null)
            {
                s_m_authForm = new AuthForm();
                s_m_authForm.ctChangeFormMode(FormMode.Waiting);
            }

            return s_m_authForm;
        }

        private AuthForm()
        {
            InitializeComponent();

            pnLogin.Hide();
            pnRegistration.Hide();
            btStart.Hide();
            lbWaiting.Hide();
        }

        public void ctChangeFormMode(AuthForm.FormMode _formMode)
        {
            m_formMode = _formMode;

            switch (m_formMode)
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

        public void ctShow()
        {
            void _Show()
            {
                this.Show();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        public void ctHide()
        {
            void _Hide()
            {
                this.Hide();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Hide));
            else
                _Hide();
        }

        private void m_ShowLoginMode()
        {
            void _Show()
            {
                txtLoginId.Clear();
                txtLoginPw.Clear();

                pnLogin.Location = new Point(220, 240);
                pnLogin.Show();
                pnRegistration.Hide();
                btStart.Hide();
                lbWaiting.Hide();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_ShowRegistrationMode()
        {
            void _Show()
            {
                txtRegistrationId.Clear();
                txtRegistrationPw.Clear();

                pnRegistration.Location = new Point(220, 240);
                pnLogin.Hide();
                pnRegistration.Show();
                btStart.Hide();
                lbWaiting.Hide();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_ShowStartMode()
        {
            void _Show()
            {
                btStart.Location = new Point(410, 280);
                pnLogin.Hide();
                pnRegistration.Hide();
                lbWaiting.Hide();
                btStart.Show();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_ShowWaitingMode()
        {
            void _Show()
            {
                lbWaiting.Location = new Point(410, 280);
                pnLogin.Hide();
                pnRegistration.Hide();
                btStart.Hide();
                lbWaiting.Show();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();

            Task.Run(() =>
            {
                m_OnTriedConnectionToServer(NasClientProgram.TryConnectToServer());
            });
        }

        private void m_OnTriedConnectionToServer(bool _isSuccess)
        {
            void _Show()
            {
                if (_isSuccess)
                    m_ShowLoginMode();
                else
                {
                    m_ShowStartMode();
                    MessageBox.Show(this, "서버 연결에 실패했습니다.", "NAS Authentication", MessageBoxButtons.OK);
                }
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void btBackToLogin_Click(object sender, EventArgs e)
        {
            m_ShowLoginMode();
        }

        private void lklbNewAccount_Click(object sender, EventArgs e)
        {
            m_ShowRegistrationMode();
        }

        private void lklbFoundPw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, "미구현 기능입니다.", "AuthForm");
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            m_ShowWaitingMode();
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (s_m_authForm == this)
            {
                s_m_authForm = null;
                NasClientProgram.GetClient().TryHalt();
            }
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            void _Execute()
            {
                string id = txtLoginId.Text;
                string pw = txtLoginPw.Text;

                CSvLogin service = new CSvLogin(NasClientProgram.GetClient(), id, pw); // TODO: 클라이언트를 집어넣어야 함.
                service.onLoginSuccess = m_OnLoginSuccess;
                service.onInvalidAccount = m_OnInvalidAccount;
                service.onNotAcceptedAccount = m_OnNotAcceptedAccount;
                service.onError = m_OnNetworkError;
                NasClientProgram.GetClient().Request(service);
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Execute));
            else
                _Execute();
        }

        private void btRegistration_Click(object sender, EventArgs e)
        {
            void _ShowError()
            {
                MessageBox.Show(this, "id, pw를 확인하세요.", "AuthForm");
            }

            void _Execute()
            {
                string id = txtRegistrationId.Text;
                string pw = txtRegistrationPw.Text;

                if(id.Length == 0 || pw.Length == 0)
                {
                    _ShowError();
                    return;
                }

                CSvJoin service = new CSvJoin(NasClientProgram.GetClient(), id, pw); // TODO: 클라이언트를 집어넣어야 함.
                service.onJoinSuccess = m_OnJoinSuccess;
                service.onJoinFailure = m_OnJoinFailure;
                service.onError = m_OnNetworkError;
                NasClientProgram.GetClient().Request(service);
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Execute));
            else
                _Execute();
        }

        private void m_OnLoginSuccess()
        {
            void _Show()
            {
                FileBrowserForm.GetForm().ctShow();
            }

            this.ctHide();

            if(this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidAccount()
        {
            void _Show()
            {
                MessageBox.Show(this, "로그인에 실패했습니다. 정보를 확인하세요.", "AuthForm");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnNotAcceptedAccount()
        {
            void _Show()
            {
                MessageBox.Show(this, "승인되지 않은 계정입니다. 관리자에게 문의하세요.", "AuthForm");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnJoinSuccess()
        {
            void _Show()
            {
                MessageBox.Show(this, "회원가입 성공!", "AuthForm");
            }

            this.ctChangeFormMode(AuthForm.FormMode.Login);

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnJoinFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "중복된 아이디가 존재합니다.", "AuthForm");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnNetworkError()
        {
            NasClientProgram.GetClient().TryHalt();
            this.ctChangeFormMode(AuthForm.FormMode.Start);
        }
    }
}
