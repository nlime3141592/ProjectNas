using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NAS.Client
{
    public class NasClientThread : NasThread
    {
        private static NasClientThread s_m_manager;

        private ConcurrentQueue<NasService> m_serviceQueue;

        public static NasClientThread GetInstance()
        {
            if (s_m_manager == null)
                s_m_manager = new NasClientThread();

            return s_m_manager;
        }

        protected NasClientThread()
        {
            m_serviceQueue = new ConcurrentQueue<NasService>();
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));
        }

        public void RequestService(NasService _service)
        {
            m_serviceQueue.Enqueue(_service);
        }

        private void ThreadMain()
        {
            this.WriteLog("t시작");
            try
            {
                while (!base.isInterruptedStop)
                {
                    while (m_serviceQueue.Count > 0)
                    {
                        NasService service;

                        if (!m_serviceQueue.TryDequeue(out service))
                            break;

                        ServiceResult result = service.Execute();

                        // NOTE: 서비스 핸들링에 실패하면 클라이언트를 종료합니다.
                        if (!HandleService(result))
                        {
                            base.Stop();

                            NasService canceledService;

                            while (!m_serviceQueue.IsEmpty)
                                m_serviceQueue.TryDequeue(out canceledService);

                            break;
                        }
                    }
                }
            }
            catch(SocketException)
            {
                // NOTE: 서버와 통신할 수 없는 Thread입니다.
                base.Stop();
            }
            catch (ThreadInterruptedException)
            {
                // NOTE: 클라이언트의 서비스 Thread가 강제로 종료되었습니다.
                base.Stop();
            }
            catch (ThreadAbortException)
            {
                // NOTE: 클라이언트의 서비스 Thread가 강제로 종료되었습니다.
                base.Stop();
            }
            catch (Exception _ex)
            {
                // NOTE: 알 수 없는 예외 발생으로 서버가 종료되었습니다.
                base.Stop();
            }

            isEnded = true;
        }

        private bool HandleService(ServiceResult _result)
        {
            if (_result == ServiceResult.NetworkError)
                return false;

            return true;
        }
    }
}
