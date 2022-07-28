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
    internal class Roar : Behavior
    {
        private readonly float _radius;
        private readonly uint _color;
        private readonly Cooldown _cooldown;
        private readonly double _range;
        private readonly ConditionEffectIndex _effect;
        private readonly int _effectDuration;
        private readonly int _damage;

        public Roar(float radius, int damage, uint color, double range, ConditionEffectIndex effect, int effDuration, Cooldown cooldown)
        {
            _radius = radius;
            _color = color;
            _effect = effect;
            _effectDuration = effDuration;
            _range = range;
            _cooldown = cooldown;
            _damage = damage;

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
                var player = host.AttackTarget ?? host.GetNearestEntity(_range, true);

                if (player != null)
                {
                    var target = new Position() { X = enemy.X, Y = enemy.Y };

                    host.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                    {
                        world.BroadcastIfVisible(new Aoe()
                        {
                            Pos = target,
                            Radius = _radius,
                            Damage = (ushort)(_damage + enemyClasified),
                            Duration = 0,
                            Effect = _effect,
                            OrigType = host.ObjectType,
                            Color = new ARGB(_color)
                        }, host, PacketPriority.Low);
                        world.AOE(target, _radius, true, p =>
                        {
                            (p as IPlayer).Damage(_damage + enemyClasified, host);

                            if (!p.HasConditionEffect(ConditionEffects.Invincible) && !p.HasConditionEffect(ConditionEffects.Stasis))
                                p.ApplyConditionEffect(new ConditionEffect() { Effect = _effect, DurationMS = _effectDuration });
                        });
                    }));
                }
                cool = _cooldown.Next(Random);
            }
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }
    }
}
