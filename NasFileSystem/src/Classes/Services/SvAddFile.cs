using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.FileSystem.Service
{
    public class SvAddFile : NasService
    {
        private string m_currentDirectory;
        private string m_fileName;
        private Encoding m_encoding;

        public SvAddFile(string _currentDirectory, string _fileName, Encoding _encoding)
        {
            m_currentDirectory = _currentDirectory;
            m_fileName = _fileName;
            m_encoding = _encoding;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                DirectoryManager manager = DirectoryManager.Get(m_currentDirectory, m_encoding);

                if (manager.TryAddFile(m_fileName))
                    return NasServiceResult.Success;
                else
                    return NasServiceResult.Failure;
            }
            catch (SocketException)
            {
                return NasServiceResult.NetworkError;
            }
            catch (Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
