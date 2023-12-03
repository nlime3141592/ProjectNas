using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace NAS.Server
{
    public partial class NasServer : NasThread
    {
        public int currentClientCount { get; private set; } = 0;

        public int maxClient = 10;

        private Socket m_serverSocket;
        private m_NasAcceptThread m_acceptThread;
        private ConcurrentQueue<NasThread> m_clientThreads;
        private Encoding m_encoding;

        public NasServer()
        {
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));

            // m_clientThreads = new List<NasThread>(Math.Max(1, maxClient));
            m_clientThreads = new ConcurrentQueue<NasThread>();
            m_encoding = Encoding.ASCII;
        }

        public bool TryOpen(int _port)
        {
            if (m_serverSocket != null || m_acceptThread != null)
                return false;

            try
            {
                m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
                m_serverSocket.Bind(endPoint);
                m_serverSocket.Listen(10);

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
                    m_serverSocket?.Close();
                    m_acceptThread = null;
                    m_serverSocket = null;
                }

                return false;
            }
        }

        public bool TryClose()
        {
            if (m_serverSocket == null)
                return false;

            m_acceptThread.Halt();
            m_acceptThread = null;

            m_serverSocket.Close();
            m_serverSocket = null;
            return true;
        }

        private void ThreadMain()
        {
            try
            {
                while (!base.isInterruptedStop)
                {
                    // NOTE: 서버에서 Thread의 상태를 인식합니다.
                    int threadCount = m_clientThreads.Count;
                    currentClientCount = threadCount;

                    while (threadCount-- > 0)
                    {
                        NasThread clientThread;

                        if (m_clientThreads.TryDequeue(out clientThread) && !clientThread.isEnded)
                            m_clientThreads.Enqueue(clientThread);
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
            while (m_clientThreads.Count > 0)
            {
                NasThread clientThread;

                if (m_clientThreads.TryDequeue(out clientThread))
                    clientThread.Halt();
            }

            isEnded = true;
        }
    }
}
