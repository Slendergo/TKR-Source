using common.resources;
using System;
using System.Collections.Generic;

namespace wServer.core.objects
{
    public sealed class Projectile
    {
        public int StartTime { get; }
        public byte BulletId { get; }
        public ushort ContainerType { get; }
        public double Angle { get; }
        public double StartX { get; }
        public double StartY { get; }
        public int Damage { get; }
        public ProjectileDesc XMLProjectile { get; }

        public List<int> HitDictionary { get; }

        public Projectile(int startTime, byte bulletId, ushort containerType, double angle, double startX, double startY, int damage, ProjectileDesc xmlProjectile)
        {
            StartTime = startTime;
            BulletId = bulletId;
            ContainerType = containerType;
            Angle = angle;
            StartX = startX;
            StartY = startY;
            Damage = damage;
            XMLProjectile = xmlProjectile;

            HitDictionary = new List<int>();
        }

        public Position ProjectilePositionAt(int elapsed)
        {
            var x = StartX;
            var y = StartY;

            var dist = elapsed * (XMLProjectile.Speed / 10000.0);
            var phase = BulletId % 2 == 0 ? 0.0 : Math.PI;
            if (XMLProjectile.Wavy)
            {
                var periodFactor = 6.0 * Math.PI;
                var amplitudeFactor = Math.PI / 64.0;
                var theta = Angle + amplitudeFactor * Math.Sin(phase + periodFactor * elapsed / 1000.0);
                x += dist * Math.Cos(theta);
                y += dist * Math.Sin(theta);
            }
            else if (XMLProjectile.Parametric)
            {
                var t = elapsed / XMLProjectile.LifetimeMS * 2 * Math.PI;
                x = Math.Sin(t) * (BulletId % 2 == 0 ? 1.0 : -1.0);
                y = Math.Sin(2.0 * t) * (BulletId % 4 < 2 ? 1.0 : -1.0);
                var sin = Math.Sin(Angle);
                var cos = Math.Cos(Angle);
                x += (x * cos - y * sin) * XMLProjectile.Magnitude;
                y += (x * sin + y * cos) * XMLProjectile.Magnitude;
            }
            else
            {
                if (XMLProjectile.Boomerang)
                {
                    var halfway = XMLProjectile.LifetimeMS * (XMLProjectile.Speed / 10000.0) / 2.0;
                    if (dist > halfway)
                        dist = halfway - (dist - halfway);
                }
                x += dist * Math.Cos(Angle);
                y += dist * Math.Sin(Angle);
                if (XMLProjectile.Amplitude != 0.0)
                {
                    var deflection = XMLProjectile.Amplitude * Math.Sin(phase + elapsed / XMLProjectile.LifetimeMS * XMLProjectile.Frequency * 2.0 * Math.PI);
                    x += deflection * Math.Cos(Angle + Math.PI / 2.0);
                    y += deflection * Math.Sin(Angle + Math.PI / 2.0);
                }
            }
            return new Position((float)x, (float)y);
        }
    }
}
