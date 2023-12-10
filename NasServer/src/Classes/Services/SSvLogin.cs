using MySqlConnector;
using System;
using System.Net.Sockets;

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
            try
            {
                string id = m_client.socModule.ReceiveString();
                string pw = m_client.socModule.ReceiveString();

                MySqlCommand sqlcmd;
                NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT uuid, department, level FROM account WHERE id = @id AND pw = @pw");
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Parameters.AddWithValue("@pw", pw);
                MySqlDataReader reader = sqlcmd.ExecuteReader();

                if(!reader.Read())
                {
                    m_client.socModule.SendString("<INVALID_ACCOUNT>");
                    reader.Close();
                    return new NasServiceResult(20001, "INVALID_ACCOUNT");
                }
                else
                {
                    int uuid = reader.GetInt32(0);
                    int department = reader.GetInt32(1);
                    int level = reader.GetInt32(2);

                    if(department == 0)
                    {
                        m_client.socModule.SendString("<NOT_ACCEPTED_ACCOUNT>");
                        reader.Close();
                        return new NasServiceResult(20002, "NOT_ACCEPTED_ACCOUNT");
                    }
                    else
                    {
                        m_client.socModule.SendString("<LOGIN_SUCCESS>");
                        m_client.socModule.SendInt32(uuid);
                        m_client.socModule.SendInt32(department);
                        m_client.socModule.SendInt32(level);
                        m_client.socModule.SendString(m_client.fileSystem.rootFakeDirectory);
                        reader.Close();
                        return NasServiceResult.Success;
                    }
                }
            }
            catch(Exception ex)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}