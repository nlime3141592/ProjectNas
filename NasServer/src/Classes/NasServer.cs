using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAS
{
    public sealed class NasServer
    {
        public int maxClientCount { get; private set; } = 20;
        public int clientCount { get; private set; } = 0;

        private TcpListener m_server;
        private int m_port;

        private Thread m_thread;
        private Thread m_acThread;
        private volatile bool m_isOpened = false;
        private volatile bool m_isClosed = true;

        private NasFileSystem m_fileSystem;
        private ConcurrentQueue<AcceptedClient> m_clients;

        public NasServer(int _port)
        {
            m_server = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);
            m_port = _port;

            m_thread = new Thread(new ThreadStart(m_ThreadMain));
            m_acThread = new Thread(new ThreadStart(m_AcThreadMain));

            m_clients = new ConcurrentQueue<AcceptedClient>();
        }

        public bool TryOpen(string _rootStorageDirectory)
        {
            try
            {
                if (m_isOpened) // NOTE: 이미 한 번 열린 적이 있는 서버입니다.
                    return false;

                m_fileSystem = new NasFileSystem(_rootStorageDirectory);
                m_isOpened = true;
                m_isClosed = false;
                m_server.Start();
                m_thread.Start();
                m_acThread.Start();
                return true;
            }
            catch (Exception)
            {
                // NOTE: 예외 발생으로 서버를 열 수 없습니다.
                m_isClosed = true;
                return false;
            }
        }

        public bool TryClose()
        {
            try
            {
                if (m_isClosed) // NOTE: 닫혀 있는 서버입니다.
                    return false;

                m_isClosed = true;
                m_server.Stop();
                m_acThread.Interrupt();
                m_thread.Interrupt();
                return true;
            }
            catch (Exception _ex)
            {
                // NOTE: 예외 발생으로 서버를 닫을 수 없습니다.
                Console.WriteLine("[Server] {0}", _ex.Message);
                return false;
            }
        }

        private void m_ThreadMain()
        {
            Console.WriteLine("[Server] Started service handling.");

            AcceptedClient client = null;

            try
            {
                // NOTE: 클라이언트 상태를 체크합니다.
                while (m_isOpened && !m_isClosed)
                {
                    clientCount = m_clients.Count;

                    for (int i = 0; i < clientCount; ++i)
                    {
                        if (!m_clients.TryDequeue(out client))
                            m_KillClient(client);
                        else if (client.isStopped)
                            client.socModule.Close();
                        else
                            m_clients.Enqueue(client);
                    }
                }
            }
            catch (Exception)
            {
                // NOTE: 강제로 서버 Thread가 종료되었습니다.
            }

            if (client != null)
                m_KillClient(client);

            while (m_clients.Count > 0)
            {
                m_clients.TryDequeue(out client);
                m_KillClient(client);
            }

            Console.WriteLine("[Server] Stopped service handling.");
        }

        private void m_AcThreadMain()
        {
            this.WriteLog("Wait client.");

            try
            {
                while (m_isOpened && !m_isClosed)
                {
                    TcpClient tcpclnt = m_server.AcceptTcpClient();
                    SocketModule socModule = new SocketModule(tcpclnt, Encoding.UTF8);

                    string clientType = socModule.ReceiveString();
                    AcceptedClient client;

                    if (clientCount >= maxClientCount || !m_TrySwitchClient(out client, clientType))
                    {
                        socModule.SendString("<DENIED>");
                        socModule.Close();
                        continue;
                    }

                    client.fileSystem = m_fileSystem;
                    client.socModule = socModule;
                    client.socModule.SendString("<ACCEPTED>");
                    client.TryStart(); // TODO: 호출 순서가 중요한가? 맨 위에 있긴 했는데 점검해 볼 필요 있음.
                    m_clients.Enqueue(client);
                }
            }
            catch (Exception)
            {

            }

            this.WriteLog("Deny client.");
        }

        private bool m_TrySwitchClient(out AcceptedClient _acceptedClient, string _clientType)
        {
            switch (_clientType)
            {
                case "stdCLNT":
                    _acceptedClient = new StandardClient();
                    this.WriteLog("Accepted standard client.");
                    return true;
                default:
                    _acceptedClient = null;
                    return false;
            }
        }

        private void m_KillClient(AcceptedClient _client)
        {
            if (_client == null)
                return;

            _client.socModule.Close();
            _client.TryHalt();
        }
    }
}