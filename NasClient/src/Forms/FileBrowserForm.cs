using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NAS
{
    public sealed partial class FileBrowserForm : Form
    {
        private static FileBrowserForm s_m_fileBrowserForm;

        private int m_columnWidth0;
        private int m_columnWidth1;
        private int m_columnWidth2;
        private List<int> m_selectedItemIndex;

        public static FileBrowserForm GetForm()
        {
            if(s_m_fileBrowserForm == null)
                s_m_fileBrowserForm = new FileBrowserForm();

            return s_m_fileBrowserForm;
        }

        private FileBrowserForm()
        {
            InitializeComponent();

            m_columnWidth0 = 660;
            m_columnWidth1 = 120;
            m_columnWidth2 = 120;
            lvFileBrowser.Columns[0].Width = m_columnWidth0;
            lvFileBrowser.Columns[1].Width = m_columnWidth1;
            lvFileBrowser.Columns[2].Width = m_columnWidth2;

            m_selectedItemIndex = new List<int>();

            lvFileBrowser.Items.Clear();

            m_ReloadDir();
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

        public void ctClose()
        {
            void _Close()
            {
                this.Close();
            }

            if(this.InvokeRequired)
                this.Invoke(new Action(_Close));
            else
                _Close();
        }

        private void lvFileBrowser_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                m_selectedItemIndex.Add(e.ItemIndex);
            else
                m_selectedItemIndex.Remove(e.ItemIndex);

            if (m_selectedItemIndex.Count == 0)
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            else if (m_selectedItemIndex[0] > NasClientProgram.GetClient().datFileBrowse.directories.Count) // TODO: 폴더를 우선 보여주므로, 폴더 개수 정보가 조건문으로 들어와야 한다.
                lvFileBrowser.ContextMenuStrip = ctxMenuFileSelected;
            else
                lvFileBrowser.ContextMenuStrip = ctxMenuFolderSelected;
        }

        private void FileBrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (s_m_fileBrowserForm == this)
                s_m_fileBrowserForm = null;

            AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Login);
            AuthForm.GetForm().ctShow();
        }

        private void lvFileBrowser_DoubleClick(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;

            string filetype = view.SelectedItems[0].SubItems[1].Text;

            switch(filetype)
            {
                case "Folder":
                    string dirnext = view.SelectedItems[0].SubItems[0].Text;
                    m_ToNextDir(dirnext);
                    break;
                case "File":
                    break;
                default:
                    break;
            }
        }

        public void ctUpdateFileBrowser()
        {
            string name = "";
            string type = "";
            string size = "";
            NasClient client = NasClientProgram.GetClient();

            void _Init()
            {
                txtFakePath.Text = client.datFileBrowse.fakedir;
                lvFileBrowser.Items.Clear();
            }

            void _Add()
            {
                lvFileBrowser.Items.Add(new ListViewItem(new string[] { name, type, size }));
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Init));
            else
                _Init();

            foreach (string dname in client.datFileBrowse.directories.Values)
            {
                name = dname;
                type = "Folder";
                size = "";

                if (this.InvokeRequired)
                    this.Invoke(new Action(_Add));
                else
                    _Add();
            }

            foreach (string fname in client.datFileBrowse.files.Values)
            {
                name = fname;
                type = "File";
                size = "0";

                if (this.InvokeRequired)
                    this.Invoke(new Action(_Add));
                else
                    _Add();
            }
        }

        private void m_OnDirectoryMoveSuccess()
        {
            ctUpdateFileBrowser();
        }

        private void m_OnFileAddSuccess(int _fidx, string _fileName)
        {
            void _Show()
            {
                NasClient client = NasClientProgram.GetClient();
                client.datFileBrowse.files.TryAdd(_fidx, _fileName);
                ctUpdateFileBrowser();
                MessageBox.Show(this, "새로운 파일을 업로드 했습니다.", "파일 업로드");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnFileAddFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "파일을 추가할 수 없습니다.", "파일 업로드 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidFileName()
        {
            void _Show()
            {
                MessageBox.Show(this, "파일 이름이 잘못되었습니다..", "파일 업로드 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnExistFile()
        {
            void _Show()
            {
                MessageBox.Show(this, "이미 존재하는 파일 이름입니다.", "파일 업로드 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnNetworkError()
        {
            NasClientProgram.GetClient().TryHalt();
            this.ctClose();
            AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Start);
            AuthForm.GetForm().ctShow();
        }

        private void btRoot_Click(object sender, EventArgs e)
        {
            NasClient client = NasClientProgram.GetClient();

            if (client.datFileBrowse.fakeroot.Equals(client.datFileBrowse.fakedir))
                return;

            m_ToRootDir();
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            NasClient client = NasClientProgram.GetClient();

            if (client.datFileBrowse.fakeroot.Equals(client.datFileBrowse.fakedir))
                return;

            m_ToBackDir();
        }

        private void m_ToRootDir()
        {
            NasClient client = NasClientProgram.GetClient();
            client.datFileBrowse.fakedir = client.datFileBrowse.fakeroot;
            CSvDirectoryMove service = new CSvDirectoryMove(client, "");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            service.onError = m_OnNetworkError;
            NasClientProgram.GetClient().Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        private void m_ReloadDir()
        {
            CSvDirectoryMove service = new CSvDirectoryMove(NasClientProgram.GetClient(), ".");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            service.onError = m_OnNetworkError;
            NasClientProgram.GetClient().Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        private void m_ToNextDir(string _nextdir)
        {
            CSvDirectoryMove service = new CSvDirectoryMove(NasClientProgram.GetClient(), _nextdir);
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            service.onError = m_OnNetworkError;
            NasClientProgram.GetClient().Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        private void m_ToBackDir()
        {
            CSvDirectoryMove service = new CSvDirectoryMove(NasClientProgram.GetClient(), "..");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            service.onError = m_OnNetworkError;
            NasClientProgram.GetClient().Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        private bool m_TrySelectFile(out string _absPath)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "폴더 추가";
            dialog.Multiselect = false;

            DialogResult result = dialog.ShowDialog(this);

            switch(result)
            {
                case DialogResult.Cancel:
                    _absPath = null;
                    return false;
                case DialogResult.OK:
                    _absPath = dialog.FileName; // NOTE: 절대경로
                    return true;
                default:
                    _absPath = null;
                    return false;
            }
        }

        private void 이동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dirnext = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
            m_ToNextDir(dirnext);
        }

        private void 루트경로로이동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_ToRootDir();
        }

        private void 상위경로로이동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_ToBackDir();
        }

        private void 새폴더ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteFolderNameForm form = new WriteFolderNameForm();
            form.ShowDialog(this);
        }

        private void 새폴더ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WriteFolderNameForm form = new WriteFolderNameForm();
            form.ShowDialog(this);
        }

        private void 파일업로드ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string absPath;

            if (!m_TrySelectFile(out absPath))
                return;

            WriteFileNameForm form = new WriteFileNameForm(absPath);
            form.onFileAddSuccess = m_OnFileAddSuccess;
            form.onFileAddFailure = m_OnFileAddFailure;
            form.onInvalidName = m_OnInvalidFileName;
            form.onExistFile = m_OnExistFile;
            form.onError = m_OnNetworkError;
            form.ShowDialog(this);
        }

        private void 파일업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string absPath;

            if (!m_TrySelectFile(out absPath))
                return;

            WriteFileNameForm form = new WriteFileNameForm(absPath);
            form.onFileAddSuccess = m_OnFileAddSuccess;
            form.onFileAddFailure = m_OnFileAddFailure;
            form.onInvalidName = m_OnInvalidFileName;
            form.onExistFile = m_OnExistFile;
            form.onError = m_OnNetworkError;
            form.ShowDialog(this);
        }

        // NOTE: 폴더 삭제 이벤트
        private void 삭제ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "폴더를 삭제하시겠습니까?\n폴더 안에 포함된 모든 내용이 함께 삭제됩니다.", "폴더 삭제", MessageBoxButtons.OKCancel);

            switch(result)
            {
                case DialogResult.OK:
                    string folderName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
                    CSvDirectoryDelete service = new CSvDirectoryDelete(NasClientProgram.GetClient(), folderName);
                    service.onDeleteSuccess = m_OnDirectoryDeleteSuccess;
                    service.onDeleteFailure = m_OnDirectoryDeleteFailure;
                    NasClientProgram.GetClient().Request(service);
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        private void m_OnDirectoryDeleteSuccess(int _didx, string _folderName)
        {
            string folderName;

            NasClient client = NasClientProgram.GetClient();
            client.datFileBrowse.directories.TryRemove(_didx, out folderName);

            ctUpdateFileBrowser();
        }

        private void m_OnDirectoryDeleteFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "폴더를 삭제할 수 없습니다.", "폴더 삭제 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }
    }
}
