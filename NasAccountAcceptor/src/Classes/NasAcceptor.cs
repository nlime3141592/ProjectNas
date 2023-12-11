using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace NAS
{
    // NOTE: Account Acceptor의 구현 클래스입니다.
    public class NasAcceptor : NasThread
    {
        public const int c_CONNECTION_CHECK_INTERVAL = 10;

        public List<WaitingAccountData> wAccounts { get; private set; } // NOTE: 가입 승인 대기 계정 정보
        public List<DepartmentData> departments { get; private set; } // NOTE: 부서 리스트
        public SocketModule socModule { get; private set; }

        public Action onHaltedByException;

        private Stopwatch m_watch; // NOTE: 서버와의 연결 상태를 주기적으로 체크하기 위한 스탑워치 클래스
        private ConcurrentQueue<NasService> m_services; // NOTE: 서비스 객체를 순차적으로 처리하기 위한 서비스 큐

        public NasAcceptor()
        {
            wAccounts = new List<WaitingAccountData>();
            departments = new List<DepartmentData>();

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
                        this.Request(new ASvConnectionCheck(this));
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
            catch(Exception ex)
            {
                base.TryStop();
                m_watch.Stop();
                this.WriteLog("서비스 실행 오류 발생, {0}", ex.Message);
            }
        }

        // NOTE: 서버와 연결을 시도합니다.
        public bool TryConnect(string _host, int _port)
        {
            Socket socket = null;

            try
            {
                TcpClient tcpclnt = new TcpClient(AddressFamily.InterNetwork);
                tcpclnt.Connect(new IPEndPoint(IPAddress.Parse(_host), _port));

                SocketModule module = new SocketModule(tcpclnt, Encoding.UTF8);

                module.SendString("acptCLNT"); // NOTE: 클라이언트 유형을 전송합니다. 자세한 내용은 NasServer 프로젝트의 NasServer.cs 파일을 참조하세요.
                string response = module.ReceiveString();

                if (!response.Equals("<ACCEPTED>"))
                    return false;
                else if (!base.TryStart())
                    return false;

                socModule = module;
                this.WriteLog("Connected to server.");
                return true;
            }
            catch (Exception)
            {
                // NOTE: 서버에 연결할 수 없습니다.
                socket?.Close();
                return false;
            }
        }

        // NOTE: 서비스 객체를 등록합니다.
        public void Request(NasService _service)
        {
            if (isStarted && !isStopped)
                m_services.Enqueue(_service);
            else
            {
                try
                {
                    // NOTE: 만약 ThreadMain이 종료되었다면 독자적으로 수행합니다.
                    _service.Execute();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
