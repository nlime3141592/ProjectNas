using System;
using System.Net.Sockets;
using System.IO;
namespace NAS.Client.Service
{
    public class SvLogin : NasService
    {
        private SocketModule m_socModule;

        public int uuid { get; private set; }
        public string response { get; private set; }

        private string m_id;
        private string m_pw;

        public SvLogin(string _id, string _pw)
        {
            m_id = _id;
            m_pw = _pw;
        }

        public override ServiceResult Execute()
        {
            try
            {
                ClientNetworkManager.socModule.SendString("sv_login");
                ClientNetworkManager.socModule.SendString(m_id);
                ClientNetworkManager.socModule.SendString(m_pw);
                this.uuid = ClientNetworkManager.socModule.ReceiveInt32();
                this.response = ClientNetworkManager.socModule.ReceiveString();
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
