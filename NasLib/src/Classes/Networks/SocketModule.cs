using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAS
{
    public class SocketModule
    {
        private Socket m_socket;
        private Encoding m_encoding;

        public SocketModule(Socket _socket, Encoding _encoding)
        {
            m_socket = _socket;
            m_encoding = _encoding;
        }

        public void Close()
        {
            m_socket.Close();
        }

        public void SendInt32(int _value)
        {
            byte[] bytes = BitConverter.GetBytes(_value);

            if (!TrySendFixedDate(bytes, 0, 4, 1000))
                throw new SocketException();
        }

        public void SendString(string _string)
        {
            byte[] sBytes = m_encoding.GetBytes(_string);

            if (!TrySendVariableData(sBytes, 0, sBytes.Length, 1000))
                throw new SocketException();
        }

        public int ReceiveInt32()
        {
            byte[] buffer;

            if (!TryReceiveFixedData(out buffer, 4, 1000))
                throw new SocketException();

            return BitConverter.ToInt32(buffer, 0);
        }

        public string ReceiveString(int _msTimeout = 1000)
        {
            byte[] buffer;

            if (!TryReceiveVariableData(out buffer, _msTimeout))
                throw new SocketException();

            return m_encoding.GetString(buffer);
        }

        public bool TrySendFixedDate(byte[] _buffer, int _offset, int _length, int _msTimeout)
        {
            try
            {
                if (_buffer == null || _buffer.Length <= 0)
                    return false;

                Monitor.Enter(m_socket);
                m_socket.SendTimeout = _msTimeout;
                m_socket.Send(_buffer, _offset, _length, SocketFlags.None);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_socket.SendTimeout = -1;
                Monitor.Exit(m_socket);
            }
        }

        public bool TryReceiveFixedData(out byte[] _newByteArray, int _length, int _msTimeout)
        {
            try
            {
                byte[] buffer = new byte[_length];
                int ptrFront = 0;

                Monitor.Enter(m_socket);
                m_socket.ReceiveTimeout = _msTimeout;

                while (ptrFront < buffer.Length)
                    ptrFront += m_socket.Receive(buffer, ptrFront, buffer.Length - ptrFront, SocketFlags.None);

                _newByteArray = buffer;
                return true;
            }
            catch (Exception)
            {
                _newByteArray = null;
                return false;
            }
            finally
            {
                m_socket.ReceiveTimeout = -1;
                Monitor.Exit(m_socket);
            }
        }

        public bool TrySendVariableData(byte[] _buffer, int _offset, int _length, int _msTimeout)
        {
            try
            {
                byte[] lBytes = BitConverter.GetBytes(_length);
                byte[] bytes = new byte[lBytes.Length + _length];
                Buffer.BlockCopy(lBytes, 0, bytes, 0, lBytes.Length);
                Buffer.BlockCopy(_buffer, _offset, bytes, lBytes.Length, _length);

                Monitor.Enter(m_socket);
                m_socket.SendTimeout = _msTimeout;
                m_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_socket.SendTimeout = -1;
                Monitor.Exit(m_socket);
            }
        }

        public bool TryReceiveVariableData(out byte[] _newByteArray, int _msTimeout)
        {
            try
            {
                byte[] fixedBuffer = new byte[4];
                int ptrFront = 0;

                Monitor.Enter(m_socket);
                m_socket.ReceiveTimeout = _msTimeout;

                while (ptrFront < fixedBuffer.Length)
                    ptrFront += m_socket.Receive(fixedBuffer, ptrFront, fixedBuffer.Length - ptrFront, SocketFlags.None);

                int length = BitConverter.ToInt32(fixedBuffer, 0);
                byte[] variableBuffer = new byte[length];
                ptrFront = 0;

                while (ptrFront < variableBuffer.Length)
                    ptrFront += m_socket.Receive(variableBuffer, ptrFront, variableBuffer.Length - ptrFront, SocketFlags.None);

                _newByteArray = variableBuffer;
                return true;
            }
            catch (Exception)
            {
                _newByteArray = null;
                return false;
            }
            finally
            {
                m_socket.ReceiveTimeout = -1;
                Monitor.Exit(m_socket);
            }
        }
    }
}