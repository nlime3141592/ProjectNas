﻿using System;

namespace NAS
{
    public sealed class StandardClient : AcceptedClient
    {
        protected override NasService HandleServiceHeader(string _serviceHeader)
        {
            switch (_serviceHeader)
            {
                case "CONNECTION_CHECK":
                    return new SvConnectionCheck(this);
                case "SV_LOGIN":
                    return new SvLogin(this);
                default:
                    return null;
            }
        }
    }
}