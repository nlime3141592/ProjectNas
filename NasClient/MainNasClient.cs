using System;
using System.Windows.Forms;

namespace NAS.Client
{
    internal static class MainNasClient
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Console.WriteLine("Sfsfsd");
            NasClientThread.GetInstance().Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(AuthForm.GetForm(AuthForm.FormMode.Waiting));

            while (Console.ReadKey(true).Key != ConsoleKey.Q);
            NasClientThread.GetInstance().Halt();
        }
    }
}
