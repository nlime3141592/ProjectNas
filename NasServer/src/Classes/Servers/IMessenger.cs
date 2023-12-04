using NAS.Server.Handler;

namespace NAS.Server
{
    public interface IMessenger
    {
        void RequestService(string _userName, NasService _service);
    }
}
