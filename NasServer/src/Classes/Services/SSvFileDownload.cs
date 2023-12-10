using System;
using System.IO;
using System.Text;

namespace NAS
{
    public class SSvFileDownload : NasService
    {
        private const int c_BUFFER_SIZE = 1024 * 64;

        private AcceptedClient m_client;
        private byte[] m_buffer;

        public SSvFileDownload(AcceptedClient _client)
        {
            m_client = _client;
            m_buffer = new byte[c_BUFFER_SIZE];
        }

        public override NasServiceResult Execute()
        {
            try
            {
                string fakedir = m_client.socModule.ReceiveString();
                string fileName = m_client.socModule.ReceiveString();
                int loopTimes = m_client.socModule.ReceiveInt32();

                string absdir = m_client.fileSystem.FakeToPath(fakedir);
                DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

                string path = absdir + fileName + '\\' + fileName + ".a";
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                if(fileStream.Length < loopTimes * c_BUFFER_SIZE)
                {
                    fileStream.Close();
                    m_client.socModule.SendString("<EOF>");
                    return NasServiceResult.Success;
                }
                else
                {
                    m_client.socModule.SendString("<READ>");

                    fileStream.Position = loopTimes * c_BUFFER_SIZE;
                    int readBytes = fileStream.Read(m_buffer, 0, c_BUFFER_SIZE);
                    // this.WriteLog("byte: {0}, lp = {1}", readBytes, loopTimes);
                    m_client.socModule.TrySendVariableData(m_buffer, 0, readBytes, 1000);
                    return NasServiceResult.Success;
                }
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
