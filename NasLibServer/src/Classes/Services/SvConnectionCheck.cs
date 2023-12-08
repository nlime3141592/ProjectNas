namespace NAS
{
    public class SvConnectionCheck : NasService
    {
        private AcceptedClient m_client;

        public SvConnectionCheck(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            return NasServiceResult.Success;
        }
    }
}
