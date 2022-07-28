using System;

namespace common.database
{
    [Flags]
    public enum GuildCreateStatus
    {
        Ok,
        InvalidName,
        UsedName
    }
}
