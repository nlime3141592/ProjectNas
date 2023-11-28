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
            m_thread.Stop();
            base.ShowServiceLog("클라이언트가 정상적으로 접속 종료했습니다.");
            return ServiceResult.Success;
        }
    }
}
