using System;
using System.Net.Sockets;

namespace NAS.Client.Service
{
    public class SvInitialize : NasService
    {
        private string m_clientName;

        public SvInitialize(string _clientName)
        {
            m_clientName = _clientName;
        }

        public override ServiceResult Execute()
        {
            try
            {
                ClientNetworkManager.socModule.SendString("sv_init");
                ClientNetworkManager.socModule.SendString(m_clientName);
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
