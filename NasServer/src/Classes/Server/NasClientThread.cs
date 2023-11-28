using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.Server
{
    public abstract class NasClientThread : NasThread
    {
        protected Socket socket { get; private set; }
        protected Encoding encoding { get; private set; }
        protected byte[] buffer { get; private set; }

        public NasClientThread(Socket _socket, Encoding _encoding)
        {
            socket = _socket;
            encoding = _encoding;
            buffer = new byte[4096];
        }

        protected override void ThreadMain()
        {
            Console.WriteLine("새로운 클라이언트를 시작합니다.");

            while (!base.p_isStopped)
            {
                int length = socket.Receive(buffer, 0, 32, SocketFlags.None);
                string clientType = encoding.GetString(buffer, 0, length);

                if(length > 0)
                    m_HandleServiceResult(HandleService(clientType)?.Execute() ?? ServiceResult.InvalidService);
                else // NOTE: 통신 오류로 클라이언트 종료.
                    base.Stop();
            }

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
