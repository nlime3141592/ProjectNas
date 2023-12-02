using System;
using System.Net.Sockets;

namespace NAS.Server
{
    public partial class NasServer
    {
        private class m_NasAcceptThread : NasThread
        {
            private NasServer m_server;

            public m_NasAcceptThread(NasServer _server)
            {
                m_server = _server;
            }

            protected override void ThreadMain()
            {
                while(true)
                {
                    if(m_server.m_clientThreads.Count >= m_server.maxClient)
                        continue;

                    Socket clientSocket = m_server.m_serverSocket.Accept();
                    SocketModule socModule = new SocketModule(clientSocket, m_server.m_encoding);
                    string clientType = socModule.ReceiveString();
                    NasClientThread clientThread = m_ParseClientType(socModule, clientType);

                    if(clientThread == null)
                    {
                        socModule.SendString("<DENIED>");
                        socModule.Close();
                    }
                    else
                    {
                        socModule.SendString("<ACCEPTED>");
                        clientThread.Start();
                        m_server.m_clientThreads.Add(clientThread);
                    }
                }
            }

            private NasClientThread m_ParseClientType(SocketModule _socModule, string _clientType)
            {
                switch(_clientType)
                {
                    case "stdCLNT":
                        return new NasStandardClientThread(_socModule);
                    default:
                        return null;
                }
            }
        }
    }
}
