﻿using NAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NAS.Server.Service
{
    public sealed class SvTest01 : NasService, ISocketModuleService
    {
        private SocketModule m_socModule;

        void ISocketModuleService.Bind(SocketModule _module)
        {
            m_socModule = _module;
        }

        public override ServiceResult Execute()
        {
            try
            {
                this.WriteLog("서비스 시작");
                string message = m_socModule.ReceiveString();
                this.WriteLog("서비스가 메시지를 수신하였음. ({0})", message);
                this.WriteLog("서비스 종료");
                return ServiceResult.Success;
            }
            catch(SocketException)
            {
                this.WriteLog("통신 오류 발생");
                return ServiceResult.NetworkError;
            }
        }
    }
}
