using System;

namespace NAS
{
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
