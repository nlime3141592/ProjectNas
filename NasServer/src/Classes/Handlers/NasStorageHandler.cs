using NAS.Server.Service;

namespace NAS.Server.Handler
{
    public class NasStorageHandler : NasHandler
    {
        public NasStorageHandler(IMessenger _messenger, SocketModule _module)
        : base(_messenger, _module)
        {

        }

        protected override NasService HandleService(string _serviceType)
        {
            this.WriteLog("Receive service header. ({0})", _serviceType);

            switch (_serviceType)
            {
                case "sv_disconnect":
                    return new SvDisconnect(this);
                case "sv_init":
                    return new SvInitialize(this);
                default:
                    return null;
            }
        }

        protected override void OnHandlerEnd()
        {
            base.OnHandlerEnd();
        }
    }
}
