using System;
using System.Net.Sockets;
using System.Threading;

namespace NAS
{
    public abstract class NasThread
    {
        public bool isRunning { get; private set; } = false;
        public bool isEnded { get; protected set; } = false;

        protected bool isInterruptedStop { get; private set; } = false;

        private Thread m_thread;

        protected NasThread()
        {

        }

        public void Start()
        {
            m_thread.Start();
        }

        public void Stop()
        {
            isInterruptedStop = true;
            m_thread = null;
        }

        public void Abort()
        {
            try
            {
                Console.WriteLine("[NasThread] Abort0");
                // m_thread.Abort();
                m_thread.Interrupt();
                Console.WriteLine("[NasThread] Abort1");
            }
            catch(Exception _ex)
            {
                Console.WriteLine(_ex.Message);
            }
            Console.WriteLine("[NasThread] Abort2");
            isInterruptedStop = true;
            Console.WriteLine("[NasThread] Abort3");
            m_thread = null;
        }

        protected void SetThread(Thread _thread)
        {
            m_thread = _thread;
        }

        protected Thread GetThread(Thread _thread)
        {
            return m_thread;
        }
    }
}
