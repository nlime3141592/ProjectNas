using System;

namespace NAS
{
    public class CSvLogin : NasService
    {
        public Action<int> onLoginSuccess;
        public Action onLoginFailure;
        public Action onError;

        public int uuid { get; private set; }
        public string response { get; private set; }

        private NasClient m_client;
        private string m_id;
        private string m_pw;

        public CSvLogin(NasClient _client, string _id, string _pw)
        {
            m_client = _client;
            m_id = _id;
            m_pw = _pw;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_LOGIN");
                m_client.socModule.SendString(m_id);
                m_client.socModule.SendString(m_pw);
                this.uuid = m_client.socModule.ReceiveInt32();
                this.response = m_client.socModule.ReceiveString();

                switch(this.response)
                {
                    case "<LOGIN_SUCCESS>":
                        onLoginSuccess?.Invoke(this.uuid);
                        return NasServiceResult.Success;
                    case "<LOGIN_FAILURE>":
                        onLoginFailure?.Invoke();
                        return NasServiceResult.Success;
                    default:
                        return NasServiceResult.Failure;
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
