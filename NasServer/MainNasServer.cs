using System;
using System.IO;

namespace NAS
{
    internal static class MainNasServer
    {
        private const int c_PORT = 25565;
        
        // private static DBConnection s_m_db;
        private static NasServer s_m_server;

        // public static DBConnection GetDB() => s_m_db;
        public static NasServer GetServer() => s_m_server;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        private static void Main(string[] args)
        {
            /*
            s_m_db = new DBConnection();

            if (!s_m_db.TryOpen())
            {
                s_m_db = null;
            }
            else
            {
                
            }
            */
            try
            {
                if (args == null || args.Length != 2)
                {
                    s_m_server.WriteLog("Command argument error.");
                    return;
                }

                string rootStorageDirectory = s_m_GetAbsoluteDirectory(args[0]);
                int port = s_m_GetPortNumber(args[1]);

                NasServer server = new NasServer(port);

                if (!server.TryOpen(rootStorageDirectory))
                {
                    s_m_server.WriteLog("Server open failed.");
                    return;
                }

                s_m_server.WriteLog("Opened server.");
                s_m_ExecuteServerCommand(s_m_server);
            }
            catch (Exception)
            {

            }
        }

        private static string s_m_GetAbsoluteDirectory(string _directory)
        {
            string directory = _directory;

            if (!Path.IsPathRooted(directory))
                directory = Path.GetFullPath(directory);

            if (directory.LastIndexOf('\\') < directory.Length - 1)
                directory += '\\';

            return directory;
        }

        private static int s_m_GetPortNumber(string _stringPort)
        {
            int port;

            if (!int.TryParse(_stringPort, out port) || port <= 0 || port >= 65535)
                throw new ArgumentException("_stringPort parameter error.");
            else
                return port;
        }

        private static void s_m_ExecuteServerCommand(NasServer _server)
        {
            bool isStopped = false;

            do
            {
                string line = Console.ReadLine();

                switch (line)
                {
                    case "stop":
                    case "quit":
                        isStopped = _server.TryClose();
                        break;
                    case "ccnt":
                        _server.WriteLog("Client Count: {0}", _server.clientCount);
                        break;
                    default:
                        break;
                }
            }
            while (!isStopped);
        }
    }
}
