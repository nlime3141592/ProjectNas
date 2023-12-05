using System;
using System.Threading;

namespace NAS.Client
{
    internal class ConnectionTask
    {
        public Thread thread { get; private set; }

        private Action<bool> m_callback;

        public ConnectionTask(Action<bool> _callback)
        {
            thread = new Thread(onTask);

            m_callback = _callback;
        }

        private void onTask()
        {
            bool isSuccess = ClientNetworkManager.TryConnectToServer("127.0.0.1", 25565, "usrCLNT");

            if(!isSuccess)
                Thread.Sleep(1000);

            m_callback?.Invoke(isSuccess);
        }
    }
}
