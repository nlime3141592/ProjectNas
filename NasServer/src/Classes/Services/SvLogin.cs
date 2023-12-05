using NAS.DB;
using MySqlConnector;
using System;
using System.Net.Sockets;

namespace NAS.Server.Service
{
    public class SvLogin : NasService, ISocketModuleService
    {
        private SocketModule m_socModule;

        void ISocketModuleService.Bind(SocketModule _module)
        {
            m_socModule = _module;
        }

        public override ServiceResult Execute()
        {
            try
            {
                string id = m_socModule.ReceiveString();
                string pw = m_socModule.ReceiveString();

                MySqlCommand sqlcmd;
                MainNasServer.GetDB().TryGetSqlCommand(out sqlcmd, "SELECT uuid FROM account WHERE id = @id AND pw = @pw");
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Parameters.AddWithValue("@pw", pw);
                MySqlDataReader reader = sqlcmd.ExecuteReader();

                if(!reader.Read())
                {
                    // NOTE: 존재하지 않는 계정입니다.
                    m_socModule.SendInt32(-1);
                    m_socModule.SendString("<LOGIN_FAILURE>");
                    reader.Close();
                    return new ServiceResult(20001, "INVALID_ACCOUNT");
                }
                else
                {
                    m_socModule.SendInt32(reader.GetInt32(0));
                    m_socModule.SendString("<LOGIN_SUCCESS>");
                    reader.Close();
                    return ServiceResult.Success;
                }
            }
            catch(SocketException _socEx)
            {
                return ServiceResult.NetworkError;
            }
            catch(Exception _ex)
            {
                return ServiceResult.Error;
            }
        }
    }
}
