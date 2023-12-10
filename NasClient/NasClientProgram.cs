using System;
using System.Windows.Forms;

namespace NAS
{
    internal class NasClientProgram
    {
        public const string c_HOST = "127.0.0.1";
        public const int c_PORT = 25565;

        private static NasClientProgram instance
        {
            get
            {
                if(s_m_program == null)
                    s_m_program = new NasClientProgram();

                return s_m_program;
            }
        }

        private static NasClient s_m_client;
        private static NasClientProgram s_m_program;

        private NasClientProgram() { }

        public static NasClient GetClient()
        {
            if (s_m_client == null)
            {
                s_m_client = new NasClient();
                s_m_client.onHaltedByException = s_m_OnHaltedByException;
            }

            return s_m_client;
        }

        public static void ClearClient()
        {
            s_m_client = null;
        }

        public static bool TryConnectToServer()
        {
            s_m_client?.socModule?.Close();
            s_m_client = new NasClient();
            s_m_client.onHaltedByException = s_m_OnHaltedByException;

            if (s_m_client.TryConnect(c_HOST, c_PORT))
            {
                AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Login);
                return true;
            }
            else
            {
                AuthForm.GetForm().ctChangeFormMode(AuthForm.FormMode.Start);
                return false;
            }
        }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(AuthForm.GetForm());
            }
            catch(Exception ex)
            {
                NasClientProgram.instance.WriteLog("예외가 발생했담.... {0}", ex.Message);
                NasClientProgram.instance.WriteLog("StackTrace : {0}", ex.StackTrace);
                NasClientProgram.instance.WriteLog("Message : {0}", ex.Message);
            }
        }

        private static void s_m_OnHaltedByException()
        {
            NasClientProgram.ClearClient();
            AuthForm authForm = AuthForm.GetForm();
            NasClientProgram.instance.WriteLog("오 여기에 들어왔네?");
            authForm.Invoke(new Action(() =>
            {
                authForm.Show();
            }));
        }
    }
}
