using System;

namespace common.database.status
{
    [Flags]
    public enum GuildCreateStatus
    {
        Ok,
        InvalidName,
        UsedName
    }
}
