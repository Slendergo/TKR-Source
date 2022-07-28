using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace wServer.core.objects
{
    public interface IProjectileOwner
    {
        Projectile[] Projectiles { get; }
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        public ProjectileDesc ProjDesc;
        private HashSet<Entity> _hit = new HashSet<Entity>();
        private ConcurrentDictionary<Player, Tuple<int, int>> _startTime = new ConcurrentDictionary<Player, Tuple<int, int>>();

        public Projectile(CoreServerManager manager, ProjectileDesc desc) : base(manager, manager.Resources.GameData.IdToObjectType[desc.ObjectId]) => ProjDesc = desc;

        public float Angle { get; set; }
        public ushort Container { get; set; }
        public long CreationTime { get; set; }
        public int Damage { get; set; }
        public byte ProjectileId { get; set; }
        public IProjectileOwner ProjectileOwner { get; set; }

        public Entity ProjEntity => this as Entity;

        public Position StartPos { get; set; }
        private bool _used { get; set; }

        public void AddPlayerStartTime(Player player, int serverTime, int clientTime) => _startTime.TryAdd(player, new Tuple<int, int>(serverTime, clientTime));

        public void ForceHit(Entity entity, TickData time)
        {
            if (!ProjDesc.MultiHit && _used && !(entity is Player))
                return;

            if (_hit.Add(entity))
                entity.HitByProjectile(this, time);

            _used = true;
        }

        public void GetHit(float pX, float pY, TickData time)
        {
            foreach (var player in Owner.PlayersCollision.HitTest(ProjEntity.X, ProjEntity.Y, 30).Where(e => e is Player && e != ProjectileOwner.Self))
            {
                var xDiff = player.X > pX ? player.X - pX : pX - player.X;
                var yDiff = player.Y > pY ? player.Y - pY : pY - player.Y;

                if (!(xDiff > 0.2 || yDiff > 0.2))
                {
                    Console.WriteLine(xDiff);
                    Console.WriteLine(yDiff);

                    ForceHit(player, time);
                }
            }
        }

        public int GetPlayerClientStartTime(Player player)
        {
            if (!_startTime.ContainsKey(player))
                return -1;

            return _startTime[player].Item2;
        }

        public int GetPlayerServerStartTime(Player player)
        {
            if (!_startTime.ContainsKey(player))
                return -1;

            return _startTime[player].Item1;
        }

        public Position GetPosition(long elapsedTicks)
        {
            double periodFactor;
            double amplitudeFactor;
            double theta;

            var t = 0d;
            var x = 0d;
            var y = 0d;
            var sin = 0d;
            var cos = 0d;
            var halfway = 0d;
            var deflection = 0d;
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
                t = elapsedTicks / ProjDesc.LifetimeMS * 2 * Math.PI;
                x = Math.Sin(t) * (ProjectileId % 2 == 0 ? 1 : -1);
                y = Math.Sin(2 * t) * (ProjectileId % 4 < 2 ? 1 : -1);
                sin = Math.Sin(Angle);
                cos = Math.Cos(Angle);
                pX += (x * cos - y * sin) * ProjDesc.Magnitude;
                pY += (x * sin + y * cos) * ProjDesc.Magnitude;
            }
            else
            {
                if (ProjDesc.Boomerang)
                {
                    halfway = ProjDesc.LifetimeMS * (ProjDesc.Speed / 10000) / 2;

                    if (dist > halfway)
                        dist = halfway - (dist - halfway);
                }
                pX += dist * Math.Cos(Angle);
                pY += dist * Math.Sin(Angle);

                if (ProjDesc.Amplitude != 0)
                {
                    deflection = ProjDesc.Amplitude * Math.Sin(phase + elapsedTicks / ProjDesc.LifetimeMS * ProjDesc.Frequency * 2 * Math.PI);
                    pX += deflection * Math.Cos(Angle + Math.PI / 2);
                    pY += deflection * Math.Sin(Angle + Math.PI / 2);
                }
            }

            return new Position() { X = (float)pX, Y = (float)pY };
        }

        public void OnDestroy() => Owner?.LeaveWorld(this);

        public override void Tick(TickData time)
        {
            var elapsed = time.TotalElapsedMs - CreationTime;

            if (elapsed > ProjDesc.LifetimeMS)
            {
                OnDestroy();
                return;
            }

            base.Tick(time);
        }
    }
}
