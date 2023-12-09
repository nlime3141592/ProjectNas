using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NAS
{
    public class DirectoryManager
    {
        private const int c_DIRECTORY_MANAGER_TIMEOUT = 5;
        private static ConcurrentDictionary<string, DirectoryManager> s_m_managers;

        public string dirRoot => m_dirRoot;
        public bool isTimeout => m_watch.ElapsedMilliseconds >= 1000 * c_DIRECTORY_MANAGER_TIMEOUT;
        public int directoryCount => directories.Count;
        public int fileCount => files.Count;

        #region .dmeta file datas
        private int headerSize = 24;
        private int idxInc = 0;
        private int ptrData = 0;
        private int reserve01 = 0;
        private int reserve02 = 0;
        private int reserve03 = 0;

        private SortedList<int, string> directories;
        private SortedList<int, string> files;
        #endregion

        private string m_dirRoot;
        private Encoding m_encoding;
        private Stopwatch m_watch;

        static DirectoryManager()
        {
            s_m_managers = new ConcurrentDictionary<string, DirectoryManager>();
        }

        private DirectoryManager(string _dirRoot, Encoding _encoding)
        {
            directories = new SortedList<int, string>();
            files = new SortedList<int, string>();

            m_dirRoot = _dirRoot;
            m_encoding = _encoding;
            m_watch = new Stopwatch();
            m_watch.Start();

            if (!m_TryInitialize())
                m_LoadMetaFile();
        }

        public static bool IsValidName(string _fileOrFolderName)
        {
            if (_fileOrFolderName == null || _fileOrFolderName.Length < 1 || _fileOrFolderName.Length > 128)
                return false;

            string charset = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_ 0123456789";

            for (int i = 0; i < _fileOrFolderName.Length; ++i)
                if (!charset.Contains(_fileOrFolderName[i]))
                    return false;

            return true;
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

        public int GetFolderIndex(string _folderName)
        {
            return directories.FirstOrDefault((_kvPair) => _kvPair.Value == _folderName).Key;
        }

        public int GetFileIndex(string _fileName)
        {
            return files.FirstOrDefault((_kvPair) => _kvPair.Value == _fileName).Key;
        }

        public bool TryAddFolder(string _folderName)
        {
            m_watch.Restart();

            string directory = string.Format(@"{0}{1}\", m_dirRoot, _folderName);

            if (Directory.Exists(directory)) // NOTE: 디렉토리가 이미 존재하는데 .dmeta에 추가되지 않은 경우에 대한 로직 처리
            {
                foreach (int key in directories.Keys)
                    if (directories[key].Equals(_folderName))
                        return false;

                int idxKey = ++idxInc;
                directories.Add(idxKey, _folderName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
                this.m_SaveMetaFile();
                return true;
            }
            else
            {
                Directory.CreateDirectory(directory);
                int idxKey = ++idxInc;
                directories.Add(idxKey, _folderName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
                this.m_SaveMetaFile();
                return true;
            }
        }

        public bool TryAddFile(string _fileName)
        {
            m_watch.Restart();

            string file = string.Format(@"{0}{1}\", m_dirRoot, _fileName);

            if (Directory.Exists(file)) // NOTE: 디렉토리가 이미 존재하는데 .dmeta에 추가되지 않은 경우에 대한 로직 처리
            {
                foreach (int key in files.Keys)
                    if (files[key].Equals(_fileName))
                        return false;

                int idxKey = ++idxInc;
                files.Add(idxKey, _fileName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
                this.m_SaveMetaFile();
                return true;
            }
            else
            {
                Directory.CreateDirectory(file);
                int idxKey = ++idxInc;
                files.Add(idxKey, _fileName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
                this.m_SaveMetaFile();
                return true;
            }
        }

        public bool TryDeleteFolder(string _folderName)
        {
            m_watch.Restart();

            string directory = string.Format(@"{0}{1}\", m_dirRoot, _folderName);

            foreach (int key in directories.Keys)
            {
                if (directories[key].Equals(_folderName))
                {
                    // NOTE: 디렉토리가 존재하지 않는데 Directory.Delete()를 호출하는 문제를 회피
                    if (Directory.Exists(directory))
                    {
                        m_rec_Delete(directory);
                        Directory.Delete(directory);
                    }

                    directories.Remove(key);
                    ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                    this.m_SaveMetaFile();
                    return true;
                }
            }

            return false;
        }

        public bool TryDeleteFile(string _fileName)
        {
            m_watch.Restart();

            string file = string.Format(@"{0}{1}\", m_dirRoot, _fileName);

            foreach (int key in files.Keys)
            {
                if (files[key].Equals(_fileName))
                {
                    // NOTE: 디렉토리가 존재하지 않는데 Directory.Delete()를 호출하는 문제를 회피
                    if (Directory.Exists(file))
                        Directory.Delete(file);

                    files.Remove(key);
                    ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                    this.m_SaveMetaFile();
                    return true;
                }
            }

            return false;
        }

        private bool m_TryInitialize()
        {
            if (!Directory.Exists(m_dirRoot))
                Directory.CreateDirectory(m_dirRoot);
            else if (File.Exists(string.Format("{0}.dmeta", m_dirRoot)))
                return false;

            directories.Clear();
            files.Clear();

            headerSize = 24;
            idxInc = 0;
            ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
            reserve01 = 0;
            reserve02 = 0;
            reserve03 = 0;

            this.m_SaveMetaFile();
            return true;
        }

        private void m_LoadMetaFile()
        {
            string path = string.Format("{0}.dmeta", m_dirRoot);

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader rd = new BinaryReader(stream, m_encoding);

            headerSize = rd.ReadInt32();
            idxInc = rd.ReadInt32();
            ptrData = rd.ReadInt32();
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
                byte dcode = rd.ReadByte();
                int idxKey = rd.ReadInt32();
                int dirStringLength = rd.ReadInt32();

                switch (dcode)
                {
                    case 0x0D: // directory
                        directories[idxKey] = m_encoding.GetString(rd.ReadBytes(dirStringLength));
                        break;
                    case 0x0A: // file
                        files[idxKey] = m_encoding.GetString(rd.ReadBytes(dirStringLength));
                        break;
                }
            }

            rd.Close();
            stream.Close();
        }

        private void m_SaveMetaFile()
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

        private void m_rec_Delete(string _directory)
        {
            foreach(string file in Directory.GetFiles(_directory))
                File.Delete(file);

            foreach (string directory in Directory.GetDirectories(_directory))
            {
                m_rec_Delete(directory);
                Directory.Delete(directory);
            }
        }
    }
}
