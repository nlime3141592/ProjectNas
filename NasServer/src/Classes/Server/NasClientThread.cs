using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.Server
{
    public abstract class NasClientThread : NasThread
    {
        protected SocketModule m_socModule { get; private set; }

        public NasClientThread(SocketModule _module)
        {
            m_socModule = _module;
        }

        protected sealed override void ThreadMain()
        {
            NasService service = null;
            ServiceResult result = null;

            Console.WriteLine("새로운 클라이언트를 시작합니다.");

            while (!base.p_isStopped)
            {
                string serviceType = m_socModule.ReceiveString(-1);

                if (serviceType == null)
                    base.Stop(); // NOTE: 통신 오류로 클라이언트 종료.
                else if ((service = HandleService(serviceType)) == null)
                    base.Stop(); // NOTE: 서비스 핸들링 오류로 클라이언트 종료.
                else
                {
                    (service as ISocketModuleService)?.Bind(m_socModule);
                    result = service.Execute();
                    m_HandleServiceResult(result);
                }
            }
        }

        protected override void OnThreadEnding()
        {
            base.OnThreadEnding();
            Console.WriteLine("클라이언트가 종료되었습니다.");
        }

        protected abstract NasService HandleService(string _serviceType);

        private void m_HandleServiceResult(ServiceResult _serviceResult)
        {
            Console.WriteLine("통신 결과: {0} - ({1})", _serviceResult.value, _serviceResult.name);

            if (_serviceResult == ServiceResult.NetworkError)
            {
                base.Stop();
                return;
            }
        }
    }
}
