﻿using MySqlConnector;
using System;

namespace NAS
{
    public class SSvJoinAccept : NasService
    {
        private AcceptedClient m_client;

        public SSvJoinAccept(AcceptedClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            int uuid = m_client.socModule.ReceiveInt32();
            string departmentName = m_client.socModule.ReceiveString();
            int level = m_client.socModule.ReceiveInt32();

            DBConnection db = NasServerProgram.GetDB();
            MySqlCommand sqlcmd;
            MySqlDataReader reader = null;

            db.TryGetSqlCommand(out sqlcmd, "SELECT depid FROM department WHERE depname = @depname");
            sqlcmd.Parameters.AddWithValue("@depname", departmentName);
            reader = sqlcmd.ExecuteReader();

            if(!reader.Read())
            {
                reader.Close();
                m_client.socModule.SendString("<ACCEPT_FAILURE>");
                return NasServiceResult.Failure;
            }

            int depid = reader.GetInt32(0);
            reader.Close();

            // NOTE: 회원 가입을 승인하고 권한 정보를 DB에 저장합니다.
            db.TryGetSqlCommand(out sqlcmd, "UPDATE userinfo SET depid = @depid, permission_level = @level WHERE uuid = @uuid");
            sqlcmd.Parameters.AddWithValue("@depid", depid);
            sqlcmd.Parameters.AddWithValue("@level", level);
            sqlcmd.Parameters.AddWithValue("@uuid", uuid);
            int affected0 = sqlcmd.ExecuteNonQuery();

            if (affected0 > 0)
            {
                m_client.socModule.SendString("<ACCEPT_SUCCESS>");
                return NasServiceResult.Success;
            }
            else
            {
                m_client.socModule.SendString("<ACCEPT_FAILURE>");
                return NasServiceResult.Success;
            }
        }
    }
}
