using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    // NOTE: 회원가입 서비스 객체입니다.
    public class CSvJoin : NasService
    {
        public Action onJoinSuccess;
        public Action onJoinFailure;
        public Action onError;

        private NasClient m_client;
        private string m_id;
        private string m_pw;
        private string m_name;

        public CSvJoin(NasClient _client, string _id, string _pw, string _name)
        {
            m_client = _client;
            m_id = _id;
            m_pw = _pw;
            m_name = _name;
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_JOIN");
            m_client.socModule.SendString(m_id);
            m_client.socModule.SendString(m_pw);
            m_client.socModule.SendString(m_name);
            string response = m_client.socModule.ReceiveString();

            switch (response)
            {
                case "<JOIN_SUCCESS>": // NOTE: 회원가입 성공시 수신하는 문자열
                    onJoinSuccess?.Invoke();
                    return NasServiceResult.Success;
                case "<JOIN_FAILURE>":
                    onJoinFailure?.Invoke();
                    return NasServiceResult.Success;
                default:
                    return NasServiceResult.Failure;
            }
        }
    }
}
