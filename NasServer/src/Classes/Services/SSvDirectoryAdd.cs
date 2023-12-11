using System;
using System.Net.Sockets;
using System.Text;

namespace NAS
{
    public class SSvDirectoryAdd : NasService
    {
        private AcceptedClient m_client;

        public SSvDirectoryAdd(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            string fakedir = m_client.socModule.ReceiveString();
            string dirnext = m_client.socModule.ReceiveString();
            int department = m_client.socModule.ReceiveInt32();
            int level = m_client.socModule.ReceiveInt32();

            if(!DirectoryManager.IsValidName(dirnext))
            {
                m_client.socModule.SendString("<INVALID_NAME>");
                return NasServiceResult.Failure;
            }

            string absdir = m_client.fileSystem.FakeToPath(fakedir);
            DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

            if (manager.TryAddFolder(dirnext, department, level))
            {
                m_client.socModule.SendString("<SUCCESS>");
                m_client.socModule.SendInt32(manager.GetFolderIndex(dirnext));
                return NasServiceResult.Success;
            }
            else
            {
                m_client.socModule.SendString("<FAILURE>");
                return NasServiceResult.Failure;
            }
        }
    }
}
