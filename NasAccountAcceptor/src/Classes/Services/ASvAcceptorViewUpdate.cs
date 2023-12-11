using System;

namespace NAS
{
    // NOTE: 가입 승인 관리 페이지의 각종 UI를 갱신합니다.
    public class ASvAcceptorViewUpdate : NasService
    {
        public Action onSuccess;
        public Action onError;

        private NasAcceptor m_acceptor;

        public ASvAcceptorViewUpdate(NasAcceptor _acceptor)
        {
            m_acceptor = _acceptor;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_acceptor.socModule.SendString("SV_ACCEPTOR_VIEW_UPDATE");
                int count = 0;
                
                // NOTE: 계정 목록 갱신
                count = m_acceptor.socModule.ReceiveInt32();
                m_acceptor.wAccounts.Clear();
                for(int i = 0; i < count; ++i)
                {
                    int uuid = m_acceptor.socModule.ReceiveInt32();
                    string name = m_acceptor.socModule.ReceiveString();
                    string id = m_acceptor.socModule.ReceiveString();
                    string regdate = m_acceptor.socModule.ReceiveString();

                    WaitingAccountData data = new WaitingAccountData();
                    data.uuid = uuid;
                    data.name = name;
                    data.id = id;
                    data.regdate = regdate;
                    m_acceptor.wAccounts.Add(data);
                }

                // NOTE: 부서 목록 콤보 박스 갱신
                count = m_acceptor.socModule.ReceiveInt32();
                m_acceptor.departments.Clear();
                for (int i = 0; i < count; ++i)
                {
                    int depid = m_acceptor.socModule.ReceiveInt32();
                    string department = m_acceptor.socModule.ReceiveString();

                    DepartmentData ddat = new DepartmentData();
                    ddat.depid = depid;
                    ddat.departmentName = department;
                    m_acceptor.departments.Add(ddat);
                }

                onSuccess?.Invoke();
                return NasServiceResult.Success;
            }
            catch(Exception ex)
            {
                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
