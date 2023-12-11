using System;

namespace NAS
{
    // NOTE: 사용자의 회원 가입을 거절합니다.
    public class ASvJoinDeny : NasService
    {
        public Action onDenySuccess;
        public Action onInvalidUuid;
        public Action onDenyFailure;
        public Action onError;

        private NasAcceptor m_acceptor;
        private string m_uuidString;

        public ASvJoinDeny(NasAcceptor _acceptor, string _uuidString)
        {
            m_acceptor = _acceptor;
            m_uuidString = _uuidString;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                int uuid;

                if (!int.TryParse(m_uuidString, out uuid) || m_acceptor.wAccounts.FindIndex((wdat) => wdat.uuid == uuid) < 0)
                {
                    onInvalidUuid?.Invoke();
                    return NasServiceResult.Failure;
                }

                m_acceptor.socModule.SendString("SV_JOIN_DENY");
                m_acceptor.socModule.SendInt32(uuid);
                string response = m_acceptor.socModule.ReceiveString();

                switch(response)
                {
                    case "<DENY_SUCCESS>":
                        onDenySuccess?.Invoke();
                        return NasServiceResult.Success;
                    case "<DENY_FAILURE>":
                        onDenyFailure?.Invoke();
                        return NasServiceResult.Failure;
                    default:
                        onError?.Invoke();
                        return NasServiceResult.Error;
                }
            }
            catch(Exception)
            {
                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
