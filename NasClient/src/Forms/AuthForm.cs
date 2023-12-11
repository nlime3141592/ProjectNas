using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS
{
    public sealed partial class AuthForm : Form
    {
        private static AuthForm s_m_authForm;

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
                s_m_authForm = new AuthForm(AuthForm.FormMode.Waiting);

            return s_m_authForm;
        }

        private AuthForm(AuthForm.FormMode _formMode)
        {
            InitializeComponent();

            pnLogin.Hide();
            pnRegistration.Hide();
            btStart.Hide();
            lbWaiting.Hide();

            ctChangeFormMode(_formMode);
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

        #region Change Form Mode
        public void ctChangeFormMode(AuthForm.FormMode _formMode)
        {
            switch (_formMode)
            {
                case AuthForm.FormMode.Login:
                    m_ctShowLoginMode();
                    break;
                case AuthForm.FormMode.Registration:
                    m_ctShowRegistrationMode();
                    break;
                case AuthForm.FormMode.Start:
                    m_ctShowStartMode();
                    break;
                case AuthForm.FormMode.Waiting:
                    m_ctShowWaitingMode();
                    break;
                default:
                    break;
            }
        }

        private void m_ctShowLoginMode()
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

        private void m_ctShowRegistrationMode()
        {
            void _Show()
            {
                txtRegistrationId.Clear();
                txtRegistrationPw.Clear();
                txtRegistrationName.Clear();

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

        private void m_ctShowStartMode()
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

        private void m_ctShowWaitingMode()
        {
            void _Show()
            {
                lbWaiting.Location = new Point(410, 280);
                pnLogin.Hide();
                pnRegistration.Hide();
                btStart.Hide();
                lbWaiting.Show();
            }

            void _ShowError()
            {
                MessageBox.Show(this, "서버 연결에 실패했습니다.", "NAS Server");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();

            Task.Run(() =>
            {
                if (NasClient.TryConnectToServer())
                {
                    m_ctShowLoginMode();
                }
                else
                {
                    m_ctShowStartMode();

                    if (this.InvokeRequired)
                        this.Invoke(new Action(_ShowError));
                    else
                        _ShowError();
                }
            });
        }
        #endregion

        #region Button Events
        private void btBackToLogin_Click(object sender, EventArgs e)
        {
            m_ctShowLoginMode();
        }

        private void lklbNewAccount_Click(object sender, EventArgs e)
        {
            m_ctShowRegistrationMode();
        }

        private void lklbFoundPw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, "미구현 기능입니다.", "NAS Server");
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            m_ctShowWaitingMode();
        }

        // NOTE: 로그인 버튼을 눌렀을 때
        private void btLogin_Click(object sender, EventArgs e)
        {
            string id = txtLoginId.Text;
            string pw = txtLoginPw.Text;

            CSvLogin service = new CSvLogin(NasClient.instance, id, pw); // TODO: 클라이언트를 집어넣어야 함.
            service.onLoginSuccess = m_OnLoginSuccess;
            service.onInvalidAccount = m_OnInvalidAccount;
            service.onNotAcceptedAccount = m_OnNotAcceptedAccount;
            NasClient.instance.Request(service);
        }

        // NOTE: 회원가입 버튼을 눌렀을 때
        private void btRegistration_Click(object sender, EventArgs e)
        {
            string id = txtRegistrationId.Text;
            string pw = txtRegistrationPw.Text;
            string name = txtRegistrationName.Text;

            if (id.Length == 0 || pw.Length == 0 || name.Length == 0)
            {
                MessageBox.Show(this, "아이디, 패스워드, 이름은 필수 입력입니다.", "NAS Server");
                return;
            }

            CSvJoin service = new CSvJoin(NasClient.instance, id, pw, name);
            service.onJoinSuccess = m_OnJoinSuccess;
            service.onJoinFailure = m_OnJoinFailure;
            NasClient.instance.Request(service);
        }
        #endregion

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            NasClient.instance?.TryHalt();
        }

        private void m_OnLoginSuccess()
        {
            void _Show()
            {
                // TODO: 예외 발생하는지 점검해보기
                new FileBrowserForm().Show();
                this.Hide();
            }

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

            ctChangeFormMode(AuthForm.FormMode.Login);

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
    }
}
