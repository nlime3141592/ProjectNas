using NAS.Client;
using NAS.Client.Service;
using NAS.FileSystem.Service;
using System.Text;

namespace NAS.FileSystem
{
    public class MainFileSystem
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        private static int Main(string[] _args)
        {
            // NOTE: 서버와 통신할 수 있는 객체를 생성합니다.
            if (!ClientNetworkManager.TryConnectToServer("usrCLNT"))
            {
                NasClientThread.GetInstance().WriteLog("서버에 접속할 수 없습니다.");
                NasClientThread.GetInstance().Halt();
                return 1;
            }
            else
            {
                NasClientThread.GetInstance().Start();
                NasClientThread.GetInstance().WriteLog("서버에 접속했습니다.");
            }

            NasClientThread.GetInstance().RequestService(new SvInitialize("my first client"));
            NasClientThread.GetInstance().RequestService(new SvAddDirectory(@"C:\NAS\", "tft lol", Encoding.UTF8));
            NasClientThread.GetInstance().Halt();
            return 0;
        }
    }
}
