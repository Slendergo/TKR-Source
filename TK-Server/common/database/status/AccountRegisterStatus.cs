using System;

namespace common.database.status
{
    [Flags]
    public enum AccountRegisterStatus
    {
        Ok,
        EmailInUse
    }
}
