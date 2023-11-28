using NAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NAS.Server.Service
{
    public sealed class SvTest01 : NasService
    {
        private Socket m_socket;
        private byte[] m_buffer;
        private Encoding m_encoding;

        public SvTest01(Socket _socket, byte[] _buffer, Encoding _encoding)
        {
            m_socket = _socket;
            m_buffer = _buffer;
            m_encoding = _encoding;
        }

        public override ServiceResult Execute()
        {
            try
            {
                base.ShowServiceLog("서비스 시작");
                string message = m_socket.ReceiveString(m_buffer, m_encoding);
                base.ShowServiceLogFormat("서비스가 메시지를 수신하였음. ({0})", message);
                base.ShowServiceLog("서비스 종료");
                return ServiceResult.Success;
            }
            catch(SocketException)
            {
                base.ShowServiceLog("통신 오류 발생");
                return ServiceResult.NetworkError;
            }
        }
    }
}
