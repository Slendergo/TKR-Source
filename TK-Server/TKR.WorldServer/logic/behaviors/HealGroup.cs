﻿using System.Linq;
using TKR.Shared.resources;
using TKR.WorldServer.core.net.datas;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors
{
    internal class HealGroup : Behavior
    {
        private readonly string group;
        private readonly double range;
        private int? amount;
        private Cooldown coolDown;

        public HealGroup(double range, string group, Cooldown coolDown = new Cooldown(), int? healAmount = null)
        {
            this.range = (float)range;
            this.group = group;
            this.coolDown = coolDown.Normalize();

            amount = healAmount;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => state = 0;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return;

                foreach (var entity in host.GetNearestEntitiesByGroup(range, group).OfType<Enemy>())
                {
                    var newHp = entity.MaximumHP;

                    if (amount != null)
                    {
                        var newHealth = (int)amount + entity.HP;

                        if (newHp > newHealth)
                            newHp = newHealth;
                    }

                    if (newHp != entity.HP)
                    {
                        var n = newHp - entity.HP;

                        entity.HP = newHp;
                        entity.World.BroadcastIfVisible(new ShowEffect() { EffectType = EffectType.Potion, TargetObjectId = entity.Id, Color = new ARGB(0xffffffff) }, entity);
                        entity.World.BroadcastIfVisible(new ShowEffect()
                        {
                            EffectType = EffectType.Trail,
                            TargetObjectId = host.Id,
                            Pos1 = new Position() { X = entity.X, Y = entity.Y },
                            Color = new ARGB(0xffffffff)
                        }, host);
                        entity.World.BroadcastIfVisible(new Notification() { ObjectId = entity.Id, Message = "+" + n, Color = new ARGB(0xff00ff00) }, entity);
                    }
                }

                cool = coolDown.Next(Random);
            }
            else
                cool -= time.ElapsedMsDelta;

            state = cool;
        }
    }
}
