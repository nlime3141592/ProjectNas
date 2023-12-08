using System;

namespace NAS
{
    public abstract class AcceptedClient : NasThread
    {
        public const int c_CLIENT_TIMEOUT = 10;

        public SocketModule socModule { get; internal set; }

        protected sealed override void ThreadMain()
        {
            Console.WriteLine("[{0}] Join a {0} client.", this.GetType().Name);

            try
            {
                while (base.isStarted && !base.isStopped)
                {
                    string serviceHeader = socModule.ReceiveString(1000 * c_CLIENT_TIMEOUT);
                    NasService service = HandleServiceHeader(serviceHeader);
                    NasServiceResult result = service.Execute();

                    if (!TryHandleServiceResult(service, result))
                    {
                        socModule.Close();
                        base.TryHalt();
                        break;
                    }

                    /*
                    if (TryHandleServiceResult(service, result))
                        socModule.SendString(result.name); // NOTE: 최종 서비스 결과를 클라이언트에 response 해줌.
                    else
                    {
                        socModule.Close();
                        base.TryHalt();
                        break;
                    }
                    */
                }
            }
            catch (Exception)
            {
                // NOTE: Thread가 강제 종료되었음.

                // NOTE: Kill self.
                base.TryStop();
                this.socModule.Close();
            }

            Console.WriteLine("[{0}] Disconnect a {0} client.", this.GetType().Name);
        }

        protected abstract NasService HandleServiceHeader(string _serviceHeader);

        private bool TryHandleServiceResult(NasService _service, NasServiceResult _result)
        {
            if (_result == NasServiceResult.NetworkError)
                return false;

            return true;
        }
    }
}