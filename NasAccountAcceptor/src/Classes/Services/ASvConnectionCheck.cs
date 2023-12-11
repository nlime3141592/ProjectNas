using System;

namespace NAS
{
    // NOTE: 서버와의 연결 상태를 주기적으로 체크하는 서비스 클래스입니다.
    public class ASvConnectionCheck : NasService
    {
        private NasAcceptor m_acceptor;

        public ASvConnectionCheck(NasAcceptor _acceptor)
        {
            m_acceptor = _acceptor;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_acceptor.socModule.SendString("SV_CONNECTION_CHECK");
                return NasServiceResult.Success;
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
