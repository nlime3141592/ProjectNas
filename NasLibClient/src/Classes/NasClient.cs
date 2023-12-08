using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NAS
{
    public class NasClient : NasThread
    {
        public const int c_CONNECTION_CHECK_INTERVAL = 8;

        public SocketModule socModule { get; private set; }

        private Stopwatch m_watch;
        private ConcurrentQueue<NasService> m_services;

        public NasClient()
        {
            m_watch = new Stopwatch();
            m_services = new ConcurrentQueue<NasService>();
        }

        protected override void ThreadMain()
        {
            try
            {
                m_watch.Reset();
                m_watch.Start();

                while (base.isStarted && !base.isStopped)
                {
                    // NOTE: 10초마다 서버와의 연결 체크를 위한 서비스를 전송한다.
                    if (m_watch.ElapsedMilliseconds > 1000 * c_CONNECTION_CHECK_INTERVAL)
                    {
                        this.Request(new SvConnectionCheck(this));
                        m_watch.Restart();
                    }

                    if (m_services.Count <= 0)
                        continue;

                    NasService service;

                    if (!m_services.TryDequeue(out service))
                        continue;

                    NasServiceResult result = service.Execute();
                    m_watch.Restart();

                    if (result == NasServiceResult.NetworkError || result == NasServiceResult.Error)
                        break; // NOTE: 오류 발생하여 클라이언트 종료합니다.
                    else if (result == NasServiceResult.Loopback)
                        this.Request(service);
                }

                base.TryStop();
                m_watch.Stop();
            }
            catch (Exception)
            {
                base.TryStop();
                m_watch.Stop();
            }
        }

        public bool TryConnect(string _host, int _port)
        {
            Socket socket = null;

            try
            {
                TcpClient tcpclnt = new TcpClient(AddressFamily.InterNetwork);
                tcpclnt.Connect(new IPEndPoint(IPAddress.Parse(_host), _port));

                SocketModule module = new SocketModule(tcpclnt, Encoding.UTF8);

                module.SendString("stdCLNT");
                string response = module.ReceiveString();

                if (!response.Equals("<ACCEPTED>"))
                    return false;
                else if (!base.TryStart())
                    return false;

                socModule = module;
                Console.WriteLine("연결 성공");
                return true;
            }
            catch (Exception)
            {
                // NOTE: 서버에 연결할 수 없습니다.
                socket?.Close();
                return false;
            }
        }

        public void Request(NasService _service)
        {
            m_services.Enqueue(_service);
        }
    }
}