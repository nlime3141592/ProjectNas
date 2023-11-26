using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS.Client
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

            m_columnWidth0 = 480;
            m_columnWidth1 = 120;
            m_columnWidth2 = 120;
            lvFileBrowser.Columns[0].Width = m_columnWidth0;
            lvFileBrowser.Columns[1].Width = m_columnWidth1;
            lvFileBrowser.Columns[2].Width = m_columnWidth2;

            m_selectedItemIndex = new List<int>();

            lvFileBrowser.Items.Clear();

            for(int i = 0; i < 50; ++i)
            {
                string n0 = string.Format("{0:D05}", i);
                string n1 = string.Format("{0:D05}", i + 100);
                string n2 = string.Format("{0:D05}", i + 10000);

                lvFileBrowser.Items.Add(new ListViewItem(new string[] { n0, n1, n2 }));
            }
        }

        private void lvFileBrowser_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                m_selectedItemIndex.Add(e.ItemIndex);
            else
                m_selectedItemIndex.Remove(e.ItemIndex);

            if (m_selectedItemIndex.Count == 0)
                lvFileBrowser.ContextMenuStrip = null;
            else if (m_selectedItemIndex[0] > 3) // TODO: 폴더를 우선 보여주므로, 폴더 개수 정보가 조건문으로 들어와야 한다.
                lvFileBrowser.ContextMenuStrip = ctxMenuFileSelected;
            else
                lvFileBrowser.ContextMenuStrip = ctxMenuFolderSelected;
        }
    }
}
