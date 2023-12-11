using System;

namespace NAS
{
    internal class ASvConnectionCheck : NasService
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
                m_acceptor.socModule.SendString("CONNECTION_CHECK");
                return NasServiceResult.Success;
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
