using TKR.Shared.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.memory;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.core.objects
{
    public sealed class Projectile : IObjectPoolObject
    {
        public int ProjectileId;
        public float StartX;
        public float StartY;
        public float Angle;
        public ushort Container;
        public long CreationTime;
        public int Damage;
        public Entity Host;
        public World World;
        public ProjectileDesc ProjDesc;
        public int Ticks;
        private List<int> Hits = new List<int>();

        public Projectile() { }

        public bool HasAlreadyHitTarget(int objectId) => Hits.Contains(objectId) && ProjDesc.MultiHit;

        public void RegisterHit(int objectId) => Hits.Add(objectId);

        public bool HasElapsed(long time) => time - CreationTime > ProjDesc.LifetimeMS;

        public bool Tick(ref TickTime time)
        {
            if(Host is Player)
                return true;
            var elapsed = time.TotalElapsedMs - CreationTime;
            if (elapsed > ProjDesc.LifetimeMS)
                return false;
            return true;
        }

        public Position GetPosition(long elapsedTicks)
        {
            double periodFactor;
            double amplitudeFactor;
            double theta;

            var pX = (double)StartX;
            var pY = (double)StartY;
            var dist = elapsedTicks * ProjDesc.Speed / 10000.0;
            var phase = ProjectileId % 2 == 0 ? 0 : Math.PI;

            if (ProjDesc.Wavy)
            {
                periodFactor = 6 * Math.PI;
                amplitudeFactor = Math.PI / 64;
                theta = Angle + amplitudeFactor * Math.Sin(phase + periodFactor * elapsedTicks / 1000);
                pX += dist * Math.Cos(theta);
                pY += dist * Math.Sin(theta);
            }
            else if (ProjDesc.Parametric)
            {
                var t = elapsedTicks / ProjDesc.LifetimeMS * 2 * Math.PI;
                var x = Math.Sin(t) * (ProjectileId % 2 == 0 ? 1 : -1);
                var y = Math.Sin(2 * t) * (ProjectileId % 4 < 2 ? 1 : -1);
                var sin = Math.Sin(Angle);
                var cos = Math.Cos(Angle);
                pX += (x * cos - y * sin) * ProjDesc.Magnitude;
                pY += (x * sin + y * cos) * ProjDesc.Magnitude;
            }
            else
            {
                if (ProjDesc.Boomerang)
                {
                    double halfway = ProjDesc.LifetimeMS * (ProjDesc.Speed / 10000) / 2;

                    if (dist > halfway)
                        dist = halfway - (dist - halfway);
                }
                pX += dist * Math.Cos(Angle);
                pY += dist * Math.Sin(Angle);

                if (ProjDesc.Amplitude != 0)
                {
                    var deflection = ProjDesc.Amplitude * Math.Sin(phase + elapsedTicks / ProjDesc.LifetimeMS * ProjDesc.Frequency * 2 * Math.PI);
                    pX += deflection * Math.Cos(Angle + Math.PI / 2);
                    pY += deflection * Math.Sin(Angle + Math.PI / 2);
                }
            }

            return new Position((float)pX, (float)pY);
        }

        public void Reset()
        {
            ProjectileId = 0;
            StartX = 0.0f;
            StartY = 0.0f;
            Angle = 0.0f;
            Container = 0;
            CreationTime = 0;
            Damage = 0;
            Host = null;
            World = null;
            Hits.Clear();
            Hits.TrimExcess();
            ProjDesc = null;
        }
    }
}
