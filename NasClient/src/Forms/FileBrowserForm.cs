using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private void m_OnDirectoryMoveSuccess()
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

        private void m_OnNetworkError()
        {
            NasClientProgram.GetClient().TryHalt();
            this.ctClose();
            AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Start);
            AuthForm.GetForm().ctShow();
        }

        private void btRoot_Click(object sender, EventArgs e)
        {
            m_ToRootDir();
        }

        private void btBack_Click(object sender, EventArgs e)
        {
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
    }
}
