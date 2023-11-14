using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS
{
    public partial class FileBrowserForm : Form
    {
        private static FileBrowserForm s_m_form;

        private FileBrowserForm()
        {
            InitializeComponent();
            ShowBrowser();
        }

        public static FileBrowserForm GetInstance()
        {
            if (s_m_form == null)
                s_m_form = new FileBrowserForm();

            return s_m_form;
        }

        // NOTE: 파일 시스템의 속성을 정리해 화면에 보여줍니다.
        private void ShowBrowser()
        {
            FileSystem fs = FileSystemMain.GetFileSystem();

            itxt_directory.Text = fs.GetCurrentAbsoluteDirectory();

            list_files.Items.Clear();

            if (fs.NextDirectories.Length == 0 && fs.NextFiles.Length == 0)
            {
                list_files.Items.Add("이 폴더는 비어 있습니다.");
                return;
            }

            foreach (string relativeDirectory in fs.NextDirectories)
                list_files.Items.Add(@"\" + relativeDirectory);
            foreach (string relativeFile in fs.NextFiles)
                list_files.Items.Add(@"\" + relativeFile);
        }

        private void SelectItem()
        {
            FileSystem fs = FileSystemMain.GetFileSystem();

            if (fs.isEmptyDirectory)
                return;

            int index = list_files.SelectedIndex;

            if (index >= 0 && index < fs.NextDirectories.Length)
            {
                fs.MoveNext(index);
                ShowBrowser();
            }
            else
            {
                // TODO: 파일을 선택하였으므로, 파일 다운로드 로직이 이 곳에 포함되어야 합니다.
                string absdir = fs.GetCurrentAbsoluteDirectory() + list_files.SelectedItem;
                MessageBox.Show(string.Format("파일을 선택했습니다. ({0})", absdir));
                return;
            }
        }

        private void MoveBefore()
        {
            if (FileSystemMain.GetFileSystem().isRoot)
                return;

            FileSystemMain.GetFileSystem().MoveBefore();
            ShowBrowser();
        }

        private void MoveRoot()
        {
            if (FileSystemMain.GetFileSystem().isRoot)
                return;

            FileSystemMain.GetFileSystem().MoveRoot();
            ShowBrowser();
        }

        // NOTE: 구성 요소를 더블 클릭할 때 발생합니다.
        private void list_files_DoubleClick(object sender, EventArgs e)
        {
            SelectItem();
        }

        private void list_files_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.R:
                    MoveRoot();
                    break;
                case Keys.Escape:
                    MoveBefore();
                    break;
                case Keys.Enter:
                    SelectItem();
                    break;
                default:
                    return;
            }
        }

        // NOTE: 바로 위 디렉토리로 이동하는 버튼을 누를 때 발생합니다.
        private void bt_moveBack_Click(object sender, EventArgs e)
        {
            MoveBefore();
        }

        private void bt_moveRoot_Click(object sender, EventArgs e)
        {
            MoveRoot();
        }

        private void FileBrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(s_m_form == this)
                s_m_form = null;
        }
    }
}
