﻿using System;
using MySqlConnector;

namespace NAS
{
    public class DBConnection
    {
        // NOTE: 데이터베이스의 기본 정보를 나타냅니다.
        private const string c_DB_IP = "localhost";
        private const string c_DB_ID = "root";
        private const string c_DB_PW = "1234";
        private const string c_DB_NAME = "projnas";

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

        // NOTE: 데이터베이스에 질의를 시도합니다.
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
    }
}
