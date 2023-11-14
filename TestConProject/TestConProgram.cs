using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Tests
{
    internal class TestConProgram
    {
        private static void Main(string[] args)
        {
            string root = @"C:\NAS";

            FileSystem fs = new FileSystem(root);

            Console.WriteLine("---------------- 파일 시스템 출력 ----------------");
            foreach(string dir in fs.NextDirectories)
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