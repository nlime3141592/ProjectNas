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
                NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT uuid FROM account WHERE id = @id AND pw = @pw");
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Parameters.AddWithValue("@pw", pw);
                MySqlDataReader reader = sqlcmd.ExecuteReader();

                if(!reader.Read())
                {
                    // NOTE: 존재하지 않는 계정입니다.
                    m_client.socModule.SendInt32(-1);
                    m_client.socModule.SendString("<LOGIN_FAILURE>");
                    reader.Close();
                    return new NasServiceResult(20001, "INVALID_ACCOUNT");
                }
                else
                {
                    m_client.socModule.SendInt32(reader.GetInt32(0));
                    m_client.socModule.SendString("<LOGIN_SUCCESS>");
                    reader.Close();
                    return NasServiceResult.Success;
                }
            }
            catch(SocketException)
            {
                return NasServiceResult.NetworkError;
            }
            catch(Exception)
            {
                return NasServiceResult.Error;
            }
        }
    }
}