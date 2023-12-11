using System;

namespace NAS
{
    // NOTE: 로그인을 수행하는 서비스 객체입니다.
    public class CSvLogin : NasService
    {
        public Action onLoginSuccess;
        public Action onInvalidAccount;
        public Action onNotAcceptedAccount;
        public Action onError;

        private NasClient m_client;
        private string m_id;
        private string m_pw;

        public CSvLogin(NasClient _client, string _id, string _pw)
        {
            m_client = _client;
            m_id = _id;
            m_pw = _pw;
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_LOGIN");
            m_client.socModule.SendString(m_id);
            m_client.socModule.SendString(m_pw);
            string response = m_client.socModule.ReceiveString();

            switch(response)
            {
                // NOTE:
                // 로그인에 성공하면 서버로부터 계정에 대한 각종 정보를 수신합니다.
                // 자세한 사항은 NasClient 프로젝트의 LoginData.cs 및 FileBrowseData.cs 파일을 참조하세요.
                case "<LOGIN_SUCCESS>":
                    int uuid = m_client.socModule.ReceiveInt32();
                    string name = m_client.socModule.ReceiveString();
                    int department = m_client.socModule.ReceiveInt32();
                    int level = m_client.socModule.ReceiveInt32();
                    string fakedir = m_client.socModule.ReceiveString();
                    m_client.datLogin.uuid = uuid;
                    m_client.datLogin.name = name;
                    m_client.datLogin.department = department;
                    m_client.datLogin.level = level;
                    m_client.datFileBrowse.fakedir = fakedir;
                    m_client.datFileBrowse.fakeroot = fakedir;
                    onLoginSuccess?.Invoke();
                    return NasServiceResult.Success;
                case "<INVALID_ACCOUNT>":
                    onInvalidAccount?.Invoke();
                    return NasServiceResult.Failure;
                case "<NOT_ACCEPTED_ACCOUNT>":
                    onNotAcceptedAccount?.Invoke();
                    return NasServiceResult.Failure;
                default:
                    return NasServiceResult.Failure;
            }
        }
    }
}
