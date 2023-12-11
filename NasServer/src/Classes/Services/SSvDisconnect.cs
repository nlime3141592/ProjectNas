using System;

namespace NAS
{
    public class SSvDisconnect : NasService
    {
        private NasThread m_thread;

        public SSvDisconnect(NasThread _thread)
        {
            m_thread = _thread;
        }

        public override NasServiceResult Execute()
        {
            m_thread.TryHalt();
            return NasServiceResult.Success;
        }
    }
}