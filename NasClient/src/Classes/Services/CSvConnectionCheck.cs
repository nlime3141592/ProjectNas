using System;

namespace NAS
{
    // NOTE: 서버와의 연결 상태 확인을 위해 주기적으로 전송하는 서비스 객체입니다.
    internal class CSvConnectionCheck : NasService
    {
        private NasClient m_client;

        public CSvConnectionCheck(NasClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_CONNECTION_CHECK");
            return NasServiceResult.Success;
        }
    }
}
