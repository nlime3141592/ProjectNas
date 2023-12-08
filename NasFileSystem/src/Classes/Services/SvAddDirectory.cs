using System;
using System.Net.Sockets;
using System.Text;

namespace NAS.FileSystem.Service
{
    public class SvAddDirectory : NasService
    {
        private string m_currentDirectory;
        private string m_folderName;
        private Encoding m_encoding;

        public SvAddDirectory(string _currentDirectory, string _folderName, Encoding _encoding)
        {
            m_currentDirectory = _currentDirectory;
            m_folderName = _folderName;
            m_encoding = _encoding;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                DirectoryManager manager = DirectoryManager.Get(m_currentDirectory, m_encoding);

                if(manager.TryAddFolder(m_folderName))
                    return NasServiceResult.Success;
                else
                    return NasServiceResult.Failure;
            }
            catch(SocketException)
            {
                return NasServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
