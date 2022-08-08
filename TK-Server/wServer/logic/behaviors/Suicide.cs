using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
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
