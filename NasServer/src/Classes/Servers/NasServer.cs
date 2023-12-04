using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using NAS.Server.Handler;

namespace NAS.Server
{
    public partial class NasServer : NasThread, IMessenger
    {
        public int currentClientCount { get; private set; } = 0;

        public int maxClient = 10;

        private ConcurrentQueue<NasHandler> m_users;
        private Encoding m_encoding;

        private Socket m_socServer;
        private m_NasAcceptThread m_acceptThread;

        public NasServer()
        {
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));

            m_users = new ConcurrentQueue<NasHandler>();
            m_encoding = Encoding.ASCII;
        }

        void IMessenger.RequestService(string _userName, NasService _service)
        {
            foreach(NasHandler handler in m_users)
            { 
                if(handler.handlerName.Equals(_userName))
                {
                    handler.RequestService(_service);
                    return;
                }
            }
        }

        public bool TryOpen(int _port)
        {
            if (m_socServer != null || m_acceptThread != null)
                return false;

            try
            {
                m_socServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
                m_socServer.Bind(endPoint);
                m_socServer.Listen(10);

                m_acceptThread = new m_NasAcceptThread(this);
                m_acceptThread.Start();

                return true;
            }
            catch (Exception _ex0)
            {
                Console.WriteLine("서버를 열 수 없습니다.");
                Console.WriteLine(_ex0.Message);

                try
                {
                    m_acceptThread?.Halt();
                }
                catch (Exception)
                {

                }
                finally
                {
                    m_socServer?.Close();
                    m_acceptThread = null;
                    m_socServer = null;
                }

                return false;
            }
        }

        public bool TryClose()
        {
            if (m_socServer == null)
                return false;

            m_acceptThread.Halt();
            m_acceptThread = null;

            m_socServer.Close();
            m_socServer = null;
            return true;
        }

        private void ThreadMain()
        {
            try
            {
                while (!base.isInterruptedStop)
                {
                    // NOTE: 서버에서 Thread의 상태를 인식합니다.
                    int threadCount = m_users.Count;
                    currentClientCount = threadCount;

                    while (threadCount-- > 0)
                    {
                        NasHandler clientThread;

                        if (m_users.TryDequeue(out clientThread) && !clientThread.isEnded)
                            m_users.Enqueue(clientThread);
                    }
                    // Thread.Sleep(1000); Console.WriteLine("동시 접속자 수: {0}", m_clientThreads.Count);
                }

                // NOTE: 서버가 정상 종료되었습니다.
                this.TryClose();
            }
            catch(ThreadInterruptedException)
            {
                // NOTE: 서버가 강제로 종료되었습니다.
                this.TryClose();
            }
            catch (ThreadAbortException)
            {
                // NOTE: 서버가 강제로 종료되었습니다.
                this.TryClose();
            }
            catch(Exception _ex)
            {
                // NOTE: 알 수 없는 예외 발생으로 서버가 종료되었습니다.
                this.TryClose();
            }

            // NOTE: 클라이언트 강제 종료 처리
            while (m_users.Count > 0)
            {
                NasHandler clientThread;

                if (m_users.TryDequeue(out clientThread))
                    clientThread.Halt();
            }

            isEnded = true;
        }
    }
}
