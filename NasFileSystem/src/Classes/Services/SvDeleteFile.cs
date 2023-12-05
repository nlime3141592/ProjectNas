using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.FileSystem.Service
{
    class SvDeleteFile : NasService
    {
        private string m_currentDirectory;
        private string m_fileName;
        private Encoding m_encoding;

        public SvDeleteFile(string _currentDirectory, string _fileName, Encoding _encoding)
        {
            m_currentDirectory = _currentDirectory;
            m_fileName = _fileName;
            m_encoding = _encoding;
        }

        public override ServiceResult Execute()
        {
            try
            {
                DirectoryManager manager = DirectoryManager.Get(m_currentDirectory, m_encoding);

                if (manager.TryAddFile(m_fileName))
                    return ServiceResult.Success;
                else
                    return ServiceResult.Failure;
            }
            catch (SocketException)
            {
                return ServiceResult.NetworkError;
            }
            catch (Exception)
            {
                return ServiceResult.NetworkError;
            }
        }
    }
}
