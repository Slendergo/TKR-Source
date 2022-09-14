using CA.Extensions.Concurrent;
using common.resources;
using Mono.Game;
using System;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class Shoot : CycleBehavior
    {
        private const int PREDICT_NUM_TICKS = 4;

        private readonly float _angleOffset;
        private readonly int _coolDownOffset;
        private readonly int _count;
        private readonly float? _defaultAngle;
        private readonly float? _fixedAngle;
        private readonly float _predictive;
        private readonly int _projectileIndex;
        private readonly float _radius;
        private readonly float? _rotateAngle;
        private readonly bool _seeInvis;
        private readonly float _shootAngle;
        private readonly bool _shootLowHp;
        private Cooldown _coolDown;
        private int _rotateCount;

        public Shoot(double radius, int count = 1, double? shootAngle = null, int projectileIndex = 0, double? fixedAngle = null, double? rotateAngle = null, double angleOffset = 0, double? defaultAngle = null, double predictive = 0, int coolDownOffset = 0, Cooldown coolDown = new Cooldown(), bool shootLowHp = false, bool seeInvis = false)
        {
            _radius = (float)radius;
            _count = count;
            _shootAngle = count == 1 ? 0 : (float)((shootAngle ?? 360.0 / count) * Math.PI / 180);
            _projectileIndex = projectileIndex;
            _fixedAngle = (float?)(fixedAngle * Math.PI / 180);
            _rotateAngle = (float?)(rotateAngle * Math.PI / 180);
            _angleOffset = (float)(angleOffset * Math.PI / 180);
            _defaultAngle = (float?)(defaultAngle * Math.PI / 180);
            _predictive = (float)predictive;
            _coolDownOffset = coolDownOffset;
            _coolDown = coolDown.Normalize();
            _shootLowHp = shootLowHp;
            _seeInvis = seeInvis;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => state = _coolDownOffset;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var cool = (int?)state ?? -1; // <-- crashes server due to state being null... patched now but should be looked at.

            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return;

                var count = _count;

                if (host.HasConditionEffect(ConditionEffectIndex.Dazed))
                    count = (int)Math.Ceiling(_count / 2.0);

                var player = host.AttackTarget ?? (_shootLowHp ? host.GetLowestHpEntity(_radius, null, _seeInvis) : host.GetNearestEntity(_radius, null, _seeInvis));

                if (player != null || _defaultAngle != null || _fixedAngle != null)
                {
                    var desc = host.ObjectDesc.Projectiles[_projectileIndex];

                    float a;

                    if (_fixedAngle != null)
                        a = (float)_fixedAngle;
                    else if (player != null)
                    {
                        if (_predictive != 0 && _predictive > Random.NextDouble())
                            a = Predict(host, player, desc);
                        else
                            a = (float)Math.Atan2(player.Y - host.Y, player.X - host.X);
                    }
                    else if (_defaultAngle != null)
                        a = (float)_defaultAngle;
                    else
                        a = 0;

                    a += _angleOffset + ((_rotateAngle != null) ? (float)_rotateAngle * _rotateCount : 0);

                    _rotateCount++;

                    var dmg = (double)Random.Next(desc.MinDamage, desc.MaxDamage);

                    if (host.HasConditionEffect(ConditionEffectIndex.Weak))
                        dmg /= 2;

                    var startAngle = a - _shootAngle * (count - 1) / 2;

                    int prjId = 0;

                    var prjPos = new Position() { X = host.X, Y = host.Y };
                    var prjs = new Projectile[count];

                    for (var i = 0; i < count; i++)
                    {
                        if (host == null || host.World == null)
                            return;

                        var prj = host.CreateProjectile(desc, host.ObjectType, (int)dmg, time.TotalElapsedMs, prjPos, startAngle + _shootAngle * i);

                        host.World.AddProjectile(prj);

                        if (i == 0)
                            prjId = prj.ProjectileId;

                        prjs[i] = prj;
                    }

                    var pkt = new EnemyShoot()
                    {
                        BulletId = prjId,
                        OwnerId = host.Id,
                        StartingPos = prjPos,
                        Angle = startAngle,
                        Damage = (short)dmg,
                        BulletType = (byte)desc.BulletType,
                        AngleInc = _shootAngle,
                        NumShots = (byte)count,
                    };

                    // changed to this
                    host.World.BroadcastIfVisible(pkt, host);
                }

                cool = _coolDown.Next(Random);

                Status = CycleStatus.Completed;
            }
            else
            {
                cool -= time.ElapsedMsDelta;

                Status = CycleStatus.InProgress;
            }

            state = cool;
        }

        private static float Predict(Entity host, Entity target, ProjectileDesc desc)
        {
            var targetX = target.X + PREDICT_NUM_TICKS * (target.X - target.PrevX);
            var targetY = target.Y + PREDICT_NUM_TICKS * (target.Y - target.PrevY);
            var angle = (float)Math.Atan2(targetY - host.Y, targetX - host.X);
            return angle;
        }
    }
}
