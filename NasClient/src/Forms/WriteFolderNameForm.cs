using System;
using System.Windows.Forms;

namespace NAS
{
    public partial class WriteFolderNameForm : Form
    {
        public Action<int, string> onFileAddSuccess;
        public Action onFileAddFailure;
        public Action onInvalidName;
        public Action onExistFolder;

        public WriteFolderNameForm()
        {
            InitializeComponent();

            int level = NasClient.instance.datLogin.level;
            cbxPermissionLevel.Items.Clear();
            for (int i = 1; i <= level; ++i)
                cbxPermissionLevel.Items.Add(i);
            cbxPermissionLevel.SelectedIndex = 0;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            int department = rbtAll.Checked ? 0 : NasClient.instance.datLogin.department;
            int level = department == 0 ? 0 : int.Parse(cbxPermissionLevel.Text);

            CSvDirectoryAdd service = new CSvDirectoryAdd(NasClient.instance, txtFolderName.Text, department, level);
            service.onAddSuccess += onFileAddSuccess;
            service.onAddFailure = onFileAddFailure;
            service.onInvalidName = onInvalidName;
            NasClient.instance.Request(service);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_OnSuccess(int _uuid, string _folderName)
        {
            void _Show()
            {
                NasClient.instance.datFileBrowse.directories.TryAdd(_uuid, _folderName);
                // FileBrowserForm.GetForm().ctUpdateFileBrowser();
                MessageBox.Show(this, "폴더를 추가했습니다.", "폴더 추가 완료");
                this.Close();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "이미 존재하는 파일입니다.", "폴더 추가 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidName()
        {
            void _Show()
            {
                MessageBox.Show(this, "잘못된 폴더 이름 형식입니다.", "폴더 이름 입력");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }
    }
}
