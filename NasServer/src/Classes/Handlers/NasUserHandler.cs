using NAS.Server.Service;

namespace NAS.Server.Handler
{
    public class NasUserHandler : NasHandler
    {
        public NasUserHandler(IMessenger _messenger, SocketModule _module)
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
                case "sv_login":
                    return new SvLogin();
                case "sv_test01":
                    return new SvTest01();
                default:
                    return null;
            }
        }
    }
}
