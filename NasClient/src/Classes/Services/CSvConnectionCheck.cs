﻿using System;

namespace NAS
{
    internal class CSvConnectionCheck : NasService
    {
        private NasClient m_client;

        public CSvConnectionCheck(NasClient _client)
        {
            m_client = _client;
        }

        public override NasServiceResult Execute()
        {
            m_client.socModule.SendString("SV_CONNECTION_CHECK");
            return NasServiceResult.Success;
        }
    }
}
