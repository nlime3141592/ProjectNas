using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public class CSvDirectoryDelete : NasService
    {
        public Action<int, string> onDeleteSuccess;
        public Action onDeleteFailure;
        public Action onError;

        private NasClient m_client;
        private string m_folderName;

        public CSvDirectoryDelete(NasClient _client, string _folderName)
        {
            m_client = _client;
            m_folderName = _folderName.Trim();
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_DIRECTORY_DELETE");
            m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
            m_client.socModule.SendString(m_folderName);
            string response = m_client.socModule.ReceiveString();

            switch(response)
            {
                case "<SUCCESS>":
                    int didx = m_client.socModule.ReceiveInt32();
                    onDeleteSuccess?.Invoke(didx, m_folderName);
                    return NasServiceResult.Success;
                case "<FAILURE>":
                    onDeleteFailure?.Invoke();
                    return NasServiceResult.Failure;
                default:
                    onError?.Invoke();
                    return NasServiceResult.Error;
            }
        }
    }
}
