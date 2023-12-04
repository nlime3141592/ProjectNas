using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Server.Service
{
    public class SvDisconnect : NasService
    {
        private NasThread m_thread;

        public SvDisconnect(NasThread _thread)
        {
            m_thread = _thread;
        }

        public override ServiceResult Execute()
        {
            try
            {
                m_thread.Stop();
                return ServiceResult.Success;
            }
            catch(Exception ex)
            {
                return ServiceResult.NetworkError;
            }
        }
    }
}