using common.resources;
using System;
using System.Collections.Generic;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class Grenade : Behavior
    {
        private readonly uint color;
        private readonly Cooldown coolDown;
        private readonly int damage;
        private readonly ConditionEffectIndex effect;
        private readonly int effectDuration;
        private readonly double? fixedAngle;
        private readonly float radius;
        private readonly double range;

        public Grenade(double radius, int damage, double range = 5, double? fixedAngle = null, Cooldown coolDown = new Cooldown(), ConditionEffectIndex effect = 0, int effectDuration = 0, uint color = 0xffff0000)
        {
            this.radius = (float)radius;
            this.damage = damage;
            this.range = range;
            this.fixedAngle = fixedAngle * Math.PI / 180;
            this.coolDown = coolDown.Normalize();
            this.effect = effect;
            this.effectDuration = effectDuration;
            this.color = color;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = 0;

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var pkts = new List<Packet>();
            var cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                var enemy = host as Enemy;
                var enemyClasified = enemy.Legendary ? 50 : enemy.Epic ? 25 : enemy.Rare ? 10 : 0;
                var player = host.AttackTarget ?? host.GetNearestEntity(range, true);

                if (player != null || fixedAngle != null)
                {
                    var target = fixedAngle != null ? new Position() { X = (float)(range * Math.Cos(fixedAngle.Value)) + host.X, Y = (float)(range * Math.Sin(fixedAngle.Value)) + host.Y }
                    : new Position() { X = player.X, Y = player.Y };

                    host.Owner.BroadcastIfVisible(new ShowEffect() { EffectType = EffectType.Throw, Color = new ARGB(color), TargetObjectId = host.Id, Pos1 = target, Pos2 = new Position() { X = 222 } }, host, PacketPriority.Low);
                    host.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                    {
                        world.BroadcastIfVisible(new Aoe()
                        {
                            Pos = target,
                            Radius = radius,
                            Damage = (ushort)(damage + enemyClasified),
                            Duration = 0,
                            Effect = 0,
                            OrigType = host.ObjectType,
                            Color = new ARGB(color)
                        }, host, PacketPriority.Low);
                        world.AOE(target, radius, true, p =>
                        {
                            (p as IPlayer).Damage(damage + enemyClasified, host);

                            if (!p.HasConditionEffect(ConditionEffects.Invincible) && !p.HasConditionEffect(ConditionEffects.Stasis))
                                p.ApplyConditionEffect(new ConditionEffect() { Effect = effect, DurationMS = effectDuration });
                        });
                    }));
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }
    }
}
