using common.resources;
using System;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class TossObject2 : Behavior
    {
        private readonly ushort child;
        private readonly int coolDownOffset;
        private readonly bool randomToss;
        private readonly double range;
        private double? angle;
        private Cooldown coolDown;

        public TossObject2(string child, double range = 5, double? angle = null, Cooldown coolDown = new Cooldown(), int coolDownOffset = 0, bool randomToss = false)
        {
            this.child = BehaviorDb.InitGameData.IdToObjectType[child];
            this.range = range;
            this.angle = angle * Math.PI / 180;
            this.coolDown = coolDown.Normalize();
            this.coolDownOffset = coolDownOffset;
            this.randomToss = randomToss;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = coolDownOffset;

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                var tossAngle = randomToss ? Random.Next(0, 360) * Math.PI / 180 : angle;

                Entity en = null;

                if (tossAngle == null)
                    en = host.GetNearestEntity(range, null);

                if (tossAngle == null && en == null) return;

                var target = tossAngle == null ? new Position { X = en.X, Y = en.Y } : new Position { X = host.X + (float)(range * Math.Cos(tossAngle.Value)), Y = host.Y + (float)(range * Math.Sin(tossAngle.Value)) };

                host.Owner.Broadcast(new ShowEffect { EffectType = EffectType.Throw, Color = new ARGB(0xffffbf00), TargetObjectId = host.Id, Pos1 = target }, PacketPriority.Low);
                host.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    var entity = Entity.Resolve(world.Manager, child);
                    entity.Move(target.X, target.Y);

                    if (entity is Enemy && host is Enemy)
                        (entity as Enemy).Terrain = (host as Enemy).Terrain;

                    world.EnterWorld(entity);
                }));

                cool = coolDown.Next(Random);
            }
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }
    }
}
