using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public class CSvDirectoryMove : NasService
    {
        public Action onMoveSuccess;
        public Action onError;

        private NasClient m_client;
        private string m_dirNext;

        public CSvDirectoryMove(NasClient _client, string _dirNext)
        {
            m_client = _client;
            m_dirNext = _dirNext.Trim();
        }

        public override NasServiceResult Execute()
        {
            try
            {
                m_client.socModule.SendString("SV_DIRECTORY_MOVE");
                m_client.socModule.SendString(m_client.datFileBrowse.fakedir);
                m_client.socModule.SendString(m_dirNext);
                m_client.socModule.SendInt32(m_client.datLogin.department);
                m_client.socModule.SendInt32(m_client.datLogin.level);
                m_client.datFileBrowse.fakedir = m_client.socModule.ReceiveString();

                int dcnt = m_client.socModule.ReceiveInt32();
                m_client.datFileBrowse.directories.Clear();
                for(int i = 0; i < dcnt; ++i)
                {
                    int didx = m_client.socModule.ReceiveInt32();
                    string dname = m_client.socModule.ReceiveString();

                    if(didx != -1)
                        m_client.datFileBrowse.directories.TryAdd(didx, dname);
                }

                int fcnt = m_client.socModule.ReceiveInt32();
                m_client.datFileBrowse.files.Clear();
                for(int i = 0; i < fcnt; ++i)
                {
                    int fidx = m_client.socModule.ReceiveInt32();
                    string fname = m_client.socModule.ReceiveString();

                    if(fidx != -1)
                        m_client.datFileBrowse.files.TryAdd(fidx, fname);
                }

                onMoveSuccess?.Invoke();
                return NasServiceResult.Success;
            }
            catch(Exception ex)
            {
                this.WriteLog(ex.StackTrace);
                onError?.Invoke();
                return NasServiceResult.NetworkError;
            }
        }
    }
}
