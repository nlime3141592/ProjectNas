using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace NAS.Tests
{
    public class NasClient
    {
        private readonly Socket m_socket;

        public NasClient(Socket _socket)
        {
            m_socket = _socket;
        }
    }
}
