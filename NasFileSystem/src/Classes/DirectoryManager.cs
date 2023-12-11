using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NAS
{
    // NOTE:
    // NAS Storage에 존재하는 폴더를 관리합니다.
    public class DirectoryManager
    {
        private const int c_DIRECTORY_MANAGER_TIMEOUT = 5;
        private static ConcurrentDictionary<string, DirectoryManager> s_m_managers;

        public string dirRoot => m_dirRoot;
        public bool isTimeout => m_watch.ElapsedMilliseconds >= 1000 * c_DIRECTORY_MANAGER_TIMEOUT;
        public int directoryCount => directories.Count;
        public int fileCount => files.Count;

        // NOTE: .dmeta 파일 정보입니다.
        #region .dmeta file datas
        private int headerSize = 24;
        private int idxInc = 0;
        private int ptrData => headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
        private int reserve01 = 0;
        private int reserve02 = 0;
        private int reserve03 = 0;

        private SortedList<int, string> directories;
        private SortedList<int, string> files;
        #endregion

        // NOTE: .fmeta 파일 정보입니다.
        #region .fmeta file datas
        private SortedList<int, int> departments;
        private SortedList<int, int> levels;
        #endregion

        private string m_dirRoot;
        private Encoding m_encoding;
        private Stopwatch m_watch;

        static DirectoryManager()
        {
            s_m_managers = new ConcurrentDictionary<string, DirectoryManager>();
        }

        // NOTE:
        // _dirRoot 폴더를 관리하는 객체를 생성합니다.
        // _dirRoot의 접근 권한 등 각종 기능을 사용할 수 있습니다.
        private DirectoryManager(string _dirRoot, Encoding _encoding)
        {
            directories = new SortedList<int, string>();
            files = new SortedList<int, string>();

            departments = new SortedList<int, int>();
            levels = new SortedList<int, int>();

            m_dirRoot = _dirRoot;
            m_encoding = _encoding;
            m_watch = new Stopwatch();
            m_watch.Start();

            // 만약, .dmeta 파일이 없다면 새로 생성합니다.
            if (!m_TryInitializeD())
                m_LoadMetaFileD();

            // 만약, .fmeta 파일이 없다면 새로 생성합니다.
            if (!m_TryInitializeP())
                m_LoadMetaFileP();
        }

        public static bool IsValidName(string _fileOrFolderName)
        {
            if (_fileOrFolderName == null || _fileOrFolderName.Length < 1 || _fileOrFolderName.Length > 128)
                return false;

            Regex unsupportedRegex = new Regex("(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";|/<>])+)|(([\\.]+))", RegexOptions.IgnoreCase);
            Match match = unsupportedRegex.Match(_fileOrFolderName);
            return !match.Success;
        }

        public static DirectoryManager Get(string _dirRoot, Encoding _encoding)
        {
            if (!s_m_managers.ContainsKey(_dirRoot))
                s_m_managers.TryAdd(_dirRoot, new DirectoryManager(_dirRoot, _encoding));

            return s_m_managers[_dirRoot];
        }

        public static void Update()
        {
            DirectoryManager manager;

            foreach(string key in s_m_managers.Keys)
                if (s_m_managers[key].isTimeout)
                    s_m_managers.TryRemove(key, out manager);
        }

        public IEnumerator<KeyValuePair<int, string>> GetDirectories()
        {
            m_watch.Restart();
            return directories.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, string>> GetFiles()
        {
            m_watch.Restart();
            return files.GetEnumerator();
        }

        // NOTE: 현재 폴더가 포함하고 있는 폴더가 접근 가능한 폴더인지 점검합니다.
        public bool IsPermittedUserForFolder(string _folderName, int _department, int _level)
        {
            int didx = GetFolderIndex(_folderName);

            if (departments[didx] == 0)
                return true;
            else if (departments[didx] == _department && levels[didx] <= _level)
                return true;
            else
                return false;
        }

        // NOTE: 현재 폴더가 포함하고 있는 파일이 접근 가능한 파일인지 점검합니다.
        public bool IsPermittedUserForFile(string _fileName, int _department, int _level)
        {
            int fidx = GetFileIndex(_fileName);

            if (departments[fidx] == 0)
                return true;
            else if (departments[fidx] == _department && levels[fidx] <= _level)
                return true;
            else
                return false;
        }

        public int GetFolderIndex(string _folderName)
        {
            return directories.FirstOrDefault((_kvPair) => _kvPair.Value == _folderName).Key;
        }

        public int GetFileIndex(string _fileName)
        {
            return files.FirstOrDefault((_kvPair) => _kvPair.Value == _fileName).Key;
        }

        // NOTE: 새로운 폴더를 추가합니다.
        public bool TryAddFolder(string _folderName, int _department, int _level)
        {
            m_watch.Restart();

            string directory = string.Format(@"{0}{1}\", m_dirRoot, _folderName);

            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (directories.Values.IndexOf(_folderName) >= 0)
                return false;

            int idxKey = ++idxInc;
            directories.Add(idxKey, _folderName);
            departments.Add(idxKey, _department);
            levels.Add(idxKey, _level);

            // NOTE: 파일 시스템 작업 후 즉시 meta파일을 갱신합니다.
            this.m_SaveMetaFileD();
            this.m_SaveMetaFileP();
            return true;
        }

        // NOTE: 새로운 파일을 추가합니다.
        public bool TryAddFile(string _fileName, int _department, int _level)
        {
            m_watch.Restart();

            string file = string.Format(@"{0}{1}\", m_dirRoot, _fileName);

            if(!Directory.Exists(file))
                Directory.CreateDirectory(file);

            if (files.Values.IndexOf(_fileName) >= 0)
                return false;

            int idxKey = ++idxInc;
            files.Add(idxKey, _fileName);
            departments.Add(idxKey, _department);
            levels.Add(idxKey, _level);

            this.m_SaveMetaFileD();
            this.m_SaveMetaFileP();
            return true;
        }

        // NOTE: 기존 폴더를 삭제합니다.
        public bool TryDeleteFolder(string _folderName)
        {
            m_watch.Restart();

            string directory = m_dirRoot + _folderName + '\\';

            foreach (int key in directories.Keys)
            {
                if (directories[key].Equals(_folderName))
                {
                    // NOTE: 디렉토리가 존재하지 않는데 Directory.Delete()를 호출하는 문제를 회피
                    if (Directory.Exists(directory))
                        m_rec_Delete(directory);

                    directories.Remove(key);
                    departments.Remove(key);
                    levels.Remove(key);

                    this.m_SaveMetaFileD();
                    this.m_SaveMetaFileP();
                    return true;
                }
            }

            return false;
        }

        // NOTE: 기존 파일을 삭제합니다.
        public bool TryDeleteFile(string _fileName)
        {
            m_watch.Restart();

            string file = m_dirRoot + _fileName + '\\';

            foreach (int key in files.Keys)
            {
                if (files[key].Equals(_fileName))
                {
                    // NOTE: 디렉토리가 존재하지 않는데 Directory.Delete()를 호출하는 문제를 회피
                    if (Directory.Exists(file))
                        m_rec_Delete(file);

                    files.Remove(key);
                    departments.Remove(key);
                    levels.Remove(key);

                    this.m_SaveMetaFileD();
                    this.m_SaveMetaFileP();
                    return true;
                }
            }

            return false;
        }

        // NOTE: meta파일의 생성/로드/저장을 관리하는 함수 영역입니다.
        #region Meta File Processing
        private bool m_TryInitializeD()
        {
            if (!Directory.Exists(m_dirRoot))
                Directory.CreateDirectory(m_dirRoot);
            else if (File.Exists(string.Format("{0}.dmeta", m_dirRoot)))
                return false;

            directories.Clear();
            files.Clear();

            headerSize = 24;
            idxInc = 0;
            reserve01 = 0;
            reserve02 = 0;
            reserve03 = 0;

            this.m_SaveMetaFileD();
            return true;
        }

        private bool m_TryInitializeP()
        {
            if (File.Exists(string.Format("{0}.fmeta", m_dirRoot)))
                return false;

            foreach(int key in directories.Keys)
            {
                departments.Add(key, 0);
                levels.Add(key, 0);
            }

            foreach(int key in files.Keys)
            {
                departments.Add(key, 0);
                levels.Add(key, 0);
            }

            this.m_SaveMetaFileP();
            return true;
        }

        private void m_LoadMetaFileD()
        {
            string path = string.Format("{0}.dmeta", m_dirRoot);

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader rd = new BinaryReader(stream, m_encoding);

            headerSize = rd.ReadInt32();
            idxInc = rd.ReadInt32();
            int ptrData = rd.ReadInt32();
            reserve01 = rd.ReadInt32();
            reserve02 = rd.ReadInt32();
            reserve03 = rd.ReadInt32();

            int countDirectory = rd.ReadInt32();
            directories.Clear();
            for (int i = 0; i < countDirectory; ++i)
                directories.Add(rd.ReadInt32(), null);

            int countFile = rd.ReadInt32();
            files.Clear();
            for (int i = 0; i < countFile; ++i)
                files.Add(rd.ReadInt32(), null);

            while (rd.BaseStream.Position < rd.BaseStream.Length)
            {
                byte fcode = rd.ReadByte();
                int idxKey = rd.ReadInt32();
                int fnlen = rd.ReadInt32();

                switch (fcode)
                {
                    case 0x0D: // directory
                        directories[idxKey] = m_encoding.GetString(rd.ReadBytes(fnlen));
                        break;
                    case 0x0A: // file
                        files[idxKey] = m_encoding.GetString(rd.ReadBytes(fnlen));
                        break;
                }
            }

            rd.Close();
            stream.Close();
        }

        private void m_LoadMetaFileP()
        {
            string path = string.Format("{0}.fmeta", m_dirRoot);

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader rd = new BinaryReader(stream, m_encoding);

            while(rd.BaseStream.Position < rd.BaseStream.Length)
            {
                int key = rd.ReadInt32();
                int department = rd.ReadInt32();
                int level = rd.ReadInt32();

                departments.Add(key, department);
                levels.Add(key, level);
            }

            rd.Close();
            stream.Close();
        }

        private void m_SaveMetaFileD()
        {
            string path = string.Format("{0}.dmeta", m_dirRoot);

            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter wr = new BinaryWriter(stream, m_encoding);

            wr.Write(headerSize);
            wr.Write(idxInc);
            wr.Write(ptrData);
            wr.Write(reserve01);
            wr.Write(reserve02);
            wr.Write(reserve03);

            wr.Write(directories.Count);
            foreach (int key in directories.Keys)
                wr.Write(key);
            wr.Write(files.Count);
            foreach (int key in files.Keys)
                wr.Write(key);

            foreach (int key in directories.Keys)
            {
                wr.Write((byte)0x0D);
                wr.Write(key);
                byte[] bytes = m_encoding.GetBytes(directories[key]);
                wr.Write(bytes.Length);
                wr.Write(bytes, 0, bytes.Length);
            }
            foreach (int key in files.Keys)
            {
                wr.Write((byte)0x0A);
                wr.Write(key);
                byte[] bytes = m_encoding.GetBytes(files[key]);
                wr.Write(bytes.Length);
                wr.Write(bytes, 0, bytes.Length);
            }

            wr.Close();
            stream.Close();
        }

        private void m_SaveMetaFileP()
        {
            string path = string.Format("{0}.fmeta", m_dirRoot);

            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter wr = new BinaryWriter(stream, m_encoding);

            foreach(int key in departments.Keys)
            {
                wr.Write(key);
                wr.Write(departments[key]);
                wr.Write(levels[key]);
            }

            wr.Close();
            stream.Close();
        }
        #endregion

        // NOTE: 현재 디렉토리로부터 하위의 모든 파일과 폴더를 지우기 위한 재귀 함수입니다.
        private void m_rec_Delete(string _directory)
        {
            foreach(string file in Directory.GetFiles(_directory))
                File.Delete(file);

            foreach (string directory in Directory.GetDirectories(_directory))
                m_rec_Delete(directory);

            Directory.Delete(_directory);
        }
    }
}
