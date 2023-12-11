using System;

namespace NAS
{
    public abstract class AcceptedClient : NasThread
    {
        public const int c_CLIENT_TIMEOUT = 20;

        public SocketModule socModule { get; internal set; }
        public NasFileSystem fileSystem { get; internal set; }

        protected sealed override void ThreadMain()
        {
            Console.WriteLine("[{0}] Join a {0} client.", this.GetType().Name);

            try
            {
                while (base.isStarted && !base.isStopped)
                {
                    // NOTE: 먼저, 서비스 헤더를 수신하고, 그에 맞는 서비스 로직을 수행합니다.
                    string serviceHeader = socModule.ReceiveString(1000 * c_CLIENT_TIMEOUT);
                    NasService service = HandleServiceHeader(serviceHeader);
                    service.Execute();
                }

                // NOTE: 정상 종료
                base.TryStop();
                this.socModule.Close();
            }
            catch (Exception _exception)
            {
                // NOTE: Thread가 강제 종료되었음.
                this.WriteLog(_exception.Message);
                this.WriteLog(_exception.StackTrace);
                base.TryStop();
                // this.socModule.Close();
            }

            Console.WriteLine("[{0}] Disconnect a {0} client.", this.GetType().Name);
        }

        // NOTE:
        // 서비스 헤더를 수신하고, 헤더에 맞는 서비스 객체를 반환해야 합니다.
        // 서비스 Thread를 자식 클래스에서 다르게 구현하면, 다른 서비스를 제공할 수 있습니다.
        protected abstract NasService HandleServiceHeader(string _serviceHeader);
    }
}