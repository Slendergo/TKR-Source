using System;

namespace common.database.status
{
    [Flags]
    public enum AddCharacterStatus
    {
        Ok,
        ReachedLimit,
        SkinUnavailable,
        Locked,
        Error
    }
}
