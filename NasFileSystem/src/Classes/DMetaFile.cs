using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NAS.FileSystem
{
    public class DMetaFile
    {
        public int headerSize = 24;
        public int idxInc = 0;
        public int ptrData = 0;
        public int reserve01 = 0;
        public int reserve02 = 0;
        public int reserve03 = 0;

        public SortedList<int, string> directories;
        public SortedList<int, string> files;

        public string dirRoot;
        public Encoding encoding;

        public DMetaFile(string _dirRoot, Encoding _encoding)
        {
            directories = new SortedList<int, string>();
            files = new SortedList<int, string>();

            dirRoot = _dirRoot;
            encoding = _encoding;
        }

        public void Load()
        {
            string path = string.Format("{0}.dmeta", dirRoot);

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
                        directories[idxKey] = encoding.GetString(rd.ReadBytes(dirStringLength));
                        break;
                    case 0x0A: // file
                        files[idxKey] = encoding.GetString(rd.ReadBytes(dirStringLength));
                        break;
                }
            }

            rd.Close();
            stream.Close();
        }

        public void Save()
        {
            string path = string.Format("{0}.dmeta", dirRoot);

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
                byte[] bytes = encoding.GetBytes(directories[key]);
                wr.Write(bytes.Length);
                wr.Write(bytes, 0, bytes.Length);
            }
            foreach (int key in files.Keys)
            {
                wr.Write((byte)0x0A);
                wr.Write(key);
                byte[] bytes = encoding.GetBytes(files[key]);
                wr.Write(bytes.Length);
                wr.Write(bytes, 0, bytes.Length);
            }

            wr.Close();
            stream.Close();
        }

        public void Initialize()
        {
            if (!Directory.Exists(dirRoot))
                Directory.CreateDirectory(dirRoot);

            directories.Clear();
            files.Clear();

            headerSize = 24;
            idxInc = 0;
            ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);
            reserve01 = 0;
            reserve02 = 0;
            reserve03 = 0;

            this.Save();
        }

        public void AddFolder(string _folderName)
        {
            string directory = string.Format(@"{0}{1}\", dirRoot, _folderName);

            if (Directory.Exists(directory)) // TODO: 디렉토리가 이미 존재하는데 dmeta에 추가되지 않은 경우에 대한 로직 처리 필요
                return;
            else
            {
                Directory.CreateDirectory(directory);
                int key = ++idxInc;
                directories.Add(key, _folderName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                this.Save();
            }
        }

        public void AddFile(string _fileName)
        {
            string file = string.Format(@"{0}{1}\", dirRoot, _fileName);

            if (Directory.Exists(file)) // TODO: 디렉토리가 이미 존재하는데 dmeta에 추가되지 않은 경우에 대한 로직 처리 필요
                return;
            else
            {
                Directory.CreateDirectory(file);
                int key = ++idxInc;
                files.Add(key, _fileName);
                ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                this.Save();
            }
        }

        public void DeleteFolder(string _folderName)
        {
            string directory = string.Format(@"{0}{1}\", dirRoot, _folderName);

            if (!Directory.Exists(directory))
                return;

            foreach (int key in directories.Keys)
            {
                if (directories[key].Equals(_folderName))
                {
                    Directory.Delete(directory);
                    directories.Remove(key);
                    ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                    this.Save();
                    break;
                }
            }
        }

        public void DeleteFile(string _fileName)
        {
            string file = string.Format(@"{0}{1}\", dirRoot, _fileName);

            if (!Directory.Exists(file))
                return;

            foreach (int key in files.Keys)
            {
                if (files[key].Equals(_fileName))
                {
                    Directory.Delete(file);
                    files.Remove(key);
                    ptrData = headerSize + (sizeof(int) + directories.Count) + (sizeof(int) + files.Count);

                    this.Save();
                    break;
                }
            }
        }
    }
}
