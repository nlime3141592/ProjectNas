using MySqlConnector;
using System;

namespace NAS
{
    public class SSvLogin : NasService
    {
        private AcceptedClient m_client;

        public SSvLogin(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            string id = m_client.socModule.ReceiveString();
            string pw = m_client.socModule.ReceiveString();

            MySqlCommand sqlcmd;
            MySqlDataReader reader = null;
            NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT u.uuid, u.name, u.depid, u.permission_level FROM account AS a, userinfo AS u WHERE a.uuid = u.uuid AND a.id = @id AND a.pw = @pw");
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.Parameters.AddWithValue("@pw", pw);
            reader = sqlcmd.ExecuteReader();

            if(!reader.Read())
            {
                reader.Close();
                m_client.socModule.SendString("<INVALID_ACCOUNT>");
                return new NasServiceResult(20001, "INVALID_ACCOUNT");
            }
            else
            {
                int uuid = reader.GetInt32(0);
                string name = reader.GetString(1);
                int department = reader.GetInt32(2);
                int level = reader.GetInt32(3);
                reader.Close();

                if(department == 0)
                {
                    m_client.socModule.SendString("<NOT_ACCEPTED_ACCOUNT>");
                    return new NasServiceResult(20002, "NOT_ACCEPTED_ACCOUNT");
                }
                else
                {
                    // NOTE: 로그인 성공하여 사용자 정보를 클라이언트에 전송합니다.
                    m_client.socModule.SendString("<LOGIN_SUCCESS>");
                    m_client.socModule.SendInt32(uuid);
                    m_client.socModule.SendString(name);
                    m_client.socModule.SendInt32(department);
                    m_client.socModule.SendInt32(level);
                    m_client.socModule.SendString(m_client.fileSystem.rootFakeDirectory);
                    return NasServiceResult.Success;
                }
            }
        }
    }
}