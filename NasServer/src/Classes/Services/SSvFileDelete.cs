using System;
using System.Text;

namespace NAS
{
    public class SSvFileDelete : NasService
    {
        private AcceptedClient m_client;

        public SSvFileDelete(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                string fakedir = m_client.socModule.ReceiveString();
                string fileNameWithoutExt = m_client.socModule.ReceiveString();
                string fileExt = m_client.socModule.ReceiveString();

                if (!DirectoryManager.IsValidName(fileNameWithoutExt))
                {
                    m_client.socModule.SendString("<FAILURE>");
                    return NasServiceResult.Failure;
                }

                string absdir = m_client.fileSystem.FakeToPath(fakedir);
                DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);
                int fidx = manager.GetFileIndex(fileNameWithoutExt + fileExt);

                if (manager.TryDeleteFile(fileNameWithoutExt + fileExt))
                {
                    m_client.socModule.SendString("<SUCCESS>");
                    m_client.socModule.SendInt32(fidx);
                    return NasServiceResult.Success;
                }
                else
                {
                    m_client.socModule.SendString("<FAILURE>");
                    return NasServiceResult.Failure;
                }
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
