using System.Threading;

namespace NAS
{
    public abstract class NasThread
    {
        public bool isRunning { get; private set; } = false;
        public bool isEnded { get; protected set; } = false;

        protected bool isInterruptedStop { get; private set; } = false;

        protected Thread m_thread;

        protected NasThread()
        {

        }

        public virtual void Start()
        {
            m_thread.Start();
        }

        public virtual void Stop()
        {
            isInterruptedStop = true;
        }

        public virtual void Halt()
        {
            m_thread.Interrupt();

            isInterruptedStop = true;
        }

        protected void SetThread(Thread _thread)
        {
            m_thread = _thread;
        }

        protected Thread GetThread()
        {
            return m_thread;
        }
    }
}
