﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace NAS
{
    public class NasAcceptor : NasThread
    {
        public const int c_CONNECTION_CHECK_INTERVAL = 10;

        public List<WaitingAccountData> wAccounts { get; private set; }
        public List<DepartmentData> departments { get; private set; }
        public SocketModule socModule { get; private set; }

        public Action onHaltedByException;

        private Stopwatch m_watch;
        private ConcurrentQueue<NasService> m_services;

        public NasAcceptor()
        {
            wAccounts = new List<WaitingAccountData>();
            departments = new List<DepartmentData>();

            m_watch = new Stopwatch();
            m_services = new ConcurrentQueue<NasService>();
        }

        protected override void ThreadMain()
        {
            try
            {
                m_watch.Reset();
                m_watch.Start();

                while (base.isStarted && !base.isStopped)
                {
                    // NOTE: 10초마다 서버와의 연결 체크를 위한 서비스를 전송한다.
                    if (m_watch.ElapsedMilliseconds > 1000 * c_CONNECTION_CHECK_INTERVAL)
                    {
                        this.Request(new ASvConnectionCheck(this));
                        m_watch.Restart();
                    }

                    if (m_services.Count <= 0)
                        continue;

                    NasService service;

                    if (!m_services.TryDequeue(out service))
                        continue;

                    NasServiceResult result = service.Execute();
                    m_watch.Restart();

                    if (result == NasServiceResult.NetworkError || result == NasServiceResult.Error)
                        break; // NOTE: 오류 발생하여 클라이언트 종료합니다.
                    else if (result == NasServiceResult.Loopback)
                        this.Request(service);
                }

                base.TryStop();
                m_watch.Stop();
            }
            catch(Exception ex)
            {
                base.TryStop();
                m_watch.Stop();
                this.WriteLog("서비스 실행 오류 발생, {0}", ex.Message);
            }
        }

        public bool TryConnect(string _host, int _port)
        {
            Socket socket = null;

            try
            {
                TcpClient tcpclnt = new TcpClient(AddressFamily.InterNetwork);
                tcpclnt.Connect(new IPEndPoint(IPAddress.Parse(_host), _port));

                SocketModule module = new SocketModule(tcpclnt, Encoding.UTF8);

                module.SendString("acptCLNT");
                string response = module.ReceiveString();

                if (!response.Equals("<ACCEPTED>"))
                    return false;
                else if (!base.TryStart())
                    return false;

                socModule = module;
                this.WriteLog("Connected to server.");
                return true;
            }
            catch (Exception)
            {
                // NOTE: 서버에 연결할 수 없습니다.
                socket?.Close();
                return false;
            }
        }

        public void Request(NasService _service)
        {
            if (isStarted && !isStopped)
                m_services.Enqueue(_service);
            else
            {
                try
                {
                    _service.Execute();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}