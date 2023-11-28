using System;
using System.Net.Sockets;
using System.Text;

namespace NAS
{
    public static class SocketExtensions
    {
        public static void SendString(this Socket _socket, String _string, Encoding _encoding)
        {
            byte[] bytes = _encoding.GetBytes(_string);

            _socket.SendBufferSize = 4;
            _socket.Send(BitConverter.GetBytes(bytes.Length));
            _socket.SendBufferSize = bytes.Length;
            _socket.Send(bytes);
        }

        public static string ReceiveString(this Socket _socket, byte[] _buffer, Encoding _encoding)
        {
            _socket.ReceiveBufferSize = 4;
            _socket.Receive(_buffer, 0, 4, SocketFlags.None);
            int len1 = BitConverter.ToInt32(_buffer, 0);
            _socket.ReceiveBufferSize = len1;
            int len2 = _socket.Receive(_buffer, 0, len1, SocketFlags.None);
            return _encoding.GetString(_buffer, 0, len2);
        }
    }
}
