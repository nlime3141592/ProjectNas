using System;
using System.Windows.Forms;

namespace NAS
{
    public partial class WriteFolderNameForm : Form
    {
        public WriteFolderNameForm()
        {
            InitializeComponent();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            CSvDirectoryAdd service = new CSvDirectoryAdd(NasClientProgram.GetClient(), txtFolderName.Text);
            service.onAddSuccess = m_OnSuccess;
            service.onAddFailure = m_OnFailure;
            service.onInvalidName = m_OnInvalidName;
            service.onError = m_OnNetworkError;
            NasClientProgram.GetClient().Request(service);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_OnSuccess(int _uuid, string _folderName)
        {
            void _Show()
            {
                NasClientProgram.GetClient().datFileBrowse.directories.TryAdd(_uuid, _folderName);
                FileBrowserForm.GetForm().ctUpdateFileBrowser();
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

        private void m_OnNetworkError()
        {
            void _Show()
            {
                NasClientProgram.GetClient().TryHalt();
                this.Close();
                FileBrowserForm.GetForm().ctClose();
                AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Start);
                AuthForm.GetForm().ctShow();
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }
    }
}
