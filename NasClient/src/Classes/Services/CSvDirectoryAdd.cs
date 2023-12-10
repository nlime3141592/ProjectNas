using System;

namespace NAS
{
    public class CSvDirectoryAdd : NasService
    {
        public Action<int, string> onAddSuccess;
        public Action onInvalidName;
        public Action onAddFailure;
        public Action onError;

        private NasClient m_client;
        private string m_nextDir;
        private int m_department;
        private int m_level;

        public CSvDirectoryAdd(NasClient _client, string _nextDir, int _department, int _level)
        {
            m_client = _client;
            m_nextDir = _nextDir.Trim();
            m_department = _department;
            m_level = _level;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_DIRECTORY_ADD");
                m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
                m_client.socModule.SendString(m_nextDir);
                m_client.socModule.SendInt32(m_department);
                m_client.socModule.SendInt32(m_level);
                string response = m_client.socModule.ReceiveString();

                switch(response)
                {
                    case "<SUCCESS>":
                        int didx = m_client.socModule.ReceiveInt32();
                        onAddSuccess?.Invoke(didx, m_nextDir);
                        return NasServiceResult.Success;
                    case "<INVALID_NAME>":
                        onInvalidName?.Invoke();
                        return NasServiceResult.Failure;
                    case "<FAILURE>":
                        onAddFailure?.Invoke();
                        return NasServiceResult.Failure;
                    default:
                        onError?.Invoke();
                        return NasServiceResult.Error;
                }
            }
            catch(Exception)
            {
                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
