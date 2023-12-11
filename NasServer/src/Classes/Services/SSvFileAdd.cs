using System;
using System.IO;
using System.Text;

namespace NAS
{
    public class SSvFileAdd : NasService
    {
        private const int c_BUFFER_SIZE = 1024 * 64;

        private AcceptedClient m_client;

        public SSvFileAdd(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            FileStream fileStream = null;

            try
            {
                string fakedir = m_client.socModule.ReceiveString();
                string fileName = m_client.socModule.ReceiveString();
                string fileExtension = m_client.socModule.ReceiveString();
                int department = m_client.socModule.ReceiveInt32();
                int level = m_client.socModule.ReceiveInt32();
                int loopTimes = m_client.socModule.ReceiveInt32();

                string absdir = m_client.fileSystem.FakeToPath(fakedir);
                DirectoryManager manager = DirectoryManager.Get(absdir, Encoding.UTF8);

                if (!DirectoryManager.IsValidName(fileName))
                {
                    m_client.socModule.SendString("<INVALID_NAME>");
                    return NasServiceResult.Failure;
                }
                else if (loopTimes == 0 && !manager.TryAddFile(fileName + fileExtension, department, level))
                {
                    m_client.socModule.SendString("<EXIST_FILE>");
                    return NasServiceResult.Failure;
                }
                else
                    m_client.socModule.SendString("<FILE_ADD_AVAILABLE>");

                string serviceType = m_client.socModule.ReceiveString();

                switch (serviceType)
                {
                    case "<EOF>":
                        m_client.socModule.SendInt32(manager.GetFileIndex(fileName + fileExtension));
                        return NasServiceResult.Success;
                    case "<WRITE>":
                        // NOTE:
                        // 서비스 1회 당 정해진 버퍼 사이즈가 있습니다.
                        // 클라이언트가 크기가 큰 파일을 업로드 할 때 서버 측 서비스 객체는
                        // 파일 포인터의 위치를 계속 증가시키며 파일에 저장합니다. (Append와 같은 동작)
                        // 클라이언트는 클라이언트 측의 파일 포인터가 EOF가 될 때까지 이 서비스 객체 수행을 요청합니다.
                        byte[] buffer;
                        m_client.socModule.TryReceiveVariableData(out buffer, 1000);
                        string path = absdir + fileName + fileExtension + '\\' + fileName + fileExtension + ".a";
                        fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                        fileStream.Position = fileStream.Length;
                        fileStream.Write(buffer, 0, buffer.Length);
                        fileStream.Close();
                        return NasServiceResult.Success;
                    default:
                        return NasServiceResult.Failure;
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
