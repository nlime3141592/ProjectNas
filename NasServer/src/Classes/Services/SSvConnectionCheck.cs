namespace NAS
{
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
