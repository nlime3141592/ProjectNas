﻿namespace NAS
{
    // NOTE: 회원가입 승인 프로그램용 서비스 스레드입니다.
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
