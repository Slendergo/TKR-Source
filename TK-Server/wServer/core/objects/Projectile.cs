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

        public Position GetPosition(long elapsedTicks)
        {
            double periodFactor;
            double amplitudeFactor;
            double theta;
            
            var pX = (double)StartX;
            var pY = (double)StartY;
            var dist = elapsedTicks * ProjectileDesc.Speed / 10000.0;
            var phase = BulletId % 2 == 0 ? 0 : Math.PI;

            if (ProjectileDesc.Wavy)
            {
                periodFactor = 6 * Math.PI;
                amplitudeFactor = Math.PI / 64;
                theta = Angle + amplitudeFactor * Math.Sin(phase + periodFactor * elapsedTicks / 1000);
                pX += dist * Math.Cos(theta);
                pY += dist * Math.Sin(theta);
            }
            else if (ProjectileDesc.Parametric)
            {
                var t = elapsedTicks / ProjectileDesc.LifetimeMS * 2 * Math.PI;
                var x = Math.Sin(t) * (BulletId % 2 == 0 ? 1 : -1);
                var y = Math.Sin(2 * t) * (BulletId % 4 < 2 ? 1 : -1);
                var sin = Math.Sin(Angle);
                var cos = Math.Cos(Angle);
                pX += (x * cos - y * sin) * ProjectileDesc.Magnitude;
                pY += (x * sin + y * cos) * ProjectileDesc.Magnitude;
            }
            else
            {
                if (ProjectileDesc.Boomerang)
                {
                    double halfway = ProjectileDesc.LifetimeMS * (ProjectileDesc.Speed / 10000) / 2;

                    if (dist > halfway)
                        dist = halfway - (dist - halfway);
                }
                pX += dist * Math.Cos(Angle);
                pY += dist * Math.Sin(Angle);

                if (ProjectileDesc.Amplitude != 0)
                {
                    var deflection = ProjectileDesc.Amplitude * Math.Sin(phase + elapsedTicks / ProjectileDesc.LifetimeMS * ProjectileDesc.Frequency * 2 * Math.PI);
                    pX += deflection * Math.Cos(Angle + Math.PI / 2);
                    pY += deflection * Math.Sin(Angle + Math.PI / 2);
                }
            }
            return new Position((float)pX, (float)pY);
        }
    }
}
