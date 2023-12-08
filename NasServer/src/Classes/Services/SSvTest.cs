using System;

namespace NAS
{
    public sealed class SSvTest : NasService
    {
        private AcceptedClient m_client;

        public SSvTest(AcceptedClient _client)
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
