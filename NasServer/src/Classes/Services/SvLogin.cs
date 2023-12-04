using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Server.Service
{
    public class SvLogin : NasService, ISocketModuleService
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
                string id = m_socModule.ReceiveString();
                string pw = m_socModule.ReceiveString();

                this.WriteLog("ID: {0}", id);
                this.WriteLog("PW: {0}", pw);

                Console.WriteLine("ID : {0}", id);
                Console.WriteLine("PW : {0}", pw);

                if(id.Equals("testid") && pw.Equals("testpw"))
                {
                    m_socModule.SendString("<LOGIN_SUCCESS>");
                    return ServiceResult.Success;
                }
                else
                {
                    m_socModule.SendString("<LOGIN_FAILURE>");
                    return ServiceResult.Failure;
                }
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
