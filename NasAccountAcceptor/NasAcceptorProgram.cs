using System;
using System.Windows.Forms;

namespace NAS
{
    internal class NasAcceptorProgram
    {
        public const string c_HOST = "127.0.0.1";
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
        private static NasAcceptorProgram s_m_program; // TEST: Log & Debug을 위한 객체

        private NasAcceptorProgram() { }

        // NOTE: NasAccountAcceptor의 기본 클래스를 Singleton으로 관리합니다.
        public static NasAcceptor GetAcceptor()
        {
            if(s_m_acceptor == null)
            {
                s_m_acceptor = new NasAcceptor();
                s_m_acceptor.onHaltedByException = s_m_OnHaltedByException;
            }

            return s_m_acceptor;
        }

        // NOTE: 서버와 연결을 시도합니다.
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
        }
    }
}
