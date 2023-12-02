using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using NAS.Server.Service;

namespace NAS.Server
{
    public class NasStandardClientThread : NasClientThread
    {
        public NasStandardClientThread(SocketModule _module)
        : base(_module)
        {

        }

        protected override NasService HandleService(string _serviceType)
        {
            Console.WriteLine("서비스타입: " + _serviceType);

            switch(_serviceType)
            {
                case "sv_test01":
                    return new SvTest01();
                case "sv_discon":
                    return new SvDisconnect(this);
                case "sv_login":
                    return new SvLogin();
                default:
                    return null;
            }
        }
    }
}
