using CA.Extensions.Concurrent;
using common.resources;
using System;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class RingAttack : CycleBehavior
    {
        private readonly int _count;
        private readonly int _projectileIndex;
        private readonly bool _seeInvis;
        private readonly bool _useSavedAngle;
        private float? _angleToIncrement;
        private Cooldown _coolDown;
        private float? _fixedAngle;
        private float? _offset;
        private float? _radius;

        public RingAttack(double radius, int count, double offset, int projectileIndex, double angleToIncrement, double? fixedAngle = null, Cooldown coolDown = new Cooldown(), bool seeInvis = false, bool useSavedAngle = false)
        {
            _count = count;
            _radius = (float)radius;
            _offset = (float)offset;
            _projectileIndex = projectileIndex;
            _angleToIncrement = (float?)angleToIncrement;
            _fixedAngle = (float?)(fixedAngle * Math.PI / 180);
            _coolDown = coolDown.Zero();
            _seeInvis = seeInvis;
            _useSavedAngle = useSavedAngle;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => state = new RingAttackState { angleToIncrement = _angleToIncrement, fixedAngle = _fixedAngle, coolDown = _coolDown };

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var rastate = (RingAttackState)state;

            Status = CycleStatus.NotStarted;

            if (host == null || host.World == null)
                return;

            if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                return;

            var dist = (float)_radius;
            var isQuest = host.ObjectDesc.Quest;

            if (rastate.coolDown.CoolDown <= 0)
            {
                var entity = _radius == 0 ? null : host.GetNearestEntity((float)_radius, null, _seeInvis);
                var enemy = host as Enemy;
                var enemyClasified = enemy.Legendary ? 50 : enemy.Epic ? 25 : enemy.Rare ? 10 : 0;

                if (_radius == 0)
                {
                    if (!(host is Character chr) || chr.World == null) return;

                    var angleInc = 2 * Math.PI / _count;
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float? angle = null;

                    if (rastate.fixedAngle != null)
                    {
                        if (rastate.angleToIncrement != null)
                        {
                            if (_useSavedAngle)
                                rastate.fixedAngle = host.savedAngle;

                            rastate.fixedAngle += rastate.angleToIncrement;
                            host.savedAngle = rastate.fixedAngle;
                        }
                        angle = (float)rastate.fixedAngle;
                    }
                    else
                        angle = entity == null ? _offset : (float)Math.Atan2(entity.Y - chr.Y, entity.X - chr.X) + _offset;

                    var count = _count;
                    if (host.HasConditionEffect(ConditionEffectIndex.Dazed))
                        count = Math.Max(1, count / 2);

                    var dmg = Random.Next(desc.MinDamage + enemyClasified, desc.MaxDamage + enemyClasified);

                    var prjPos = new Position() { X = host.X, Y = host.Y };

                    var bulletId = host.GetNextBulletId(count);
                    var pkt = new EnemyShoot(bulletId, host.Id, (byte)desc.BulletType, host.ObjectType, ref prjPos, (float)angle, dmg, (byte)count, (float)angleInc);
                    
                    host.World.BroadcastIfVisible(pkt, host);
                }

                rastate.coolDown = _coolDown.Next(Random);

                Status = CycleStatus.Completed;
            }
            else
            {
                rastate.coolDown.CoolDown -= time.ElapsedMsDelta;

                Status = CycleStatus.InProgress;
            }

            state = rastate;
        }

        public class RingAttackState
        {
            public float? angleToIncrement;
            public Cooldown coolDown;
            public float? fixedAngle;
        }
    }
}
