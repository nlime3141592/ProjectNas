using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace NAS.Server
{
    public class NasClientThread : NasThread
    {
        private Socket m_socket;
        private byte[] m_buffer;

        public NasClientThread(Socket _socket)
        {
            m_socket = _socket;
            m_buffer = new byte[4096];
        }

        protected override void ThreadMain()
        {
            Console.WriteLine("새로운 클라이언트를 시작합니다.");

            while (!base.p_isStopped)
            {
                int length = m_socket.Receive(m_buffer);

                if (length != 1) // NOTE: 통신 오류로 클라이언트 종료.
                    base.Stop();
                else
                    m_HandleService(m_buffer[0]);
            }

            Console.WriteLine("클라이언트가 종료되었습니다.");
        }

        private void m_HandleService(byte _serviceType)
        {
            switch (_serviceType)
            {
                case 0:
                    Console.WriteLine("Client Thread를 종료합니다.");
                    base.Stop();
                    break;
                default:
                    Console.WriteLine("서비스 번호: {0}", _serviceType);
                    break;
            }
        }
    }
}
