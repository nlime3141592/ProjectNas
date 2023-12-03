using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NAS.Client
{
    public class ClientManager : NasThread
    {
        private static ClientManager s_m_manager;

        private Queue<NasService> m_serviceQueue;

        public static ClientManager GetInstance()
        {
            if (s_m_manager == null)
                s_m_manager = new ClientManager();

            return s_m_manager;
        }

        private ClientManager()
        {
            m_serviceQueue = new Queue<NasService>(8);
            base.SetThread(new Thread(new ThreadStart(ThreadMain)));
            base.Start();
        }

        public void RequestService(NasService _service)
        {
            m_serviceQueue.Enqueue(_service);
        }

        private void ThreadMain()
        {
            while(!base.isInterruptedStop)
            {
                while(m_serviceQueue.Count > 0)
                {
                    NasService service = m_serviceQueue.Dequeue();

                    (service as ISocketModuleService)?.Bind(NetworkManager.socModule);

                    ServiceResult result = service.Execute();

                    if (!HandleService(result))
                    {
                        m_serviceQueue.Clear();
                        break;
                    }
                }
            }
        }

        private bool HandleService(ServiceResult _result)
        {
            if(_result == ServiceResult.NetworkError)
            {
                base.Stop();
                return false;
            }

            return true;
        }
    }
}
