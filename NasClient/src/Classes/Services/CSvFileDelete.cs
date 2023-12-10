using System;
using System.IO;

namespace NAS
{
    public class CSvFileDelete : NasService
    {
        public Action<int, string> onDeleteSuccess;
        public Action onDeleteFailure;
        public Action onError;

        private NasClient m_client;
        private string m_fileNameWithoutExt;
        private string m_fileExt;

        public CSvFileDelete(NasClient _client, string _fileName)
        {
            m_client = _client;

            string fileName = _fileName.Trim();
            m_fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            m_fileExt = Path.GetExtension(fileName);
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_FILE_DELETE");
                m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
                m_client.socModule.SendString(m_fileNameWithoutExt);
                m_client.socModule.SendString(m_fileExt);
                string response = m_client.socModule.ReceiveString();

                switch (response)
                {
                    case "<SUCCESS>":
                        int didx = m_client.socModule.ReceiveInt32();
                        onDeleteSuccess?.Invoke(didx, m_fileNameWithoutExt + m_fileExt);
                        return NasServiceResult.Success;
                    case "<FAILURE>":
                        onDeleteFailure?.Invoke();
                        return NasServiceResult.Failure;
                    default:
                        onError?.Invoke();
                        return NasServiceResult.Error;
                }
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
