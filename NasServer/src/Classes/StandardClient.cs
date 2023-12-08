using System;

namespace NAS
{
    public sealed class StandardClient : AcceptedClient
    {
        protected override NasService HandleServiceHeader(string _serviceHeader)
        {
            switch (_serviceHeader)
            {
                case "CONNECTION_CHECK":
                    return new SSvConnectionCheck(this);
                case "SV_LOGIN":
                    return new SSvLogin(this);
                case "SV_JOIN":
                    return new SSvJoin(this);
                default:
                    return null;
            }
        }
    }
}