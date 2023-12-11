using System;
using System.IO;
using System.Text;

namespace NAS
{
    // NOTE: 파일 다운로드 서비스를 클라이언트에 제공합니다.
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
            FileStream fileStream = null;

            try
            {
                string fakedir = m_client.socModule.ReceiveString();
                string fileName = m_client.socModule.ReceiveString();
                int loopTimes = m_client.socModule.ReceiveInt32();

                string absdir = m_client.fileSystem.FakeToPath(fakedir);
                DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

                string path = absdir + fileName + '\\' + fileName + ".a";
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                if (fileStream.Length < loopTimes * c_BUFFER_SIZE)
                {
                    fileStream.Close();
                    m_client.socModule.SendString("<EOF>");
                    return NasServiceResult.Success;
                }
                else
                {
                    m_client.socModule.SendString("<READ>");

                    // NOTE:
                    // 서비스 1회 당 정해진 버퍼 사이즈가 있습니다.
                    // 클라이언트가 크기가 큰 파일을 수신하고자 할 때 서버 측 서비스 객체는
                    // 파일 포인터의 위치를 바꾸고, 파일을 읽습니다.
                    // 클라이언트는 파일 포인터가 EOF가 될 때까지 이 서비스 객체 수행을 요청합니다.
                    fileStream.Position = loopTimes * c_BUFFER_SIZE;
                    int readBytes = fileStream.Read(m_buffer, 0, c_BUFFER_SIZE);
                    // this.WriteLog("byte: {0}, lp = {1}", readBytes, loopTimes);
                    m_client.socModule.TrySendVariableData(m_buffer, 0, readBytes, 1000);
                    return NasServiceResult.Success;
                }
            }
            catch(Exception _exception)
            {
                fileStream?.Close();
                throw _exception;
            }
        }
    }
}
