using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Tests
{
    internal static partial class NasFileSystem
    {
        // STORAGE_ROOT => SYSTEM_ROOT로 이름을 치환할 수 있다?
        // STORAGE_ROOT:25565/FirstRootDir/ ... 과 같은 형식으로 파일 경로를 만들까?

        public static readonly ConcurrentDictionary<string, NasFile> nasFiles; // NOTE: Enabled file directories.
        public static readonly List<string> absRootDirectories; // NOTE: Root absolute directories, should query from database.

        static NasFileSystem()
        {
            nasFiles = new ConcurrentDictionary<string, NasFile>();
            absRootDirectories = new List<string>(10);
        }

        public static string GetFileString(string _directory, string _fileName)
        {
            return string.Format("{0}.{1}/", _directory, _fileName);
        }

        // NOTE: 파일 하나에 접근합니다.
        public static NasFile GetFile(string _directory, string _fileName)
        {
            if (nasFiles[_directory] == null)
                nasFiles[_directory] = new NasFile(_directory, _fileName, 10);

            return nasFiles[_directory];
        }
    }
}
