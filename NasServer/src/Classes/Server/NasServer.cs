using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace NAS.Server
{
    public partial class NasServer : NasThread
    {
        public int maxClient = 10;

        private Socket m_serverSocket;
        private m_NasAcceptThread m_acceptThread;
        private List<NasThread> m_clientThreads;
        private Encoding m_encoding;

        public NasServer()
        {
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));

            m_clientThreads = new List<NasThread>(Math.Max(1, maxClient));
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
                    m_acceptThread?.Abort();
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
            Console.WriteLine("[NasServer] TryClose 1");
            m_acceptThread.Abort(); // TODO: 이 곳에 오류가 발생하여 해결해야 합니다.
            Console.WriteLine("[NasServer] TryClose 2");
            m_acceptThread = null;
            Console.WriteLine("[NasServer] TryClose 3");
            m_serverSocket.Close();
            Console.WriteLine("[NasServer] TryClose 4");
            m_serverSocket = null;
            Console.WriteLine("[NasServer] TryClose 5");
            return true;
        }

        private void ThreadMain()
        {
            try
            {
                while (!base.isInterruptedStop)
                {
                    // NOTE: 서버에서 Thread의 상태를 인식합니다.
                    for (int i = m_clientThreads.Count - 1; i >= 0; --i)
                    {
                        if (m_clientThreads[i] == null || m_clientThreads[i].isEnded)
                            m_clientThreads.RemoveAt(i);
                    }

                    // Thread.Sleep(1000); Console.WriteLine("동시 접속자 수: {0}", m_clientThreads.Count);
                    Console.WriteLine("[NasServer] 서버 동작 중");
                }

                // NOTE: 서버가 정상 종료되었습니다.
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.41");
                this.TryClose();
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.42");
            }
            catch (ThreadAbortException)
            {
                // NOTE: 서버가 강제로 종료되었습니다.
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.31");
                this.TryClose();
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.32");
            }
            catch(Exception _ex)
            {
                // NOTE: 알 수 없는 예외 발생으로 서버가 종료되었습니다.
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.21");
                this.TryClose();
                Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.22");
            }
            Console.WriteLine("[NasServer] 서버가 정상 종료되었습니다.1");
            OnThreadEnding();
            isEnded = true;
        }

        private void OnThreadEnding()
        {
            while(m_clientThreads.Count > 0)
            {
                Console.WriteLine("[NasServer] 남은 클라이언트 : {0}", m_clientThreads.Count);
                m_clientThreads[0].Stop();
                while (!m_clientThreads[0].isEnded);
                m_clientThreads.RemoveAt(0);
            }
        }
    }
}
