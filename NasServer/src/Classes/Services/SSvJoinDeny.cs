using MySqlConnector;
using System;

namespace NAS
{
    public class SSvJoinDeny : NasService
    {
        private AcceptedClient m_client;

        public SSvJoinDeny(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            int uuid = m_client.socModule.ReceiveInt32();

            DBConnection db = NasServerProgram.GetDB();
            MySqlCommand sqlcmd;

            db.TryGetSqlCommand(out sqlcmd, "DELETE FROM userinfo where uuid = @uuid");
            sqlcmd.Parameters.AddWithValue("@uuid", uuid);
            int affected0 = sqlcmd.ExecuteNonQuery();

            db.TryGetSqlCommand(out sqlcmd, "DELETE FROM account where uuid = @uuid");
            sqlcmd.Parameters.AddWithValue("@uuid", uuid);
            int affected1 = sqlcmd.ExecuteNonQuery();

            if (affected0 > 0 && affected1 > 0)
            {
                m_client.socModule.SendString("<DENY_SUCCESS>");
                return NasServiceResult.Success;
            }
            else
            {
                m_client.socModule.SendString("<DENY_FAILURE>");
                return NasServiceResult.Failure;
            }
        }
    }
}
