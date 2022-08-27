﻿using common.resources;
using System;
using System.Linq;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    [Obsolete("This algorithm require revision due lack of performance during its execution (too much cost for application)", true)]
    internal class ConditionEffectRegion : Behavior
    {
        private readonly int _duration;
        private readonly ConditionEffectIndex[] _effects;
        private readonly int _range;

        public ConditionEffectRegion(ConditionEffectIndex[] effects, int range = 2, int duration = -1)
        {
            _range = range;
            _effects = effects;
            _duration = duration;
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            if (host == null || host.World == null)
                return;

            if (host.HasConditionEffect(ConditionEffectIndex.Paused))
                return;

            var hx = (int)host.X;
            var hy = (int)host.Y;
            var players = host.World.Players.Values.Where(p => Math.Abs(hx - (int)p.X) < _range && Math.Abs(hy - (int)p.Y) < _range);

            foreach (var player in players)
                foreach (var effect in _effects)
                    player.ApplyConditionEffect(new ConditionEffect(effect, _duration));
        }
    }
}
