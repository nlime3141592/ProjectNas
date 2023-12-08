using NAS.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                string fakedir = m_client.socModule.ReceiveString();
                string dirnext = m_client.socModule.ReceiveString();
                string absdir = m_client.fileSystem.FakeToPath(fakedir);

                switch(dirnext)
                {
                    case "":
                    case ".":
                        break;
                    case "..":
                        absdir = absdir.Substring(0, absdir.Length - 1);
                        int idxbackslach = absdir.LastIndexOf('\\');
                        if (idxbackslach > 0)
                            absdir = absdir.Substring(0, idxbackslach);
                        absdir += '\\';
                        break;
                    default:
                        absdir += (dirnext + '\\');
                        break;
                }

                m_client.socModule.SendString(m_client.fileSystem.PathToFake(absdir));

                DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

                m_client.socModule.SendInt32(manager.directoryCount);
                IEnumerator<KeyValuePair<int, string>> directories = manager.GetDirectories();
                while(directories.MoveNext())
                {
                    m_client.socModule.SendInt32(directories.Current.Key);
                    m_client.socModule.SendString(directories.Current.Value);
                }

                m_client.socModule.SendInt32(manager.fileCount);
                IEnumerator<KeyValuePair<int, string>> files = manager.GetFiles();
                while(files.MoveNext())
                {
                    m_client.socModule.SendInt32(files.Current.Key);
                    m_client.socModule.SendString(files.Current.Value);
                }

                return NasServiceResult.Success;
            }
            catch(Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
