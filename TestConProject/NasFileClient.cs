using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Tests
{
    public class NasFileClient
    {
        public Socket clientSocket => m_clientSocket;

        private Socket m_clientSocket;
        private NasFile m_fileDirectory;

        public NasFileClient(NasFile _fileDirectory)
        {
            m_fileDirectory = _fileDirectory;
        }

        public void SetClientSocket(Socket _clientSocket)
        {
            m_clientSocket = _clientSocket;
        }
    }
}
