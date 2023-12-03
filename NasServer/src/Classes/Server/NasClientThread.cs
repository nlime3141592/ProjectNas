using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAS.Server
{
    public abstract class NasClientThread : NasThread
    {
        protected SocketModule m_socModule { get; private set; }

        protected NasClientThread(SocketModule _module)
        {
            m_socModule = _module;
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));
        }

        protected abstract NasService HandleService(string _serviceType);

        private void ThreadMain()
        {
            NasService service = null;
            ServiceResult result = null;

            Console.WriteLine("새로운 클라이언트를 시작합니다.");

            try
            {
                while (!base.isInterruptedStop)
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

                // NOTE:
                // 클라이언트가 정상 종료되었습니다.
                // 외부에서 NasThread.Stop()이 호출되어야만 이 곳에 진입하므로
                // NasThread.Stop()을 이 곳에서 호출하지 않아야 합니다.
            }
            catch (ThreadAbortException)
            {
                // NOTE: 클라이언트와의 연결이 강제로 종료되었습니다.
                base.Stop();
            }
            catch (Exception _ex)
            {
                // NOTE: 알 수 없는 예외 발생으로 클라이언트의 연결이 종료되었습니다.
                base.Stop();
            }

            OnThreadEnding();
            isEnded = true;
        }

        private void OnThreadEnding()
        {
            Console.WriteLine("클라이언트가 종료되었습니다.");
        }

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
