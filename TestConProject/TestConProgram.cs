using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NAS.Tests
{
    internal class TestConProgram
    {
        private static void Main(string[] args)
        {
            Main_FileDirectoryTest();
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