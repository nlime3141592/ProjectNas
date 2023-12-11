using System;
using System.Threading;

namespace NAS
{
    // NOTE: 독자 Thread 클래스를 구성하여 생명주기를 제어합니다.
    public abstract class NasThread
    {
        public bool isStarted => m_isStarted;
        public bool isStopped => m_isStopped;

        private Thread m_thread;
        private volatile bool m_isStarted = false;
        private volatile bool m_isStopped = true;

        protected NasThread()
        {
            m_thread = new Thread(new ThreadStart(ThreadMain));
        }

        protected abstract void ThreadMain();

        public bool TryStart()
        {
            try
            {
                if (m_isStarted)
                    return false; // NOTE: 이미 한 번 실행된 적이 있는 Thread입니다.

                m_isStarted = true;
                m_isStopped = false;
                m_thread.Start();
                return true;
            }
            catch (Exception)
            {
                m_isStopped = true;
                return false;
            }
        }

        public bool TryStop()
        {
            try
            {
                if (m_isStopped)
                    return false;

                m_isStopped = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryHalt()
        {
            try
            {
                if (m_isStopped)
                    return false;

                m_isStopped = true;
                m_thread.Interrupt();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}