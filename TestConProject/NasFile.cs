using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
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

        private string m_directory;
        private string m_fileName;
        private byte[] m_buffer;

        private Task m_task;
        private Stopwatch m_taskTimeoutWatch;
        private bool m_isTaskHalted = false;
        private bool m_isTaskTimeout = false;

        private NasConfigFileF m_fConfig; // NOTE: File의 메타데이터입니다.
        private ConcurrentQueue<SocketModule> m_dQueue;
        private NasFileClient[] m_dClients; // Download 클라언트
        private NasFileClient m_uClient; // Upload 클라이언트, null이면 쓰기 가능, null이 아니면 쓰기 불가능(이미 쓰기 작업을 할당받은 객체가 있음.)

        // TODO:
        // 서버로부터 IMessenger 인터페이스를 얻어서 메시지를 사용자에게 보낼 수 있어야 한다.
        // NasFileClient는 서비스를 생성해서 IMessenger를 이용해 사용자에게 전달하는 구조를 생각해본다.
        // 모든 m_dClients[]의 서비스 상태를 판단하여 쓰기 작업도 수행할 수 있어야 한다.
        public NasFile(string _directory, string _fileName, int _downloaderCapacity)
        {
            if (!TryRegisterToManager())
                throw new Exception("TODO: 어떤 Exception을 throw할지 결정해야 합니다.");

            m_buffer = new byte[c_CHUNK_SIZE];

            downloaderCapacity = Math.Max(1, _downloaderCapacity);

            m_task = new Task(this.HandleClients);
            m_taskTimeoutWatch = new Stopwatch();

            m_dQueue = new ConcurrentQueue<SocketModule>();
            m_dClients = new NasFileClient[downloaderCapacity];
            m_uClient = new NasFileClient(this);

            m_task.Start();
        }

        public void EnqueueDownloader(SocketModule _downloaderSocket)
        {
            m_dQueue.Enqueue(_downloaderSocket);
        }

        public bool TrySetUploader(SocketModule _uploaderSocket)
        {
            if (m_uClient.clientSocket != null)
                return false;

            m_uClient.SetClientSocket(_uploaderSocket);
            return true;
        }

        public int ReadFile(byte[] _buffer, int _chunkNumber, int _fpOffset)
        {
            FileStream stream = new FileStream("d", FileMode.Open, FileAccess.Read);
            return 0;
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

                foreach (SocketModule _dSocket in m_dQueue)
                    HandleQueuedDownloader(_dSocket);

                HandleUploader(ref m_uClient);
            }

            // TODO: 파일 디렉토리가 닫힐 때 후처리
            // 1. if(클라이언트가 남아있다면 && m_halted), then, Send(<EOF>, <HALTED>);
            // 2. if(클라이언트가 남아있다면 && IsTaskTimeOut()), then, Send(<EOF>, <SERVICE_TIME_OUT>);

            m_taskTimeoutWatch.Stop();
            TryUnregisterFromManager();

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
                {
                    SocketModule module;

                    if(m_dQueue.TryDequeue(out module))
                        _dClient.SetClientSocket(module);
                }
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

        private void HandleQueuedDownloader(SocketModule _dSocket)
        {
            // NOTE: Handle success.
            m_taskTimeoutWatch.Restart();
        }

        private bool IsTaskTimeOut()
        {
            m_isTaskTimeout = m_taskTimeoutWatch.ElapsedMilliseconds >= c_TASK_TIME_OUT;
            return m_isTaskTimeout;
        }

        private bool TryRegisterToManager()
        {
            Monitor.Enter(NasFileSystem.nasFiles);
            string dirFile = string.Format(@"{0}{1}\", m_directory, m_fileName);

            // if (NasFileSystem.nasFiles.ContainsKey(dirFile))
            return NasFileSystem.nasFiles.TryAdd(dirFile, this);
            // return true;
        }

        private bool TryUnregisterFromManager()
        {
            // bool isRemoved = NasFileSystem.nasFiles.Remove(_directory);
            string dirFile = string.Format(@"{0}{1}\", m_directory, m_fileName);
            NasFile file;
            return NasFileSystem.nasFiles.TryRemove(dirFile, out file);
            // return isRemoved;
        }
    }
}
