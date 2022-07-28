using System;

namespace common.database
{
    [Flags]
    public enum GuildRank : byte
    {
        Initiate = 0,
        Member = 10,
        Officer = 20,
        Leader = 30,
        Founder = 40
    }
}
