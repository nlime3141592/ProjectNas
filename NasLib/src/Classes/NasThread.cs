using System;
using System.Net.Sockets;
using System.Threading;

namespace NAS
{
    public abstract class NasThread
    {
        public bool isRunning { get; private set; } = false;
        public bool isStopped { get; private set; } = false;

        protected bool p_isStopped { get; private set; } = false;

        private Thread m_thread;

        protected NasThread()
        {
            m_thread = new Thread(m_ThreadMain);
        }

        protected abstract void ThreadMain();
        protected virtual void OnThreadEnding() { }

        public void Start()
        {
            m_thread.Start();
        }

        public void Stop()
        {
            p_isStopped = true;
        }

        public void Abort()
        {
            m_thread.Abort();
        }

        private void m_ThreadMain()
        {
            try
            {
                ThreadMain();
            }
            catch (SocketException _socketException)
            {
                // NOTE: 서버와의 연결이 끊겼습니다.
                Console.WriteLine("클라이언트의 접속 종료 발생");
            }
            catch (ThreadAbortException _threadAbortException)
            {
                // NOTE: Thread가 강제 종료되었습니다.
            }

            OnThreadEnding();
            isStopped = true;
        }
    }
}
