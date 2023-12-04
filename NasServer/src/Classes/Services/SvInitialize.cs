using NAS.Server.Handler;
using System;
using System.Net.Sockets;

namespace NAS.Server.Service
{
    public class SvInitialize : NasService, ISocketModuleService
    {
        private SocketModule m_socModule;
        private NasHandler m_handler;

        public SvInitialize(NasHandler _handler)
        {
            m_handler = _handler;
        }

        void ISocketModuleService.Bind(SocketModule _module)
        {
            m_socModule = _module;
        }

        public override ServiceResult Execute()
        {
            try
            {
                m_handler.handlerName = m_socModule.ReceiveString();
                this.WriteLog("name of handler: {0}", m_handler.handlerName);
                return ServiceResult.Success;
            }
            catch(SocketException)
            {
                return ServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return ServiceResult.NetworkError;
            }
        }
    }
}
