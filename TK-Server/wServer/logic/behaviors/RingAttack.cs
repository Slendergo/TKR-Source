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

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = new RingAttackState { angleToIncrement = _angleToIncrement, fixedAngle = _fixedAngle, coolDown = _coolDown };

        protected override void TickCore(Entity Host, TickData time, ref object state)
        {
            var rastate = (RingAttackState)state;

            Status = CycleStatus.NotStarted;

            if (Host == null || Host.Owner == null)
                return;

            if (Host.HasConditionEffect(ConditionEffects.Stunned))
                return;

            var dist = (float)_radius;
            var isQuest = Host.ObjectDesc.Quest;

            if (rastate.coolDown.CoolDown <= 0)
            {
                var entity = _radius == 0 ? null : Host.GetNearestEntity((float)_radius, null, _seeInvis);
                var enemy = Host as Enemy;
                var enemyClasified = enemy.Legendary ? 50 : enemy.Epic ? 25 : enemy.Rare ? 10 : 0;

                if (_radius == 0)
                {
                    if (!(Host is Character chr) || chr.Owner == null) return;

                    var angleInc = 2 * Math.PI / _count;
                    var desc = Host.ObjectDesc.Projectiles[_projectileIndex];

                    float? angle = null;

                    if (rastate.fixedAngle != null)
                    {
                        if (rastate.angleToIncrement != null)
                        {
                            if (_useSavedAngle)
                                rastate.fixedAngle = Host.savedAngle;

                            rastate.fixedAngle += rastate.angleToIncrement;
                            Host.savedAngle = rastate.fixedAngle;
                        }
                        angle = (float)rastate.fixedAngle;
                    }
                    else
                        angle = entity == null ? _offset : (float)Math.Atan2(entity.Y - chr.Y, entity.X - chr.X) + _offset;

                    var count = _count;
                    if (Host.HasConditionEffect(ConditionEffects.Dazed))
                        count = Math.Max(1, count / 2);

                    var dmg = Random.Next(desc.MinDamage + enemyClasified, desc.MaxDamage + enemyClasified);
                    var startAngle = angle * (count - 1) / 2;

                    byte prjId = 0;

                    var prjPos = new Position() { X = Host.X, Y = Host.Y };
                    var prjs = new Projectile[count];

                    for (var i = 0; i < count; i++)
                    {
                        var prj = Host.CreateProjectile(desc, Host.ObjectType, dmg, time.TotalElapsedMs, prjPos, (float)(startAngle + angle * i));

                        Host.Owner.EnterWorld(prj);

                        if (i == 0)
                            prjId = prj.ProjectileId;

                        prjs[i] = prj;
                    }

                    var pkt = new EnemyShoot()
                    {
                        BulletId = prjId,
                        OwnerId = Host.Id,
                        StartingPos = prjPos,
                        Angle = (float)angle,
                        Damage = (short)dmg,
                        BulletType = (byte)desc.BulletType,
                        AngleInc = (float)angleInc,
                        NumShots = (byte)count
                    };

                    if (isQuest)
                        Host.Owner.PlayersBroadcastAsParallel(_ => _.Client.SendPacket(pkt));
                    else
                    {
                        var players = Host.Owner.Players
                            .ValueWhereAsParallel(_ => _.DistSqr(Host) < PlayerUpdate.VISIBILITY_RADIUS_SQR);
                        for (var i = 0; i < players.Length; i++)
                            players[i].Client.SendPacket(pkt);
                    }
                }

                rastate.coolDown = _coolDown.Next(Random);

                Status = CycleStatus.Completed;
            }
            else
            {
                rastate.coolDown.CoolDown -= time.ElaspedMsDelta;

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
