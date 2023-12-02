using System;
using System.Net.Sockets;

namespace NAS.Client.Service
{
    public class SvLogin : NasService, ISocketModuleService
    {
        private SocketModule m_socModule;

        public Action onSuccess { get; set; } = null;
        public Action onFailure { get; set; } = null;

        private string m_id;
        private string m_pw;

        public SvLogin(string _id, string _pw)
        {
            m_id = _id;
            m_pw = _pw;
        }

        void ISocketModuleService.Bind(SocketModule _module)
        {
            m_socModule = _module;
        }

        public override ServiceResult Execute()
        {
            try
            {
                m_socModule.SendString("sv_login");
                m_socModule.SendString(m_id);
                m_socModule.SendString(m_pw);
                string response = m_socModule.ReceiveString();
                base.ShowServiceLogFormat("Response : {0}", response);
                onSuccess?.Invoke();
                return ServiceResult.Success;
            }
            catch(SocketException)
            {
                return ServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return ServiceResult.Error;
            }
        }
    }
}
