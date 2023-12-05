using NAS.DB;
using System;

namespace NAS.Server
{
    internal static class MainNasServer
    {
        private const int c_PORT = 25565;

        private static DBConnection s_m_db;
        private static NasServer s_m_server;

        public static DBConnection GetDB() => s_m_db;
        public static NasServer GetServer() => s_m_server;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        private static void Main(string[] args)
        {
            s_m_db = new DBConnection();

            if (!s_m_db.TryOpen())
            {
                s_m_db = null;
            }
            else
            {
                s_m_server = new NasServer();

                if (s_m_server.TryOpen(MainNasServer.c_PORT))
                {
                    s_m_server.WriteLog("서버 대기 중...");
                    s_m_server.Start();
                    s_m_server.WriteLog("서버가 열렸습니다.");

                    while (true)
                    {
                        string command = Console.ReadLine().ToLower();

                        if (command.Equals("stop") || command.Equals("exit") || command.Equals("quit"))
                            break;

                        switch (command)
                        {
                            case "count":
                                s_m_server.WriteLog("현재 접속한 클라이언트 수: {0}", s_m_server.currentClientCount);
                                break;
                            default:
                                s_m_server.WriteLog("잘못된 명령어입니다.");
                                break;
                        }
                    }

                    s_m_server.WriteLog("서버 종료 중...");
                    s_m_server.Halt();
                }
            }

            s_m_server.WriteLog("서버가 종료되었습니다.");
        }
    }
}
