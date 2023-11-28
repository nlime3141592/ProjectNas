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
        public NasStandardClientThread(Socket _socket, Encoding _encoding)
        : base(_socket, _encoding)
        {

        }

        protected override NasService HandleService(string _serviceType)
        {
            Console.WriteLine("서비스타입: " + _serviceType);

            switch(_serviceType)
            {
                case "sv_test01":
                    return new SvTest01(base.socket, base.buffer, base.encoding);
                case "sv_discon":
                    return new SvDisconnect(this);
                default:
                    return null;
            }
        }
    }
}
