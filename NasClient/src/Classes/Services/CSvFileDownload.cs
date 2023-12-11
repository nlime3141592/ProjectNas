using System;
using System.IO;

namespace NAS
{
    public class CSvFileDownload : NasService
    {
        private const int c_BUFFER_SIZE = 1024 * 64;

        public Action<string> onDownloadSuccess;
        public Action<string, int> onDownloadLoopback;
        public Action onError;

        private NasClient m_client;
        private string m_absDownloadDirectory;
        private string m_storageDirectory;
        private string m_storageFileName;
        private string m_saveFileName;

        private int m_loopTimes;
        private FileStream m_fileStream;

        public CSvFileDownload(NasClient _client, string _absDownloadDirectory, string _storageFileName)
        {
            m_client = _client;
            m_absDownloadDirectory = _absDownloadDirectory;
            m_storageDirectory = m_client.datFileBrowse.fakedir;
            m_storageFileName = _storageFileName.Trim();
            m_saveFileName = m_GetAvailableFileName(_absDownloadDirectory, m_storageFileName);

            m_loopTimes = 0;
            m_fileStream = new FileStream(_absDownloadDirectory + m_saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_FILE_DOWNLOAD");
            m_client.socModule.SendString(m_storageDirectory);
            m_client.socModule.SendString(m_storageFileName);
            m_client.socModule.SendInt32(m_loopTimes);

            while(true)
            {
                string serviceType = m_client.socModule.ReceiveString();
                // this.WriteLog("Service Type: {0}, lp = {1}", serviceType, m_loopTimes);

                switch(serviceType)
                {
                    case "<READ>":
                        byte[] buffer;
                        m_client.socModule.TryReceiveVariableData(out buffer, 1000);
                        m_fileStream.Position = m_loopTimes * c_BUFFER_SIZE;
                        m_fileStream.Write(buffer, 0, buffer.Length);
                        onDownloadLoopback?.Invoke(m_absDownloadDirectory + m_saveFileName, ++m_loopTimes);
                        return NasServiceResult.Loopback;
                    case "<EOF>":
                        m_fileStream.Close();
                        onDownloadSuccess?.Invoke(m_absDownloadDirectory + m_saveFileName);
                        return NasServiceResult.Success;
                    default:
                        return NasServiceResult.Error;
                }
            }
        }

        private string m_GetAvailableFileName(string _absDownloadDirectory, string _fileName)
        {
            string path = _absDownloadDirectory + _fileName;

            if (!File.Exists(path))
                return _fileName;

            string nameOnly = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            int counter = 0;

            while (true)
            {
                string newName = string.Format("{0} ({1}){2}", nameOnly, ++counter, ext);
                path = _absDownloadDirectory + newName;

                if (!File.Exists(path))
                    return newName;
            }
        }
    }
}
