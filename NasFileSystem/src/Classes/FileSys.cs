using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public class FileSys
    {
        private const string c_PERMISSION_FILE = @"\.permission";

        public bool isRoot => m_dirStack.Count == 0;
        public bool isEmptyDirectory => m_dirNextDirectories.Length == 0 && m_dirNextFiles.Length == 0;

        private StringBuilder m_pathBuilder;
        private List<string> m_dirStack;
        private string m_dirRoot;
        private string[] m_dirNextDirectories;
        private string[] m_dirNextFiles;

        public FileSys(string _dirRoot)
        {
            m_pathBuilder = new StringBuilder(256);
            m_dirStack = new List<string>(32);
            m_dirRoot = _dirRoot;
            m_dirNextDirectories = GetDirectories(m_dirRoot);
            m_dirNextFiles = GetFiles(m_dirRoot);
        }

        public string[] NextDirectories => m_dirNextDirectories;
        public string[] NextFiles => m_dirNextFiles;

        public string GetCurrentAbsoluteDirectory()
        {
            m_pathBuilder.Clear();
            m_pathBuilder.Append(m_dirRoot);

            foreach (string relativeDir in m_dirStack)
                m_pathBuilder.AppendFormat(@"\{0}", relativeDir);

            return m_pathBuilder.ToString();
        }

        public void MoveNext(int _index)
        {
            if (m_dirNextDirectories == null || _index < 0 || _index >= m_dirNextDirectories.Length)
                return;

            m_dirStack.Add(m_dirNextDirectories[_index]);
            string absdir = GetCurrentAbsoluteDirectory();
            m_dirNextDirectories = GetDirectories(absdir);
            m_dirNextFiles = GetFiles(absdir);
        }

        public void MoveBefore()
        {
            if (m_dirStack.Count == 0)
                return;

            m_dirStack.RemoveAt(m_dirStack.Count - 1);
            string absdir = GetCurrentAbsoluteDirectory();
            m_dirNextDirectories = GetDirectories(absdir);
            m_dirNextFiles = GetFiles(absdir);
        }

        public void MoveRoot()
        {
            m_dirStack.Clear();
            m_dirNextDirectories = GetDirectories(m_dirRoot);
            m_dirNextFiles = GetFiles(m_dirRoot);
        }

        private string[] GetDirectories(string _absoluteDirectory)
        {
            string[] directories = Directory.GetDirectories(_absoluteDirectory);

            for (int i = 0; i < directories.Length; ++i)
                directories[i] = Path.GetFileName(directories[i]);

            return directories;
        }

        private string[] GetFiles(string _absoluteDirectory)
        {
            string[] files = Directory.GetFiles(_absoluteDirectory);

            for (int i = 0; i < files.Length; ++i)
                files[i] = Path.GetFileName(files[i]);

            return files;
        }
    }
}
