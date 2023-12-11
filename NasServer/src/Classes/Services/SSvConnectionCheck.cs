namespace NAS
{
    // NOTE: 서버 연결 체크를 위해 클라이언트는 주기적으로 이 서비스 객체를 요청합니다.
    public class SSvConnectionCheck : NasService
    {
        private AcceptedClient m_client;

        public SSvConnectionCheck(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            this.WriteLog("Checked.");
            return NasServiceResult.Success;
        }
    }
}
