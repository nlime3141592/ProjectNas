using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS
{
    internal class NasAcceptorProgram
    {
        public const string c_HOST = "192.168.35.31";
        public const int c_PORT = 25565;

        private static NasAcceptorProgram instance
        {
            get
            {
                if (s_m_program == null)
                    s_m_program = new NasAcceptorProgram();

                return s_m_program;
            }
        }

        private static NasAcceptor s_m_acceptor;
        private static NasAcceptorProgram s_m_program;

        private NasAcceptorProgram() { }

        public static NasAcceptor GetAcceptor()
        {
            if(s_m_acceptor == null)
            {
                s_m_acceptor = new NasAcceptor();
                s_m_acceptor.onHaltedByException = s_m_OnHaltedByException;
            }

            return s_m_acceptor;
        }

        public static bool TryConnectToServer()
        {
            s_m_acceptor?.socModule?.Close();
            s_m_acceptor = new NasAcceptor();
            s_m_acceptor.onHaltedByException = s_m_OnHaltedByException;

            if (s_m_acceptor.TryConnect(c_HOST, c_PORT))
                return true;
            else
                return false;
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
                Application.Run(new AcceptorForm());
            }
            catch(Exception ex)
            {
                s_m_acceptor?.TryHalt();
                NasAcceptorProgram.instance.WriteLog("Message : {0}", ex.Message);
                NasAcceptorProgram.instance.WriteLog("StackTrace : {0}", ex.StackTrace);
            }
        }

        private static void s_m_OnHaltedByException()
        {
            s_m_acceptor?.TryHalt();
            NasAcceptorProgram.instance.WriteLog("오 여기에 들어왔네?");
            // AuthForm authForm = AuthForm.GetForm();
            // authForm.Invoke(new Action(() => { authForm.Show(); }));
        }
    }
}
