using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public class CSvJoin : NasService
    {
        public Action onJoinSuccess;
        public Action onJoinFailure;
        public Action onError;

        private NasClient m_client;
        private string m_id;
        private string m_pw;

        public CSvJoin(NasClient _client, string _id, string _pw)
        {
            m_client = _client;
            m_id = _id;
            m_pw = _pw;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_JOIN");
                m_client.socModule.SendString(m_id);
                m_client.socModule.SendString(m_pw);
                string response = m_client.socModule.ReceiveString();

                switch (response)
                {
                    case "<JOIN_SUCCESS>":
                        onJoinSuccess?.Invoke();
                        return NasServiceResult.Success;
                    case "<JOIN_FAILURE>":
                        onJoinFailure?.Invoke();
                        return NasServiceResult.Success;
                    default:
                        return NasServiceResult.Failure;
                }
            }
            catch (Exception)
            {
                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
