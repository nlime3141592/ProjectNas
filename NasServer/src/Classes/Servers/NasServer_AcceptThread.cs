using NAS.Server.Handler;
using System;
using System.Net.Sockets;
using System.Threading;

namespace NAS.Server
{
    public partial class NasServer
    {
        private class m_NasAcceptThread : NasThread
        {
            private NasServer m_server;

            public m_NasAcceptThread(NasServer _server)
            {
                base.SetThread(new Thread(new ThreadStart(ThreadMain)));
                m_server = _server;
            }

            private void ThreadMain()
            {
                try
                {
                    while (!base.isInterruptedStop)
                    {
                        if (m_server.m_users.Count >= m_server.maxClient)
                            continue;

                        m_ReceiveClient();
                    }

                    // NOTE: Thread가 정상 종료되었습니다.
                }
                catch(ThreadInterruptedException)
                {
                    // NOTE: Thread가 강제로 종료되었습니다.
                    base.Stop();
                }
                catch(ThreadAbortException)
                {
                    // NOTE: Thread가 강제로 종료되었습니다.
                    base.Stop();
                }
                catch(Exception)
                {
                    // NOTE: 알 수 없는 오류로 Thread가 종료되었습니다.
                    base.Stop();
                }
            }

            private void m_ReceiveClient()
            {
                Socket socClient = null;

                try
                {
                    socClient = m_server.m_socServer.Accept();
                    SocketModule socModule = new SocketModule(socClient, m_server.m_encoding);
                    string clientType = socModule.ReceiveString();
                    NasHandler clientThread = m_ParseClientType(socModule, clientType);

                    if (clientThread == null)
                    {
                        socModule.SendString("<DENIED>");
                        socModule.Close();
                    }
                    else
                    {
                        socModule.SendString("<ACCEPTED>");
                        clientThread.Start();

                        Monitor.Enter(m_server.m_users);
                        m_server.m_users.Enqueue(clientThread);
                        Monitor.Exit(m_server.m_users);
                    }
                }
                catch (SocketException)
                {
                    // NOTE: 클라이언트 소켓에 오류가 발생하였습니다.
                    socClient?.Close();
                }
                catch (Exception)
                {
                    // NOTE: 클라이언트를 수신하는 중 알 수 없는 오류가 발생하였습니다.
                    socClient?.Close();
                }
            }

            private NasHandler m_ParseClientType(SocketModule _socModule, string _clientType)
            {
                switch(_clientType)
                {
                    case "tstCLNT":
                        return new NasTestHandler(m_server, _socModule);
                    case "strCLNT":
                        return new NasStorageHandler(m_server, _socModule);
                    case "usrCLNT":
                        return new NasUserHandler(m_server, _socModule);
                    default:
                        return null;
                }
            }
        }
    }
}
