using System;
using System.Text;

namespace NAS
{
    public class SSvDirectoryDelete : NasService
    {
        private AcceptedClient m_client;

        public SSvDirectoryDelete(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            string fakedir = m_client.socModule.ReceiveString();
            string folderName = m_client.socModule.ReceiveString();

            if(!DirectoryManager.IsValidName(folderName))
            {
                m_client.socModule.SendString("<FAILURE>");
                return NasServiceResult.Failure;
            }

            string absdir = m_client.fileSystem.FakeToPath(fakedir);
            DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);
            int didx = manager.GetFolderIndex(folderName);

            if(manager.TryDeleteFolder(folderName))
            {
                m_client.socModule.SendString("<SUCCESS>");
                m_client.socModule.SendInt32(didx);
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
