using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NAS
{
    public sealed partial class FileBrowserForm : Form
    {
        private static FileBrowserForm s_m_fileBrowserForm;
        private Stopwatch m_watch;

        public static FileBrowserForm GetForm()
        {
            if(s_m_fileBrowserForm == null)
                s_m_fileBrowserForm = new FileBrowserForm();

            return s_m_fileBrowserForm;
        }

        private FileBrowserForm()
        {
            InitializeComponent();

            m_watch = new Stopwatch();

            lvFileBrowser.Columns[0].Width = 660;
            lvFileBrowser.Columns[1].Width = 120;
            lvFileBrowser.Columns[2].Width = 120;

            lvFileDownloadingBrowser.Columns[0].Width = 780;
            lvFileDownloadingBrowser.Columns[1].Width = 120;

            lvFileBrowser.Items.Clear();

            m_ReloadDir();
            ctUpdateFileDownloadingMonitor();
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
            if (lvFileBrowser.SelectedIndices.Count == 0)
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
                return;
            }

            string fileType = lvFileBrowser.SelectedItems[0].SubItems[1].Text;

            switch (fileType)
            {
                case "Folder":
                    lvFileBrowser.ContextMenuStrip = ctxMenuFolderSelected;
                    break;
                case "File":
                    lvFileBrowser.ContextMenuStrip = ctxMenuFileSelected;
                    break;
                default:
                    break;
            }
        }

        private void lvFileBrowser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFileBrowser.SelectedIndices.Count == 0)
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
                return;
            }

            string fileType = lvFileBrowser.SelectedItems[0].SubItems[1].Text;

            switch(fileType)
            {
                case "Folder":
                    lvFileBrowser.ContextMenuStrip = ctxMenuFolderSelected;
                    break;
                case "File":
                    lvFileBrowser.ContextMenuStrip = ctxMenuFileSelected;
                    break;
                default:
                    break;
            }
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
                    string absSaveDirectory;

                    if (!m_TrySelectDirectory(out absSaveDirectory))
                        return;

                    NasClient client = NasClientProgram.GetClient();
                    string fileName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
                    CSvFileDownload service = new CSvFileDownload(client, absSaveDirectory, fileName);
                    service.onDownloadSuccess = m_OnFileDownloadSuccess;
                    service.onDownloadLoopback = m_OnFileDownloadLoopback;
                    service.onError = m_OnNetworkError;
                    client.Request(service);
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

            void _Show()
            {
                lbFileCounter.Text = string.Format("폴더: {0}개, 파일: {1}개", client.datFileBrowse.directories.Count, client.datFileBrowse.files.Count);
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
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

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        public void ctUpdateFileDownloadingMonitor()
        {
            void _Show()
            {
                NasClient client = NasClientProgram.GetClient();
                int count = client.datFileBrowse.downloadingFiles.Count;

                if (!m_watch.IsRunning)
                    m_watch.Start();

                if (count <= 0)
                {
                    lbDownloadingCounter.Text = string.Format("다운로드 없음");
                    lvFileDownloadingBrowser.Items.Clear();
                }
                else if (m_watch.ElapsedMilliseconds < 100)
                    return;
                else
                {
                    m_watch.Restart();

                    lbDownloadingCounter.Text = string.Format("다운로드 진행 중 ... ({0}개)", count);

                    for(int i = lvFileDownloadingBrowser.Items.Count - 1; i >= 0 ; --i)
                    {
                        string path = lvFileDownloadingBrowser.Items[i].SubItems[0].Text;

                        if (client.datFileBrowse.downloadingFiles.ContainsKey(path))
                        {
                            lvFileDownloadingBrowser.Items[i].SubItems[1].Text = client.datFileBrowse.downloadingFiles[path].ToString();
                        }
                        else
                        {
                            lvFileDownloadingBrowser.Items.RemoveAt(i);
                        }
                    }

                    foreach (string key in client.datFileBrowse.downloadingFiles.Keys)
                    {
                        bool isContainInListView = false;

                        for (int i = lvFileDownloadingBrowser.Items.Count - 1; i >= 0 && !isContainInListView; --i)
                            isContainInListView = lvFileDownloadingBrowser.Items[i].SubItems[0].Text.Equals(key);

                        if(!isContainInListView)
                            lvFileDownloadingBrowser.Items.Add(new ListViewItem(new string[] { key, client.datFileBrowse.downloadingFiles[key].ToString() }));
                    }
                }
            }

            if(this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
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
                MessageBox.Show(this, "파일 이름이 잘못되었습니다.", "파일 업로드 실패");
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

        private bool m_TrySelectDirectory(out string _absSaveDirectory)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            DialogResult result = dialog.ShowDialog(this);

            switch (result)
            {
                case DialogResult.Cancel:
                    _absSaveDirectory = null;
                    return false;
                case DialogResult.OK:
                    _absSaveDirectory = dialog.SelectedPath + '\\'; // NOTE: 디렉토리
                    this.WriteLog("디렉토리 : {0}", _absSaveDirectory);
                    return true;
                default:
                    _absSaveDirectory = null;
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

        // NOTE: 파일 다운로드 이벤트
        private void 다운로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string absSaveDirectory;

            if (!m_TrySelectDirectory(out absSaveDirectory))
                return;

            NasClient client = NasClientProgram.GetClient();
            string fileName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
            CSvFileDownload service = new CSvFileDownload(client, absSaveDirectory, fileName);
            service.onDownloadSuccess = m_OnFileDownloadSuccess;
            service.onDownloadLoopback = m_OnFileDownloadLoopback;
            service.onError = m_OnNetworkError;
            client.Request(service);
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

        private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "파일을 삭제하시겠습니까?", "파일 삭제", MessageBoxButtons.OKCancel);

            switch (result)
            {
                case DialogResult.OK:
                    string fileName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
                    CSvFileDelete service = new CSvFileDelete(NasClientProgram.GetClient(), fileName);
                    service.onDeleteSuccess = m_OnFileDeleteSuccess;
                    service.onDeleteFailure = m_OnFileDeleteFailure;
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

        private void m_OnFileDeleteSuccess(int _fidx, string _folderName)
        {
            string fileName;

            NasClient client = NasClientProgram.GetClient();
            client.datFileBrowse.files.TryRemove(_fidx, out fileName);

            ctUpdateFileBrowser();
        }

        private void m_OnFileDeleteFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "파일을 삭제할 수 없습니다.", "파일 삭제 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnFileDownloadLoopback(string _absDownloadingPath, int _loopTimes)
        {
            NasClient client = NasClientProgram.GetClient();

            if (client.datFileBrowse.downloadingFiles.ContainsKey(_absDownloadingPath))
                client.datFileBrowse.downloadingFiles[_absDownloadingPath] = _loopTimes;
            else
                client.datFileBrowse.downloadingFiles.TryAdd(_absDownloadingPath, _loopTimes);

            ctUpdateFileDownloadingMonitor();
        }

        private void m_OnFileDownloadSuccess(string _absDownloadedPath)
        {
            void _Show()
            {
                int loopTimes;
                NasClientProgram.GetClient().datFileBrowse.downloadingFiles.TryRemove(_absDownloadedPath, out loopTimes);
                ctUpdateFileDownloadingMonitor();
                string message = string.Format("파일을 다운로드 했습니다.\n({0})", _absDownloadedPath);
                MessageBox.Show(this, message, "파일 다운로드 성공");
            }

            if(this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnFileDownloadFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "파일을 다운로드 하지 못했습니다.", "파일 다운로드 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }
    }
}
