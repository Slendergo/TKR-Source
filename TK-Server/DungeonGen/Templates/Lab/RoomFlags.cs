using System;

namespace dungeonGen.templates.Lab
{
    [Flags]
    public enum RoomFlags
    {
        Evil = 1,
        ConnectionMask = 6,
        Conn_Floor = 0,
        Conn_None = 2,
        Conn_Destructible = 4
    }
}
