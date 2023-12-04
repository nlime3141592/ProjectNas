using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS.FileSystem
{
    internal static class FileSystemMain
    {
        private static FileSys s_m_fileSystem;

        public static FileSys GetFileSystem()
        {
            return s_m_fileSystem;
        }

        static FileSystemMain()
        {
            s_m_fileSystem = new FileSys(@"C:\NAS");
        }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static int Main(string[] _args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(FileBrowserForm.GetInstance());
            return 0;
        }
    }
}
