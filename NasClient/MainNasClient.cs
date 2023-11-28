using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net;
using System.Text;

namespace NAS.Client
{
    internal static class NetworkManager
    {
        private const string c_HOST_IP = "127.0.0.1";
        // private const string c_HOST_IP = "192.168.35.31";
        private const int c_HOST_PORT = 25565;

        private static Socket s_m_socket;
        private static byte[] s_m_buffer;
        private static Encoding s_m_encoding;

        static NetworkManager()
        {
            s_m_buffer = new byte[0xA00000]; // NOTE: File chunk size.
            s_m_encoding = Encoding.UTF8;
        }

        public static bool TryConnectToServer()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(c_HOST_IP), c_HOST_PORT);
                socket.Connect(endPoint);

                if (!socket.Connected)
                    return false;

                // NOTE: 클라이언트 타입 전송
                s_m_buffer[0] = 1;
                socket.Send(s_m_buffer, 0, 1, SocketFlags.None);

                // NOTE: 올바른 클라이언트 유형 값을 보냈는지 결과를 반환함.
                int length = socket.Receive(s_m_buffer);
                string response = s_m_encoding.GetString(s_m_buffer, 0, length);

                if (response.Equals("<ACCEPTED>"))
                {
                    s_m_socket = socket;
                    return true;
                }

                // NOTE: 서버에 연결할 수 없습니다.
                socket.Close();
                return false;
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
            // Application.Run(AuthForm.GetForm(AuthForm.FormMode.Waiting));
            Application.Run(FileBrowserForm.GetForm());
        }
    }
}
