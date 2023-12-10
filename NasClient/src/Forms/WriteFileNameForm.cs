using System;
using System.Windows.Forms;

namespace NAS
{
    public partial class WriteFileNameForm : Form
    {
        private string m_absPath;

        public Action<int, string> onFileAddSuccess;
        public Action onFileAddFailure;
        public Action onInvalidName;
        public Action onExistFile;
        public Action onError;

        public WriteFileNameForm(string _absPath)
        {
            InitializeComponent();

            m_absPath = _absPath;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            CSvFileAdd service = new CSvFileAdd(NasClientProgram.GetClient(), m_absPath, txtFileName.Text);
            service.onAddSuccess = onFileAddSuccess;
            service.onAddSuccess += m_OnAddSuccess;
            service.onAddFailure = onFileAddFailure;
            service.onInvalidName = this.onInvalidName;
            service.onExistFile = this.onExistFile;
            service.onError = this.onError;
            NasClientProgram.GetClient().Request(service);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_OnAddSuccess(int _fidx, string _fileName)
        {
            void _Close()
            {
                this.Close();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Close));
            else
                _Close();
        }
    }
}
