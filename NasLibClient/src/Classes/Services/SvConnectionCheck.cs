using System;

namespace NAS
{
    internal class SvConnectionCheck : NasService
    {
        private NasClient m_client;

        public SvConnectionCheck(NasClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("CONNECTION_CHECK");
                return NasServiceResult.Success;
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
