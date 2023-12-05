using System;
using MySqlConnector;

namespace NAS.DB
{
    public class DBConnection
    {
        private const string c_DB_IP = "localhost";
        private const string c_DB_ID = "root";
        private const string c_DB_PW = "1234";
        private const string c_DB_NAME = "testbase";

        private MySqlConnection m_connection;

        public bool TryOpen()
        {
            try
            {
                string uri = string.Format("Server={0};User ID={1};Password={2};Database={3}", c_DB_IP, c_DB_ID, c_DB_PW, c_DB_NAME);

                m_connection?.Close();
                m_connection = new MySqlConnection(uri);
                m_connection.Open();
                this.WriteLog("DB 접속에 성공했습니다.");
                return true;
            }
            catch(Exception)
            {
                this.WriteLog("DB에 접속할 수 없습니다.");
                return false;
            }
        }

        public bool TryClose()
        {
            try
            {
                m_connection.Close();
                m_connection = null;
                this.WriteLog("DB와의 연결이 끊어졌습니다.");
                return true;
            }
            catch(Exception)
            {
                this.WriteLog("DB를 닫을 수 없습니다.");
                return false;
            }
        }

        public bool TryGetSqlCommand(out MySqlCommand _command, string _sql)
        {
            try
            {
                _command = new MySqlCommand(_sql, m_connection);
                return true;
            }
            catch(Exception)
            {
                _command = null;
                return false;
            }
        }

        /*
        // NOTE: 예제 코드
        public void Test()
        {
            
            MySqlCommand command = new MySqlCommand("SELECT * FROM account", m_connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0));
                Console.WriteLine(reader.GetString(1));
                Console.WriteLine(reader.GetString(2));
                Console.WriteLine(reader.GetDateTime(3));
            }
        }
        */
    }
}
