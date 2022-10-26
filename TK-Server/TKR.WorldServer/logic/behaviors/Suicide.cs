using System;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
{
    internal class Suicide : Behavior
    {
        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            if (!(host is Enemy))
                throw new NotSupportedException("Use Decay instead");

            (host as Enemy).Death(ref time);
        }
    }
}
