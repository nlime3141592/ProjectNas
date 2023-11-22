using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NAS.Tests
{
    public class NasFile
    {
        public const int c_CHUNK_SIZE = 0xA00000; // A00000 == 10MB
        public const int c_TASK_TIME_OUT = 5000; // 1000 == 1000ms == 1s

        public int downloaderCapacity { get; private set; } = 1;

        private string m_fileDirectoryString;
        private byte[] m_buffer;

        private Task m_task;
        private Stopwatch m_taskTimeoutWatch;
        private bool m_isTaskHalted = false;
        private bool m_isTaskTimeout = false;

        private NasConfigFileF m_fConfig; // NOTE: File의 메타데이터입니다.
        private Queue<Socket> m_dQueue;
        private NasFileClient[] m_dClients; // Download 클라언트
        private NasFileClient m_uClient; // Upload 클라이언트, null이면 쓰기 가능, null이 아니면 쓰기 불가능(이미 쓰기 작업을 할당받은 객체가 있음.)

        public NasFile(string _directory, string _fileName, int _downloaderCapacity)
        {
            if (!TryRegisterToManager(_directory, _fileName))
                throw new Exception("TODO: 어떤 Exception을 throw할지 결정해야 합니다.");

            m_buffer = new byte[c_CHUNK_SIZE];

            downloaderCapacity = Math.Max(1, _downloaderCapacity);

            m_task = new Task(this.HandleClients);
            m_taskTimeoutWatch = new Stopwatch();

            m_dQueue = new Queue<Socket>(downloaderCapacity);
            m_dClients = new NasFileClient[downloaderCapacity];
            m_uClient = new NasFileClient(this);

            m_task.Start();
        }

        public void EnqueueDownloader(Socket _downloaderSocket)
        {
            m_dQueue.Enqueue(_downloaderSocket);
        }

        public bool TrySetUploader(Socket _uploaderSocket)
        {
            if (m_uClient.clientSocket != null)
                return false;

            m_uClient.SetClientSocket(_uploaderSocket);
            return true;
        }

        public void Close()
        {
            m_isTaskHalted = true;
        }

        private void HandleClients()
        {
            m_taskTimeoutWatch.Start();
            Console.WriteLine("파일 서비스 시작");

            while (!m_isTaskHalted && !IsTaskTimeOut())
            {
                for (int i = 0; i < m_dClients.Length; ++i)
                    HandleDownloader(ref m_dClients[i]);

                foreach (Socket _dSocket in m_dQueue)
                    HandleQueuedDownloader(_dSocket);

                HandleUploader(ref m_uClient);
            }

            // TODO: 파일 디렉토리가 닫힐 때 후처리
            // 1. if(클라이언트가 남아있다면 && m_halted), then, Send(<EOF>, <HALTED>);
            // 2. if(클라이언트가 남아있다면 && IsTaskTimeOut()), then, Send(<EOF>, <SERVICE_TIME_OUT>);

            m_taskTimeoutWatch.Stop();
            TryUnregisterFromManager(m_fileDirectoryString);

            if (m_isTaskHalted)
                Console.WriteLine("파일 서비스가 강제 종료되었습니다.");
            else if (m_isTaskTimeout)
                Console.WriteLine("파일 서비스 시간이 만료되었습니다.");

            Console.WriteLine("파일 서비스 종료");
        }

        private void HandleDownloader(ref NasFileClient _dClient)
        {
            if (m_dQueue.Count > 0)
            {
                if (_dClient == null)
                    _dClient = new NasFileClient(this);

                if (_dClient.clientSocket == null)
                    _dClient.SetClientSocket(m_dQueue.Dequeue());
            }
            else if(_dClient == null || _dClient.clientSocket == null)
                return;

            // NOTE: Handle success.
            m_taskTimeoutWatch.Restart();
        }

        private void HandleUploader(ref NasFileClient _uClient)
        {
            if (_uClient == null || _uClient.clientSocket == null)
                return;

            // NOTE: Handle success.
            m_taskTimeoutWatch.Restart();
        }

        private void HandleQueuedDownloader(Socket _dSocket)
        {
            // NOTE: Handle success.
            m_taskTimeoutWatch.Restart();
        }

        private bool IsTaskTimeOut()
        {
            m_isTaskTimeout = m_taskTimeoutWatch.ElapsedMilliseconds >= c_TASK_TIME_OUT;
            return m_isTaskTimeout;
        }

        private bool TryRegisterToManager(string _directory, string _fileName)
        {
            Monitor.Enter(NasFileSystem.nasFiles);
            m_fileDirectoryString = NasFileSystem.GetFileString(_directory, _fileName);

            if (NasFileSystem.nasFiles.ContainsKey(m_fileDirectoryString))
            {
                Monitor.Exit(NasFileSystem.nasFiles);
                return false;
            }
            else
            {
                NasFileSystem.nasFiles.Add(m_fileDirectoryString, this);
                Monitor.Exit(NasFileSystem.nasFiles);
            }

            return true;
        }

        private bool TryUnregisterFromManager(string _directory)
        {
            Monitor.Enter(NasFileSystem.nasFiles);
            bool isRemoved = NasFileSystem.nasFiles.Remove(_directory);
            Monitor.Exit(NasFileSystem.nasFiles);

            return isRemoved;
        }
    }
}
