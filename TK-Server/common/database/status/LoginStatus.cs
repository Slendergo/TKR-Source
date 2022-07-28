using System;

namespace common.database.status
{
    [Flags]
    public enum LoginStatus
    {
        Ok,
        AccountNotExists,
        InvalidCredentials
    }
}
