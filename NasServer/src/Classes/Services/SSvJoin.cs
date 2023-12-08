using MySqlConnector;
using System;

namespace NAS
{
    public class SSvJoin : NasService
    {
        private AcceptedClient m_client;

        public SSvJoin(AcceptedClient _client)
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
                NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT COUNT(*) FROM account WHERE id = @id");
                sqlcmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = sqlcmd.ExecuteReader();

                if (!reader.Read() || reader.GetInt32(0) != 0)
                {
                    m_client.socModule.SendString("<JOIN_FAILURE>");
                    reader.Close();
                    return new NasServiceResult(20001, "INVALID_ACCOUNT");
                }
                reader.Close();

                NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "INSERT INTO account (id, pw) values (@id, @pw)");
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Parameters.AddWithValue("@pw", pw);
                sqlcmd.ExecuteNonQuery();
                m_client.socModule.SendString("<JOIN_SUCCESS>");
                return NasServiceResult.Success;
            }
            catch(Exception ex)
            {
                this.WriteLog("Exception occured. {0}", ex.Message);
                return NasServiceResult.NetworkError;
            }
        }
    }
}
