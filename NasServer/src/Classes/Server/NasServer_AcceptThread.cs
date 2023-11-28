using NAS;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Server
{
    public partial class NasServer
    {
        private class m_NasAcceptThread : NasThread
        {
            private NasServer m_server;
            private byte[] m_buffer;

            public m_NasAcceptThread(NasServer _server)
            {
                m_server = _server;
                m_buffer = new byte[32];
            }

            protected override void ThreadMain()
            {
                while(true)
                {
                    if(m_server.m_clientThreads.Count >= m_server.maxClient)
                        continue;

                    Socket clientSocket = m_server.m_serverSocket.Accept();
                    string clientType = m_ReceiveAndParseClientType(clientSocket);
                    NasClientThread clientThread = m_ParseClientType(clientSocket, clientType);

                    if(clientThread == null)
                    {
                        clientSocket.SendString("<DENIED>", m_server.m_encoding);
                        clientSocket.Close();
                    }
                    else
                    {
                        clientSocket.SendString("<ACCEPTED>", m_server.m_encoding);
                        clientThread.Start();
                    }
                }
            }

            private string m_ReceiveAndParseClientType(Socket _clientSocket)
            {
                return _clientSocket.ReceiveString(m_buffer, m_server.m_encoding);
            }

            private NasClientThread m_ParseClientType(Socket _clientSocket, string _clientType)
            {
                switch(_clientType)
                {
                    case "stdCLNT":
                        return new NasStandardClientThread(_clientSocket, m_server.m_encoding);
                    default:
                        return null;
                }
            }
        }
    }
}
