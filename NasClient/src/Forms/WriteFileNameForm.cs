using System;
using System.IO;
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

        public WriteFileNameForm(string _absPath)
        {
            InitializeComponent();

            m_absPath = _absPath;

            int level = NasClient.instance.datLogin.level;
            cbxPermissionLevel.Items.Clear();
            for (int i = 1; i <= level; ++i)
                cbxPermissionLevel.Items.Add(i);
            cbxPermissionLevel.SelectedIndex = 0;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            string extension = Path.GetExtension(m_absPath);
            int department = rbtAll.Checked ? 0 : NasClient.instance.datLogin.department;
            int level = department == 0 ? 0 : int.Parse(cbxPermissionLevel.Text);

            CSvFileAdd service = new CSvFileAdd(NasClient.instance, m_absPath, txtFileName.Text, extension, department, level);
            service.onAddSuccess = onFileAddSuccess;
            service.onAddSuccess += m_OnAddSuccess;
            service.onAddFailure = onFileAddFailure;
            service.onInvalidName = this.onInvalidName;
            service.onExistFile = this.onExistFile;
            NasClient.instance.Request(service);
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
