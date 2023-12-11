using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS
{
    public partial class AcceptorForm : Form
    {
        public AcceptorForm()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                m_OnTriedConnectionToServer(NasAcceptorProgram.TryConnectToServer());
            });
        }

        public void RequestAcceptorViewUpdateService()
        {
            NasAcceptor acceptor = NasAcceptorProgram.GetAcceptor();
            ASvAcceptorViewUpdate service = new ASvAcceptorViewUpdate(acceptor);
            service.onSuccess += ctUpdateWaitingAccounts;
            service.onError += m_OnNetworkError;
            acceptor.Request(service);
        }

        public void ctUpdateWaitingAccounts()
        {
            void _Update()
            {
                txtUuid.Text = string.Empty;
                lvAccounts.Items.Clear();
                cbxDepartment.Items.Clear();

                lvAccounts.Columns[0].Width = 120;
                lvAccounts.Columns[1].Width = 180;
                lvAccounts.Columns[2].Width = 180;
                lvAccounts.Columns[3].Width = 180;

                foreach (WaitingAccountData wdat in NasAcceptorProgram.GetAcceptor().wAccounts)
                {
                    string uuid = wdat.uuid.ToString();
                    string name = wdat.name;
                    string id = wdat.id;
                    string regdate = wdat.regdate;
                    lvAccounts.Items.Add(new ListViewItem(new string[] { uuid, name, id, regdate }));
                }

                foreach (DepartmentData ddat in NasAcceptorProgram.GetAcceptor().departments)
                    cbxDepartment.Items.Add(ddat.departmentName);
                cbxDepartment.SelectedIndex = 0;

                cbxLevel.Items.Clear();
                for (int i = 1; i <= 4; ++i)
                    cbxLevel.Items.Add(i);
                cbxLevel.SelectedIndex = 0;
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Update));
            else
                _Update();
        }

        private void m_OnTriedConnectionToServer(bool _isSuccess)
        {
            void _Show()
            {
                if (_isSuccess)
                {
                    RequestAcceptorViewUpdateService();
                }
                else
                {
                    MessageBox.Show(this, "서버 연결에 실패했습니다.", "NAS Acceptor Form", MessageBoxButtons.OK);
                    NasAcceptorProgram.GetAcceptor().TryHalt();
                    this.Close();
                }
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void AcceptorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            NasAcceptorProgram.GetAcceptor().TryHalt();
        }

        private void m_OnNetworkError()
        {
            void _Show()
            {
                MessageBox.Show(this, "서버와의 연결이 끊어졌습니다.", "NAS Acceptor Form", MessageBoxButtons.OK);
                NasAcceptorProgram.GetAcceptor().TryHalt();
                this.Close();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnAccountDenySuccess()
        {
            void _Show()
            {
                MessageBox.Show(this, "가입 거절했습니다.", "가입 거절");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnAccountDenyFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "가입 거절 과정에 알 수 없는 오류가 발생했습니다.", "가입 거절");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnAccountAcceptSuccess()
        {
            void _Show()
            {
                MessageBox.Show(this, "가입 승인 완료.", "가입 승인");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnAccountAcceptFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "가입 승인 과정에 알 수 없는 오류가 발생했습니다.", "가입 승인");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidUuid()
        {
            void _Show()
            {
                MessageBox.Show(this, "잘못된 사용자 고유 번호입니다.", "Acceptor Form");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidLevel()
        {
            void _Show()
            {
                MessageBox.Show(this, "잘못된 권한 레벨 값입니다.", "Acceptor Form");
                RequestAcceptorViewUpdateService();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        // NOTE: 가입 거절 버튼 누를 때
        private void btDeny_Click(object sender, EventArgs e)
        {
            ASvJoinDeny service = new ASvJoinDeny(NasAcceptorProgram.GetAcceptor(), txtUuid.Text);
            service.onDenySuccess = m_OnAccountDenySuccess;
            service.onInvalidUuid = m_OnInvalidUuid;
            service.onDenyFailure = m_OnAccountDenyFailure;
            service.onError = m_OnNetworkError;
            NasAcceptorProgram.GetAcceptor().Request(service);
        }

        // NOTE: 가입 승인 버튼 누를 때
        private void btAccept_Click(object sender, EventArgs e)
        {
            ASvJoinAccept service = new ASvJoinAccept(NasAcceptorProgram.GetAcceptor(), txtUuid.Text, cbxDepartment.SelectedItem.ToString(), cbxLevel.SelectedItem.ToString());
            service.onAcceptSuccess = m_OnAccountAcceptSuccess;
            service.onInvalidUuid = m_OnInvalidUuid;
            service.onInvalidLevel = m_OnInvalidLevel;
            service.onAcceptFailure = m_OnAccountAcceptFailure;
            service.onError = m_OnNetworkError;
            NasAcceptorProgram.GetAcceptor().Request(service);
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RequestAcceptorViewUpdateService();
        }
    }
}
