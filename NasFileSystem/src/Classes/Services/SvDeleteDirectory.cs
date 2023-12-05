using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.FileSystem.Service
{
    class SvDeleteDirectory : NasService
    {
        private string m_currentDirectory;
        private string m_folderName;
        private Encoding m_encoding;

        public SvDeleteDirectory(string _currentDirectory, string _folderName, Encoding _encoding)
        {
            m_currentDirectory = _currentDirectory;
            m_folderName = _folderName;
            m_encoding = _encoding;
        }

        public override ServiceResult Execute()
        {
            try
            {
                DirectoryManager manager = DirectoryManager.Get(m_currentDirectory, m_encoding);

                if (manager.TryDeleteFolder(m_folderName))
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
