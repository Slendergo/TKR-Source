using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace wServer.core.objects
{
    public interface IProjectileOwner
    {
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        public ProjectileDesc ProjDesc;
        private HashSet<Entity> _hit = new HashSet<Entity>();
        
        public Projectile(GameServer manager, ProjectileDesc desc) : base(manager, manager.Resources.GameData.IdToObjectType[desc.ObjectId]) => ProjDesc = desc;

        public float Angle { get; set; }
        public ushort Container { get; set; }
        public long CreationTime { get; set; }
        public int Damage { get; set; }
        public byte ProjectileId { get; set; }
        public IProjectileOwner ProjectileOwner { get; set; }

        public Entity ProjEntity => this as Entity;

        public Position StartPos { get; set; }
        private bool _used { get; set; }

        public void ForceHit(Entity entity, TickTime time)
        {
            if (!ProjDesc.MultiHit && _used && !(entity is Player))
                return;

            if (_hit.Add(entity))
                entity.HitByProjectile(this, time);

            _used = true;
        }

        public bool Update(ref TickTime time)
        {
            var elapsed = time.TotalElapsedMs - CreationTime;
            if (elapsed > ProjDesc.LifetimeMS)
            {
                World.LeaveWorld(this);
                return true;
            }
            return false;
        }

        public Position GetPosition(long elapsedTicks)
        {
            double periodFactor;
            double amplitudeFactor;
            double theta;
            
            var pX = (double)StartPos.X;
            var pY = (double)StartPos.Y;
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

            return new Position() { X = (float)pX, Y = (float)pY };
        }

    }
}
