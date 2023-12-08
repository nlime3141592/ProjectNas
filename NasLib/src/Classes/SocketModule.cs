using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAS
{
    public class SocketModule
    {
        private TcpClient tcpClient;
        private Encoding m_encoding;

        public SocketModule(TcpClient _tcpClient, Encoding _encoding)
        {
            tcpClient = _tcpClient;
            m_encoding = _encoding;
        }

        public void Close()
        {
            Monitor.Enter(tcpClient);
            tcpClient.Close();
            Monitor.Exit(tcpClient);
        }

        public void SendInt32(int _value, int _msTimeout = 1000)
        {
            byte[] bytes = BitConverter.GetBytes(_value);

            if (!TrySendFixedDate(bytes, 0, 4, _msTimeout))
                throw new SocketException();
        }

        public void SendString(string _string, int _msTimeout = 1000)
        {
            byte[] sBytes = m_encoding.GetBytes(_string);

            if (!TrySendVariableData(sBytes, 0, sBytes.Length, _msTimeout))
                throw new SocketException();
        }

        public int ReceiveInt32(int _msTimeout = 1000)
        {
            byte[] buffer;

            if (!TryReceiveFixedData(out buffer, 4, _msTimeout))
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

                Monitor.Enter(tcpClient);
                tcpClient.SendTimeout = _msTimeout;
                tcpClient.GetStream().Write(_buffer, _offset, _length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tcpClient.SendTimeout = -1;
                Monitor.Exit(tcpClient);
            }
        }

        public bool TryReceiveFixedData(out byte[] _newByteArray, int _length, int _msTimeout)
        {
            try
            {
                byte[] buffer = new byte[_length];
                int ptrFront = 0;

                Monitor.Enter(tcpClient);
                tcpClient.ReceiveTimeout = _msTimeout;

                while (ptrFront < buffer.Length)
                    ptrFront += tcpClient.GetStream().Read(buffer, ptrFront, buffer.Length - ptrFront);

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
                tcpClient.ReceiveTimeout = -1;
                Monitor.Exit(tcpClient);
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

                Monitor.Enter(tcpClient);
                tcpClient.SendTimeout = _msTimeout;
                tcpClient.GetStream().Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tcpClient.SendTimeout = -1;
                Monitor.Exit(tcpClient);
            }
        }

        public bool TryReceiveVariableData(out byte[] _newByteArray, int _msTimeout)
        {
            try
            {
                byte[] fixedBuffer = new byte[4];
                int ptrFront = 0;

                Monitor.Enter(tcpClient);
                tcpClient.ReceiveTimeout = _msTimeout;

                while (ptrFront < fixedBuffer.Length)
                    ptrFront += tcpClient.GetStream().Read(fixedBuffer, ptrFront, fixedBuffer.Length - ptrFront);

                int length = BitConverter.ToInt32(fixedBuffer, 0);
                byte[] variableBuffer = new byte[length];
                ptrFront = 0;

                while (ptrFront < variableBuffer.Length)
                    ptrFront += tcpClient.GetStream().Read(variableBuffer, ptrFront, variableBuffer.Length - ptrFront);

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
                tcpClient.ReceiveTimeout = -1;
                Monitor.Exit(tcpClient);
            }
        }
    }
}