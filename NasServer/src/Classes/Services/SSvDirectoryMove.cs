﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NAS
{
    public class SSvDirectoryMove : NasService
    {
        private AcceptedClient m_client;

        public SSvDirectoryMove(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            string fakedir = m_client.socModule.ReceiveString();
            string dirnext = m_client.socModule.ReceiveString();
            int department = m_client.socModule.ReceiveInt32();
            int level = m_client.socModule.ReceiveInt32();
            string absdir = m_client.fileSystem.FakeToPath(fakedir);

            // NOTE:
            // 수신한 다음 파일 경로에 따라 다른 폴더로 이동합니다.
            switch(dirnext)
            {
                case "": // NOTE: 현재 폴더의 정보를 갱신합니다.
                case ".":
                    break;
                case "..": // NOTE: 상위 폴더로 이동합니다.
                    absdir = absdir.Substring(0, absdir.Length - 1);
                    int idxbackslach = absdir.LastIndexOf('\\');
                    if (idxbackslach > 0)
                        absdir = absdir.Substring(0, idxbackslach);
                    absdir += '\\';
                    break;
                default: // NOTE: 하위 폴더로 이동합니다.
                    absdir += (dirnext + '\\');
                    break;
            }

            m_client.socModule.SendString(m_client.fileSystem.PathToFake(absdir));

            DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

            m_client.socModule.SendInt32(manager.directoryCount);
            IEnumerator<KeyValuePair<int, string>> directories = manager.GetDirectories();
            while(directories.MoveNext())
            {
                int didx = directories.Current.Key;
                string folderName = directories.Current.Value;

                if (!manager.IsPermittedUserForFolder(folderName, department, level))
                {
                    m_client.socModule.SendInt32(-1);
                    m_client.socModule.SendString("");
                }
                else
                {
                    m_client.socModule.SendInt32(didx);
                    m_client.socModule.SendString(folderName);
                }
            }

            m_client.socModule.SendInt32(manager.fileCount);
            IEnumerator<KeyValuePair<int, string>> files = manager.GetFiles();
            while(files.MoveNext())
            {
                int fidx = files.Current.Key;
                string fileName = files.Current.Value;

                if (!manager.IsPermittedUserForFile(fileName, department, level))
                {
                    m_client.socModule.SendInt32(-1);
                    m_client.socModule.SendString("");
                }
                else
                {
                    m_client.socModule.SendInt32(fidx);
                    m_client.socModule.SendString(fileName);
                }
            }

            return NasServiceResult.Success;
        }
    }
}
