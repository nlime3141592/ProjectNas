using MySqlConnector;
using System;
using System.Data.SqlClient;

namespace NAS
{
    public class SSvAcceptorViewUpdate : NasService
    {
        private AcceptedClient m_client;

        public SSvAcceptorViewUpdate(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            try
            {
                DBConnection db = NasServerProgram.GetDB();
                MySqlCommand sqlcmd;
                MySqlDataReader reader = null;

                db.TryGetSqlCommand(out sqlcmd, "SELECT COUNT(*) FROM account AS a, userinfo AS u WHERE a.uuid = u.uuid AND u.depid = 0");
                reader = sqlcmd.ExecuteReader();
                reader.Read();
                m_client.socModule.SendInt32(reader.GetInt32(0));
                reader.Close();

                db.TryGetSqlCommand(out sqlcmd, "SELECT a.uuid, u.name, a.id, a.regdate FROM account AS a, userinfo AS u WHERE a.uuid = u.uuid AND u.depid = 0");
                reader = sqlcmd.ExecuteReader();
                while(reader.Read())
                {
                    int uuid = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string id = reader.GetString(2);
                    DateTime regdate = reader.GetDateTime(3);
                    string regdateStr = string.Format("{0:D02}-{1:D02}-{2:D02} {3:D02}:{4:D02}:{5:D02}", regdate.Year, regdate.Month, regdate.Day, regdate.Hour, regdate.Minute, regdate.Second);

                    m_client.socModule.SendInt32(uuid);
                    m_client.socModule.SendString(name);
                    m_client.socModule.SendString(id);
                    m_client.socModule.SendString(regdateStr);
                }
                reader.Close();

                db.TryGetSqlCommand(out sqlcmd, "SELECT COUNT(*) FROM department WHERE depid > 0");
                reader = sqlcmd.ExecuteReader();
                reader.Read();
                m_client.socModule.SendInt32(reader.GetInt32(0));
                reader.Close();

                db.TryGetSqlCommand(out sqlcmd, "SELECT depid, depname FROM department WHERE depid > 0");
                reader = sqlcmd.ExecuteReader();
                while(reader.Read())
                {
                    int depid = reader.GetInt32(0);
                    string department = reader.GetString(1);

                    m_client.socModule.SendInt32(depid);
                    m_client.socModule.SendString(department);
                }
                reader.Close();

                return NasServiceResult.Success;
            }
            catch(Exception ex)
            {
                return NasServiceResult.NetworkError;
            }
        }
    }
}
