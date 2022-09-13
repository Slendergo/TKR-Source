using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.worlds;
using wServer.memory;

namespace wServer.core.objects
{
    public sealed class Projectile
    {
        public int StartTime { get; }
        public int ObjectId { get; }
        public int BulletId { get; }
        public int ContainerType { get; }
        public float Angle { get; }
        public float StartX { get; }
        public float StartY { get; }
        public int Damage { get; }
        public ProjectileDesc ProjectileDesc { get; }

        public List<int> HitDictionary { get; } = new List<int>();

        public Projectile(int startTime, int objectId, int bulletId, int containerType, float angle, float startX, float startY, int damage, ProjectileDesc projectileDesc)
        {
            StartTime = startTime;
            ObjectId = objectId;
            BulletId = bulletId;
            ContainerType = containerType;
            Angle = angle;
            StartX = startX;
            StartY = startY;
            Damage = damage;
            ProjectileDesc = projectileDesc;
        }

        public bool HitEntity(Entity shooter, Entity entity, TickTime time)
        {
            if (ProjectileDesc.MultiHit && HitDictionary.Contains(entity.Id))
                return false;
            HitDictionary.Add(entity.Id);
            _ = entity.HitByProjectile(shooter, this, time);
            return true;
        }

        public bool IsElapsed(int time) => time - StartTime >= ProjectileDesc.LifetimeMS + RootWorldThread.TICK_TIME_MS; // 1 tick tollerance might remove in future

        public Position GetPosition(int elapsed)
        {
            var x = (double)StartX;
            var y = (double)StartY;

            var dist = elapsed * (ProjectileDesc.Speed / 10000.0);
            var phase = BulletId % 2 == 0 ? 0.0 : Math.PI;
            if (ProjectileDesc.Wavy)
            {
                var periodFactor = 6.0 * Math.PI;
                var amplitudeFactor = Math.PI / 64.0;
                var theta = Angle + amplitudeFactor * Math.Sin(phase + periodFactor * elapsed / 1000.0);
                x += dist * Math.Cos(theta);
                y += dist * Math.Sin(theta);
            }
            else if (ProjectileDesc.Parametric)
            {
                var t = elapsed / ProjectileDesc.LifetimeMS * 2 * Math.PI;
                x = Math.Sin(t) * (BulletId % 2 == 0 ? 1.0 : -1.0);
                y = Math.Sin(2.0 * t) * (BulletId % 4 < 2 ? 1.0 : -1.0);
                var sin = Math.Sin(Angle);
                var cos = Math.Cos(Angle);
                x += (x * cos - y * sin) * ProjectileDesc.Magnitude;
                y += (x * sin + y * cos) * ProjectileDesc.Magnitude;
            }
            else
            {
                if (ProjectileDesc.Boomerang)
                {
                    var halfway = ProjectileDesc.LifetimeMS * (ProjectileDesc.Speed / 10000.0) / 2.0;
                    if (dist > halfway)
                        dist = halfway - (dist - halfway);
                }
                x += dist * Math.Cos(Angle);
                y += dist * Math.Sin(Angle);
                if (ProjectileDesc.Amplitude != 0.0)
                {
                    var deflection = ProjectileDesc.Amplitude * Math.Sin(phase + elapsed / ProjectileDesc.LifetimeMS * ProjectileDesc.Frequency * 2.0 * Math.PI);
                    x += deflection * Math.Cos(Angle + Math.PI / 2.0);
                    y += deflection * Math.Sin(Angle + Math.PI / 2.0);
                }
            }
            return new Position((float)x, (float)y);
        }
    }
}
