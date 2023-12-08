using System;
using System.Net.Sockets;
using System.IO;

namespace NAS
{
    public class SvLogin : NasService
    {
        private SocketModule m_socModule;

        public int uuid { get; private set; }
        public string response { get; private set; }

        private NasClient m_client;
        private string m_id;
        private string m_pw;

        public SvLogin(NasClient _client, string _id, string _pw)
        {
            m_client = _client;
            m_id = _id;
            m_pw = _pw;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("sv_login");
                m_client.socModule.SendString(m_id);
                m_client.socModule.SendString(m_pw);
                this.uuid = m_client.socModule.ReceiveInt32();
                this.response = m_client.socModule.ReceiveString();
                return NasServiceResult.Success;
            }
            catch(SocketException)
            {
                return NasServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return NasServiceResult.Error;
            }
        }
    }
}
