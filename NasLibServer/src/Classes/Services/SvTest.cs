using System;

namespace NAS
{
    public sealed class SvTest : NasService
    {
        private AcceptedClient m_client;

        public SvTest(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                string message = m_client.socModule.ReceiveString();
                this.WriteLog("Receive: {0}", message);
                return NasServiceResult.Success;
            }
            catch(Exception)
            {
                this.WriteLog("Error service.");
                return NasServiceResult.NetworkError;
            }
        }
    }
}
