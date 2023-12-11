using MySqlConnector;

namespace NAS
{
    // NOTE: 회원가입 정보를 DB에 저장하는 서버 측 서비스입니다.
    public class SSvJoin : NasService
    {
        private AcceptedClient m_client;

        public SSvJoin(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            string id = m_client.socModule.ReceiveString();
            string pw = m_client.socModule.ReceiveString();
            string name = m_client.socModule.ReceiveString();

            MySqlCommand sqlcmd;
            MySqlDataReader reader = null;
            NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT COUNT(*) FROM account WHERE id = @id");
            sqlcmd.Parameters.AddWithValue("@id", id);
            reader = sqlcmd.ExecuteReader();

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
            NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT uuid FROM account WHERE id = @id");
            sqlcmd.Parameters.AddWithValue("@id", id);
            reader = sqlcmd.ExecuteReader();
            reader.Read();
            int uuid = reader.GetInt32(0);
            reader.Close();
            NasServerProgram.GetDB().TryGetSqlCommand(out sqlcmd, "INSERT INTO userinfo (uuid, name) VALUES (@uuid, @name)");
            sqlcmd.Parameters.AddWithValue("@uuid", uuid);
            sqlcmd.Parameters.AddWithValue("@name", name);
            sqlcmd.ExecuteNonQuery();
            m_client.socModule.SendString("<JOIN_SUCCESS>");
            return NasServiceResult.Success;
        }
    }
}
