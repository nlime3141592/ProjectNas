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
        public const string c_HOST = "192.168.35.31";
        public const int c_PORT = 25565;
        public const int c_CONNECTION_CHECK_INTERVAL = 10;

        public static NasClient instance => s_m_client;

        private static NasClient s_m_client;

        public LoginData datLogin { get; private set; }
        public FileBrowseData datFileBrowse { get; private set; }
        public SocketModule socModule { get; private set; }

        private Stopwatch m_connectionCheckTimer;
        private Stopwatch m_updateTimer;
        private ConcurrentQueue<NasService> m_services;

        private NasClient(SocketModule _socModule)
        {
            socModule = _socModule;
            datLogin = new LoginData();
            datFileBrowse = new FileBrowseData();

            m_connectionCheckTimer = new Stopwatch();
            m_updateTimer = new Stopwatch();
            m_services = new ConcurrentQueue<NasService>();
        }

        public static bool TryConnectToServer()
        {
            Socket socket = null;

            try
            {
                s_m_client?.TryHalt();

                TcpClient tcpclnt = new TcpClient(AddressFamily.InterNetwork);
                tcpclnt.Connect(new IPEndPoint(IPAddress.Parse(c_HOST), c_PORT));

                SocketModule module = new SocketModule(tcpclnt, Encoding.UTF8);
                module.SendString("stdCLNT");
                string response = module.ReceiveString();

                if(response.Equals("<ACCEPTED>"))
                {
                    s_m_client = new NasClient(module);
                    return s_m_client.TryStart();
                }

                // NOTE: 서버에 연결할 수 없습니다.
                socket?.Close();
                return false;
            }
            catch (Exception)
            {
                // NOTE: 서버에 연결할 수 없습니다.
                socket?.Close();
                return false;
            }
        }

        public void Close()
        {
            socModule.Close();
        }

        protected override void ThreadMain()
        {
            try
            {
                m_connectionCheckTimer.Reset();
                m_connectionCheckTimer.Start();
                m_updateTimer.Start();

                while (base.isStarted && !base.isStopped)
                {
                    // NOTE: 10초마다 서버와의 연결 체크를 위한 서비스를 전송한다.
                    if (m_connectionCheckTimer.ElapsedMilliseconds > 1000 * c_CONNECTION_CHECK_INTERVAL)
                    {
                        this.Request(new CSvConnectionCheck(this));
                        m_connectionCheckTimer.Restart();
                    }

                    if (m_services.Count <= 0)
                        continue;

                    NasService service;

                    if (!m_services.TryDequeue(out service))
                        continue;

                    // NOTE:
                    // 서비스를 실행합니다.
                    // 만약 서비스에서 Exception이 throwing되면 클라이언트를 즉시 종료할 수 있는 로직을 마련합니다.
                    NasServiceResult result = service.Execute();

                    m_connectionCheckTimer.Restart();

                    if (result == NasServiceResult.Loopback)
                        this.Request(service);

                    m_updateTimer.Restart();
                }

                // NOTE: 정상 종료되어 이 코드로 나왔습니다.
                m_connectionCheckTimer.Stop();
                m_updateTimer.Stop();
            }
            catch (Exception _exception)
            {
                m_connectionCheckTimer.Stop();
                m_updateTimer.Stop();
            }
        }

        public void Request(NasService _service)
        {
            if(isStarted && !isStopped)
                m_services.Enqueue(_service);
            else
            {
                try
                {
                    _service.Execute();
                }
                catch(Exception)
                {
                    
                }
            }
        }
    }
}