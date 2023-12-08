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
            try
            {
                m_thread.TryHalt();
                return NasServiceResult.Success;
            }
            catch(Exception ex)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}