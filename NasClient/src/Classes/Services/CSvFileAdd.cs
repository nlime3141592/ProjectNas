using System;
using System.IO;

namespace NAS
{
    public class CSvFileAdd : NasService
    {
        private const int c_BUFFER_SIZE = 1024 * 64;

        public Action<int, string> onAddSuccess;
        public Action onAddFailure;
        public Action onInvalidName;
        public Action onExistFile;
        public Action onError;

        private NasClient m_client;
        private string m_fileAbsPath;
        private string m_fileName;

        private FileStream m_fileStream;
        private byte[] m_buffer;
        private int m_loopTimes = 0;

        public CSvFileAdd(NasClient _client, string _fileAbsPath, string _fileName)
        {
            m_client = _client;
            m_fileAbsPath = _fileAbsPath;
            m_fileName = _fileName;
            m_buffer = new byte[c_BUFFER_SIZE];
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_FILE_ADD");
                m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
                m_client.socModule.SendString(m_fileName);
                m_client.socModule.SendInt32(m_loopTimes);
                string service_able = m_client.socModule.ReceiveString();

                switch (service_able)
                {
                    case "<INVALID_NAME>":
                        onInvalidName?.Invoke();
                        return NasServiceResult.Failure;
                    case "<EXIST_FILE>":
                        onExistFile?.Invoke();
                        return NasServiceResult.Failure;
                    case "<FILE_ADD_AVAILABLE>":
                        break;
                    default:
                        onError?.Invoke();
                        return NasServiceResult.Error;
                }

                m_fileStream = new FileStream(m_fileAbsPath, FileMode.Open, FileAccess.Read);

                if (m_fileStream.Length < m_loopTimes * c_BUFFER_SIZE)
                {
                    m_fileStream.Close();
                    m_client.socModule.SendString("<EOF>");
                    int fidx = m_client.socModule.ReceiveInt32();
                    onAddSuccess?.Invoke(fidx, m_fileName);
                    return NasServiceResult.Success;
                }
                else
                {
                    m_fileStream.Position = m_loopTimes * c_BUFFER_SIZE;
                    m_client.socModule.SendString("<WRITE>");

                    int readBytes = m_fileStream.Read(m_buffer, 0, m_buffer.Length);
                    m_client.socModule.TrySendVariableData(m_buffer, 0, readBytes, 1000);
                    ++m_loopTimes;
                    return NasServiceResult.Loopback;
                }
            }
            catch(Exception ex)
            {
                this.WriteLog("Message : {0}", ex.Message);
                this.WriteLog("StackTrace : {0}", ex.StackTrace);

                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
