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

        private void m_OnTriedConnectionToServer(bool _isSuccess)
        {
            void _Show()
            {
                if (!_isSuccess)
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
    }
}
