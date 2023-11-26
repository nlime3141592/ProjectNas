using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net;

namespace NAS.Client
{
    internal static class NetworkManager
    {
        private const string c_HOST_IP = "192.168.35.31";
        private const int c_HOST_PORT = 25565;

        private static Socket s_m_socket;
        private static byte[] s_m_buffer = new byte[0xA00000]; // NOTE: File chunk size.

        public static bool TryConnectToServer()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(c_HOST_IP), c_HOST_PORT);
                socket.Connect(endPoint);

                if (!socket.Connected)
                {
                    return false;
                }

                s_m_socket = socket;
                return true;
            }
            catch (Exception _ex)
            {
                return false;
            }
        }
    }

    internal static class MainNasClient
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(AuthForm.GetForm(AuthForm.FormMode.Waiting));
        }
    }
}
