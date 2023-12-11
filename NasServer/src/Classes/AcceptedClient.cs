using System;

namespace NAS
{
    public abstract class AcceptedClient : NasThread
    {
        public const int c_CLIENT_TIMEOUT = 10;

        public SocketModule socModule { get; internal set; }
        public NasFileSystem fileSystem { get; internal set; }

        protected sealed override void ThreadMain()
        {
            Console.WriteLine("[{0}] Join a {0} client.", this.GetType().Name);

            try
            {
                while (base.isStarted && !base.isStopped)
                {
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
                this.socModule.Close();
            }

            Console.WriteLine("[{0}] Disconnect a {0} client.", this.GetType().Name);
        }

        protected abstract NasService HandleServiceHeader(string _serviceHeader);
    }
}