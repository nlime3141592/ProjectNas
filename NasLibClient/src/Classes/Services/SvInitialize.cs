using System;
using System.Net.Sockets;

namespace NAS
{
    public class SvInitialize : NasService
    {
        private NasClient m_client;
        private string m_clientName;

        public SvInitialize(NasClient _client, string _clientName)
        {
            m_client = _client;
            m_clientName = _clientName;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("sv_init");
                m_client.socModule.SendString(m_clientName);
                return NasServiceResult.Success;
            }
            catch(SocketException)
            {
                return NasServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
