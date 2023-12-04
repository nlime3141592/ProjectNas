﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NAS.Client
{
    internal static class NetworkManager
    {
        private const string c_HOST_IP = "127.0.0.1";
        // private const string c_HOST_IP = "192.168.35.31";
        private const int c_HOST_PORT = 25565;

        public static SocketModule socModule { get; private set; }

        private static Encoding s_m_encoding;

        static NetworkManager()
        {
            s_m_encoding = Encoding.UTF8;
        }

        public static bool TryConnectToServer()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(c_HOST_IP), c_HOST_PORT);
                socket.Connect(endPoint);

                if (!socket.Connected)
                    return false;

                // NOTE: 클라이언트 타입 전송
                SocketModule socModule = new SocketModule(socket, s_m_encoding);
                socModule.SendString("stdCLNT");

                // NOTE: 올바른 클라이언트 유형 값을 보냈는지 결과를 반환함.
                string response = socModule.ReceiveString();

                if (response.Equals("<ACCEPTED>"))
                {
                    NetworkManager.socModule = socModule;
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

}