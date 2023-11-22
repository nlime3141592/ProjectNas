using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DirectoryManager_FileSystemBrowser.cs
// 파일 시스템의 디렉토리 및 파일을 탐색합니다.
namespace NAS.Tests
{
    internal static partial class NasFileSystem
    {
        public static string[] GetNextDirectories(string _directory, object _permission)
        {
            List<string> directories = new List<string>(Directory.GetDirectories(_directory));

            for (int i = directories.Count - 1; i >= 0; --i)
                if (!s_m_CanAccessDirectory(directories[i], _permission))
                    directories.RemoveAt(i);

            return directories.ToArray();
        }

        public static string[] GetNextFiles(string _directory, object _permission)
        {
            List<string> files = new List<string>(
                Directory.GetDirectories(_directory)
                    .Where<string>((_dir) => _dir[0] == '.')
                );

            for (int i = nasFiles.Count - 1; i >= 0; --i)
            {
                if (!s_m_CanAccessDirectory(files[i], _permission))
                    files.RemoveAt(i);
                else
                    files[i] = files[i].Substring(1, files[i].Length - 1);
            }

            return files.ToArray();
        }

        private static bool s_m_CanAccessDirectory(string _directory, object _permission)
        {
            return true;
        }

        private static bool s_m_CanAccessFile(string _directory, object _permission)
        {
            return true;
        }
    }
}
