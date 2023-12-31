﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NAS
{
    // NOTE: 파일 탐색기를 WinForm으로 구현합니다.
    public sealed partial class FileBrowserForm : Form
    {
        // NOTE: 파일 다운로드 시 다운로드 현황 리스트 뷰 갱신 속도를 제어하는 스탑워치 클래스입니다.
        private Stopwatch m_watch;

        public FileBrowserForm()
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

        // NOTE: 파일 목록에서의 Context Menu 전환을 위한 이벤트 함수.
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

        // NOTE: 파일 목록에서의 Context Menu 전환을 위한 이벤트 함수.
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
            // NOTE: 이 Form이 닫혔을 때 초기 화면으로 이동합니다.
            NasClient.instance.Close();
            AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Start);
            AuthForm.GetForm().ctShow();
        }

        // NOTE: 폴더나 파일을 더블클릭 할 때 발생하는 이벤트
        private void lvFileBrowser_DoubleClick(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;

            if (view.SelectedItems.Count == 0)
                return;

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

                    NasClient client = NasClient.instance;
                    string fileName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
                    CSvFileDownload service = new CSvFileDownload(client, absSaveDirectory, fileName);
                    service.onDownloadSuccess = m_OnFileDownloadSuccess;
                    service.onDownloadLoopback = m_OnFileDownloadLoopback;
                    client.Request(service);
                    break;
                default:
                    break;
            }
        }

        // NOTE: 파일 탐색기의 폴더 및 파일 목록을 FileBrowseData 클래스가 포함한 내용으로 갱신합니다.
        public void ctUpdateFileBrowser()
        {
            string name = "";
            string type = "";
            string size = "";
            NasClient client = NasClient.instance;

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

            // NOTE: 폴더 목록 갱신.
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

            // NOTE: 파일 목록 갱신.
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

        // NOTE: 다운로드 상황을 실시간으로 갱신합니다.
        public void ctUpdateFileDownloadingMonitor()
        {
            void _Show()
            {
                NasClient client = NasClient.instance;
                int count = client.datFileBrowse.downloadingFiles.Count;

                if (!m_watch.IsRunning)
                    m_watch.Start();

                if (count <= 0)
                {
                    lbDownloadingCounter.Text = string.Format("다운로드 없음");
                    lvFileDownloadingBrowser.Items.Clear();
                }
                // NOTE:
                // 리스트 뷰 갱신 과부하를 막기 위해 100ms마다 갱신합니다.
                // 실제 파일 다운로드 속도에 영향을 주지는 않습니다.
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

        private void btRoot_Click(object sender, EventArgs e)
        {
            m_ToRootDir();
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            m_ToBackDir();
        }

        // NOTE: 루트 경로로 이동합니다.
        private void m_ToRootDir()
        {
            NasClient client = NasClient.instance;

            // NOTE: 최상위 폴더에 위치한다면 이동하지 않습니다.
            if (client.datFileBrowse.fakeroot.Equals(client.datFileBrowse.fakedir))
                return;

            client.datFileBrowse.fakedir = client.datFileBrowse.fakeroot;
            CSvDirectoryMove service = new CSvDirectoryMove(client, "");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            NasClient.instance.Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        // NOTE: 현재 위치한 폴더의 목록을 갱신합니다.
        private void m_ReloadDir()
        {
            CSvDirectoryMove service = new CSvDirectoryMove(NasClient.instance, ".");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            NasClient.instance.Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        // NOTE: 하위 폴더로 이동합니다.
        private void m_ToNextDir(string _nextdir)
        {
            CSvDirectoryMove service = new CSvDirectoryMove(NasClient.instance, _nextdir);
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            NasClient.instance.Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        // NOTE: 상위 폴더로 이동합니다.
        private void m_ToBackDir()
        {
            NasClient client = NasClient.instance;

            // NOTE: 최상위 폴더에 위치한다면 이동하지 않습니다.
            if (client.datFileBrowse.fakeroot.Equals(client.datFileBrowse.fakedir))
                return;

            CSvDirectoryMove service = new CSvDirectoryMove(client, "..");
            service.onMoveSuccess = m_OnDirectoryMoveSuccess;
            NasClient.instance.Request(service);

            void _Set()
            {
                lvFileBrowser.ContextMenuStrip = ctxMenuNoSelected;
            }

            if (lvFileBrowser.InvokeRequired)
                lvFileBrowser.Invoke(new Action(_Set));
            else
                _Set();
        }

        // NOTE: 파일 업로드를 위해 로컬 파일 시스템에서 파일을 선택합니다.
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

        // NOTE: 파일 다운로드를 위해 로컬 파일 시스템에서 다운로드한 파일을 저장할 폴더를 선택합니다.
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

        // NOTE: 메뉴 및 컨텍스트 메뉴의 이벤트 핸들러 함수입니다.
        #region Menu/Context Menu Events
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
            form.onFolderAddSuccess = m_OnDirectoryAddSuccess;
            form.onFolderAddFailure = m_OnDirectoryAddFailure;
            form.onInvalidName = m_OnInvalidDirectoryName;
            form.ShowDialog(this);
        }

        private void 새폴더ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WriteFolderNameForm form = new WriteFolderNameForm();
            form.onFolderAddSuccess = m_OnDirectoryAddSuccess;
            form.onFolderAddFailure = m_OnDirectoryAddFailure;
            form.onInvalidName = m_OnInvalidDirectoryName;
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
            form.ShowDialog(this);
        }

        private void 다운로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string absSaveDirectory;

            if (!m_TrySelectDirectory(out absSaveDirectory))
                return;

            NasClient client = NasClient.instance;
            string fileName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
            CSvFileDownload service = new CSvFileDownload(client, absSaveDirectory, fileName);
            service.onDownloadSuccess = m_OnFileDownloadSuccess;
            service.onDownloadLoopback = m_OnFileDownloadLoopback;
            client.Request(service);
        }

        private void 삭제ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "폴더를 삭제하시겠습니까?\n폴더 안에 포함된 모든 내용이 함께 삭제됩니다.", "폴더 삭제", MessageBoxButtons.OKCancel);

            switch(result)
            {
                case DialogResult.OK:
                    string folderName = lvFileBrowser.SelectedItems[0].SubItems[0].Text;
                    CSvDirectoryDelete service = new CSvDirectoryDelete(NasClient.instance, folderName);
                    service.onDeleteSuccess = m_OnDirectoryDeleteSuccess;
                    service.onDeleteFailure = m_OnDirectoryDeleteFailure;
                    NasClient.instance.Request(service);
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
                    CSvFileDelete service = new CSvFileDelete(NasClient.instance, fileName);
                    service.onDeleteSuccess = m_OnFileDeleteSuccess;
                    service.onDeleteFailure = m_OnFileDeleteFailure;
                    NasClient.instance.Request(service);
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }
        #endregion

        // NOTE: 폴더 및 파일 추가/삭제, 파일 다운로드 서비스 객체의 이벤트 핸들러 객체를 포함하는 영역입니다.
        #region File System Service Operation Events
        private void m_OnDirectoryAddSuccess(int _didx, string _folderName)
        {
            void _Show()
            {
                NasClient.instance.datFileBrowse.directories.TryAdd(_didx, _folderName);
                ctUpdateFileBrowser();
                MessageBox.Show(this, "폴더를 추가했습니다.", "폴더 추가 완료");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnDirectoryAddFailure()
        {
            void _Show()
            {
                MessageBox.Show(this, "이미 존재하는 폴더입니다.", "폴더 추가 실패");
            }

            if (this.InvokeRequired)
                this.Invoke(new Action(_Show));
            else
                _Show();
        }

        private void m_OnInvalidDirectoryName()
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

        private void m_OnDirectoryDeleteSuccess(int _didx, string _folderName)
        {
            string folderName;

            NasClient client = NasClient.instance;
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

            NasClient client = NasClient.instance;
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
            NasClient client = NasClient.instance;

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
                NasClient.instance.datFileBrowse.downloadingFiles.TryRemove(_absDownloadedPath, out loopTimes);
                ctUpdateFileDownloadingMonitor();
                string message = string.Format("파일을 다운로드 했습니다.\n({0})", _absDownloadedPath);
                MessageBox.Show(this, message, "파일 다운로드 성공");
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
                NasClient client = NasClient.instance;
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
        #endregion
    }
}
