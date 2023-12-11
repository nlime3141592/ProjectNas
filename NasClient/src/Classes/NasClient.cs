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
        public const string c_HOST = "127.0.0.1"; // NOTE: 로컬 환경에서 시험하기 위한 ip 주소입니다.
        public const int c_PORT = 25565;
        public const int c_CONNECTION_CHECK_INTERVAL = 10;

        public static NasClient instance => s_m_client;

        private static NasClient s_m_client;

        public LoginData datLogin { get; private set; }
        public FileBrowseData datFileBrowse { get; private set; }
        public SocketModule socModule { get; private set; }

        private Stopwatch m_connectionCheckTimer; // NOTE: 서버와의 연결을 주기적으로 확인하기 위한 스탑워치 클래스입니다.
        private Stopwatch m_updateTimer; // TEST: 서비스 실행을 지연시키고 균일하게 제어하기 위한 스탑워치 클래스.
        private ConcurrentQueue<NasService> m_services; // NOTE: 서비스 객체를 수행하기 위한 서비스 큐입니다.

        private NasClient(SocketModule _socModule)
        {
            socModule = _socModule;
            datLogin = new LoginData();
            datFileBrowse = new FileBrowseData();

            m_connectionCheckTimer = new Stopwatch();
            m_updateTimer = new Stopwatch();
            m_services = new ConcurrentQueue<NasService>();
        }

        // NOTE: 서버와 연결을 시도합니다.
        public static bool TryConnectToServer()
        {
            Socket socket = null;

            try
            {
                s_m_client?.TryHalt();

                TcpClient tcpclnt = new TcpClient(AddressFamily.InterNetwork);
                tcpclnt.Connect(new IPEndPoint(IPAddress.Parse(c_HOST), c_PORT));

                SocketModule module = new SocketModule(tcpclnt, Encoding.UTF8);
                module.SendString("stdCLNT"); // NOTE: 클라이언트 유형을 전송합니다. 자세한 내용은 NasServer 프로젝트의 NasServer.cs 파일을 참조하세요.
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

        // NOTE: 서비스 객체를 등록합니다.
        public void Request(NasService _service)
        {
            if(isStarted && !isStopped)
                m_services.Enqueue(_service);
            else
            {
                try
                {
                    // NOTE: 만약 ThreadMain이 종료되었다면 독자적으로 수행합니다.
                    _service.Execute();
                }
                catch(Exception)
                {
                    
                }
            }
        }
    }
}