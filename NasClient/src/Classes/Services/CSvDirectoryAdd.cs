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

        public CSvDirectoryAdd(NasClient _client, string _nextDir)
        {
            m_client = _client;
            m_nextDir = _nextDir;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_DIRECTORY_ADD");
                m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
                m_client.socModule.SendString(m_nextDir);
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
