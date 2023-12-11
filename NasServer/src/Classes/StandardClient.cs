using System;

namespace NAS
{
    public sealed class StandardClient : AcceptedClient
    {
        protected override NasService HandleServiceHeader(string _serviceHeader)
        {
            switch (_serviceHeader)
            {
                case "SV_CONNECTION_CHECK":
                    return new SSvConnectionCheck(this);
                case "SV_LOGIN":
                    return new SSvLogin(this);
                case "SV_JOIN":
                    return new SSvJoin(this);
                case "SV_DIRECTORY_MOVE":
                    return new SSvDirectoryMove(this);
                case "SV_DIRECTORY_ADD":
                    return new SSvDirectoryAdd(this);
                case "SV_DIRECTORY_DELETE":
                    return new SSvDirectoryDelete(this);
                case "SV_FILE_ADD":
                    return new SSvFileAdd(this);
                case "SV_FILE_DELETE":
                    return new SSvFileDelete(this);
                case "SV_FILE_DOWNLOAD":
                    return new SSvFileDownload(this);
                default:
                    return null;
            }
        }
    }
}