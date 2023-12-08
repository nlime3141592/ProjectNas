using System;
using System.Windows.Forms;

namespace NAS
{
    internal static class MainNasClient
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            NasClient client = new NasClient();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (client.TryConnect("127.0.0.1", 25565))
                Application.Run(AuthForm.GetForm(AuthForm.FormMode.Login));
            else
                Application.Run(AuthForm.GetForm(AuthForm.FormMode.Start));
        }
    }
}
