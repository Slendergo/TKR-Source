﻿using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.transitions
{
    internal class NoEntityWithinTransition : Transition
    {
        //State storage: none

        private readonly int _dist;

        public NoEntityWithinTransition(int dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            return !host.AnyEnemyNearby(_dist) && !host.AnyPlayerNearby(_dist);
        }
    }
}
