using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NAS.Tests
{
    internal class TestConProgram
    {
        private static void Main(string[] args)
        {
            Main_ClientAccess();
        }

        private static void Main_ClientAccess()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.35.31"), 25565);
                socket.Connect(endPoint);

                NasClient client = new NasClient(socket);
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

            FileSystem fs = new FileSystem(root);

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