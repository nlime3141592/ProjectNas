using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace NAS.Server.Handler
{
    public abstract class NasHandler : NasThread
    {
        public string handlerName { get; set; }
        protected IMessenger messenger { get; private set; }

        protected SocketModule m_socModule { get; private set; }

        private ConcurrentQueue<NasService> m_serviceQueue;

        protected NasHandler(IMessenger _messenger, SocketModule _module)
        {
            messenger = _messenger;
            m_socModule = _module;
            m_serviceQueue = new ConcurrentQueue<NasService>();
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));
        }

        protected abstract NasService HandleService(string _serviceType);

        public override void Halt()
        {
            base.Halt();

            m_socModule.Close();
        }

        protected virtual void OnHandlerEnd() { }

        public void RequestService(NasService _service)
        {
            m_serviceQueue.Enqueue(_service);
        }

        private void ThreadMain()
        {
            this.WriteLog("새로운 클라이언트가 접속했습니다.");

            NasService service = null;
            ServiceResult result = null;

            try
            {
                while (!base.isInterruptedStop)
                {
                    NasService queuedService;

                    while (m_serviceQueue.TryPeek(out queuedService))
                        queuedService.Execute();

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
            catch(SocketException)
            {
                // NOTE: 소켓 오류로 클라이언트와의 연결이 종료됩니다.
                m_socModule.Close();
                base.Stop();
            }
            catch(ThreadInterruptedException)
            {
                // NOTE: 클라이언트와의 연결이 강제로 종료되었습니다.
                m_socModule.Close();
                base.Stop();
            }
            catch (ThreadAbortException)
            {
                // NOTE: 클라이언트와의 연결이 강제로 종료되었습니다.
                m_socModule.Close();
                base.Stop();
            }
            catch (Exception _ex)
            {
                // NOTE: 알 수 없는 예외 발생으로 클라이언트의 연결이 종료되었습니다.
                m_socModule.Close();
                base.Stop();
            }

            base.isEnded = true;
            OnHandlerEnd();
            this.WriteLog("클라이언트가 종료되었습니다.");
        }

        private void m_HandleServiceResult(ServiceResult _serviceResult)
        {
            // Console.WriteLine("통신 결과: {0} - ({1})", _serviceResult.value, _serviceResult.name);

            if (_serviceResult == ServiceResult.NetworkError)
            {
                base.Stop();
                return;
            }
        }
    }
}
