using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace NAS.Server
{
    public class NasServer : NasThread
    {
        public int maxClient = 10;

        private Socket m_serverSocket;
        private Thread m_acceptThread;
        private List<NasThread> m_clientThreads;
        private byte[] m_buffer;
        private Encoding m_encoding;

        public NasServer()
        {
            m_clientThreads = new List<NasThread>(Math.Max(1, maxClient));
            m_buffer = new byte[4096];
            m_encoding = Encoding.UTF8;
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

                m_acceptThread = new Thread(m_OnAcceptThread);
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

            m_acceptThread.Abort();
            m_acceptThread = null;

            m_serverSocket.Close();
            m_serverSocket = null;
            return true;
        }

        protected override void ThreadMain()
        {
            while (true)
            {
                for (int i = m_clientThreads.Count - 1; i >= 0; --i)
                {
                    if (m_clientThreads[i] == null || m_clientThreads[i].isStopped)
                        m_clientThreads.RemoveAt(i);
                }

                // Thread.Sleep(1000); Console.WriteLine("동시 접속자 수: {0}", m_clientThreads.Count);
            }
        }

        protected override void OnThreadEnding()
        {
            base.OnThreadEnding();

            this.TryClose();
        }

        #region Codes on AcceptThread
        private void m_OnAcceptThread()
        {
            try
            {
                while (true)
                {
                    if (m_clientThreads.Count >= maxClient)
                        continue;

                    Socket clientSocket = m_serverSocket.Accept();

                    int length = clientSocket.Receive(m_buffer);

                    if (length != 1)
                        clientSocket.Send(m_encoding.GetBytes("<DENIED>"));
                    else
                        m_CreateClient(clientSocket);
                }
            }
            catch (ThreadAbortException _threadAbortException)
            {

            }
        }

        private void m_CreateClient(Socket _clientSocket)
        {
            byte clientType = m_buffer[0];

            switch (clientType)
            {
                case 1:
                    NasClientThread thread = new NasClientThread(_clientSocket);
                    thread.Start();
                    m_clientThreads.Add(thread);
                    _clientSocket.Send(m_encoding.GetBytes("<ACCEPTED>"));
                    break;
                default:
                    _clientSocket.Send(m_encoding.GetBytes("<DENIED>"));
                    _clientSocket.Close();
                    break;
            }
        }
        #endregion
    }
}
