using NAS.FileSystem;
using NAS.Server;
using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace NAS.Tests
{
    internal class TestConProgram
    {
        private static void Main(string[] args)
        {
            // 함수 주석을 처리하고, 솔루션 빌드하고, 실행합니다.
            // Main_NasServerOpenTest(); // 서버 열기 테스트, 배치 파일에서 실행
            // Main_FileDirectoryTest(); // 파일 연결 스레드 테스트, VS에서 실행
            NAS.DB.DBConnection con = new NAS.DB.DBConnection();

            con.TryOpen();
            con.TryClose();
        }

        private static void Main_NasServerOpenTest()
        {
            NasServer server = new NasServer();

            if (server.TryOpen(25565))
            {
                Console.WriteLine("서버가 열렸습니다.");
                server.Start();

                while(true)
                {
                    Console.WriteLine("Q를 입력하면 서버를 종료합니다.");
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Q)
                        break;
                }

                server.Halt();
            }
            Console.WriteLine("서버 종료");
        }

        private static void Main_ClientAccess()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.35.31"), 25565);
                socket.Connect(endPoint);

                // NasClient client = new NasClient(socket);
            }
            catch(Exception ex)
            {
                Console.WriteLine("E c p i n  E L");
                Console.WriteLine(" x e t o  H L O");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void Main_FileDirectoryTest()
        {
            Console.WriteLine("프로그램 시작합니다.");
            NasFile dir = new NasFile(@"C:\NAS", "test.txt", 1);
            Thread.Sleep(1000);
            dir.Close();
            Thread.Sleep(5000);
            Console.WriteLine("프로그램 종료합니다.");
        }

        private static void Main_FileSystemTest()
        {
            string root = @"C:\NAS";

            FileSys fs = new FileSys(root);

            Console.WriteLine("---------------- 파일 시스템 출력 ----------------");
            foreach (string dir in fs.NextDirectories)
                Console.WriteLine(dir);
            foreach (string file in fs.NextFiles)
                Console.WriteLine(file);
            Console.WriteLine("--------------------------------------------------");

            fs.MoveNext(1);

            Console.WriteLine("---------------- 파일 시스템 출력 ----------------");
            foreach (string dir in fs.NextDirectories)
                Console.WriteLine(dir);
            foreach (string file in fs.NextFiles)
                Console.WriteLine(file);
            Console.WriteLine("--------------------------------------------------");
        }
    }
}