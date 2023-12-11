using System;

namespace NAS
{
    public class ASvJoinAccept : NasService
    {
        public Action onAcceptSuccess;
        public Action onInvalidUuid;
        public Action onInvalidLevel;
        public Action onAcceptFailure;
        public Action onError;

        private NasAcceptor m_acceptor;
        private string m_uuidString;
        private string m_departmentName;
        private string m_levelString;

        public ASvJoinAccept(NasAcceptor _acceptor, string _uuidStirng, string _departmentName, string _levelString)
        {
            m_acceptor = _acceptor;
            m_uuidString = _uuidStirng;
            m_departmentName = _departmentName;
            m_levelString = _levelString;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                int uuid;
                int level;

                if (!int.TryParse(m_uuidString, out uuid) || m_acceptor.wAccounts.FindIndex((wdat) => wdat.uuid == uuid) < 0)
                {
                    onInvalidUuid?.Invoke();
                    return NasServiceResult.Failure;
                }
                else if(!int.TryParse(m_levelString, out level))
                {
                    onInvalidLevel?.Invoke();
                    return NasServiceResult.Failure;
                }

                m_acceptor.socModule.SendString("SV_JOIN_ACCEPT");
                m_acceptor.socModule.SendInt32(uuid);
                m_acceptor.socModule.SendString(m_departmentName);
                m_acceptor.socModule.SendInt32(level);
                string response = m_acceptor.socModule.ReceiveString();

                switch(response)
                {
                    case "<ACCEPT_SUCCESS>":
                        onAcceptSuccess?.Invoke();
                        return NasServiceResult.Success;
                    case "<ACCEPT_FAILURE>":
                        onAcceptFailure?.Invoke();
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
