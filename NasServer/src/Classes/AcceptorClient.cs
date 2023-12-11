namespace NAS
{
    public class AcceptorClient : AcceptedClient
    {
        protected override NasService HandleServiceHeader(string _serviceHeader)
        {
            switch(_serviceHeader)
            {
                case "SV_CONNECTION_CHECK":
                    return new SSvConnectionCheck(this);
                case "SV_ACCEPTOR_VIEW_UPDATE":
                    return new SSvAcceptorViewUpdate(this);
                case "SV_JOIN_DENY":
                    return new SSvJoinDeny(this);
                case "SV_JOIN_ACCEPT":
                    return new SSvJoinAccept(this);
                default:
                    return null;
            }
        }
    }
}
