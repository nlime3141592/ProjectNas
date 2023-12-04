using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NAS.FileSystem
{
    public class DirectoryManager
    {
        public string dirRoot => m_dirRoot;

        private int headerSize = 24;
        private int idxInc = 0;
        private int ptrData = 0;
        private int reserve01 = 0;
        private int reserve02 = 0;
        private int reserve03 = 0;

        private SortedList<int, string> directories;
        private SortedList<int, string> files;

        private string m_dirRoot;
        private Encoding m_encoding;

        private DirectoryManager(string _dirRoot, Encoding _encoding)
        {
            directories = new SortedList<int, string>();
            files = new SortedList<int, string>();

            m_dirRoot = _dirRoot;
            m_encoding = _encoding;

            if (!m_TryInitialize())
                m_LoadMetaFile();
        }

        public static DirectoryManager Get(string _dirRoot, Encoding _encoding)
        {
            return new DirectoryManager(_dirRoot, _encoding);
        }

        public bool TryAddFolder(string _folderName)
        {
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
            string directory = string.Format(@"{0}{1}\", m_dirRoot, _folderName);

            foreach (int key in directories.Keys)
            {
                if (directories[key].Equals(_folderName))
                {
                    // NOTE: 디렉토리가 존재하지 않는데 Directory.Delete()를 호출하는 문제를 회피
                    if (Directory.Exists(directory))
                        Directory.Delete(directory);

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
    }
}
