using System;

namespace common.database.status
{
    [Flags]
    public enum GuildMemberStatus
    {
        Ok,
        NameNotChosen,
        AlreadyIn,
        InAnother,
        AlreadyMember,
        Full
    }
}
