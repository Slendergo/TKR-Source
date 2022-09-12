using System.Collections.Generic;

namespace wServer.core.objects
{
    partial class Player
    {
        private int LastShootTime;

        public bool IsValidShoot(int time, double rateOfFire)
        {
            var dt = (int)(1 / Stats.GetAttackFrequency() * 1 / rateOfFire);
            if (time < LastShootTime + dt)
                return true;
            LastShootTime = time;
            return false;
        }

        public Dictionary<int, Dictionary<int, Projectile>> Projectiles { get; private set; } = new Dictionary<int, Dictionary<int, Projectile>>();
        
        public void AddProjectile(Projectile projectile)
        {
            if (!Projectiles.ContainsKey(projectile.ObjectId))
                Projectiles.Add(projectile.ObjectId, new Dictionary<int, Projectile>());
            Projectiles[projectile.ObjectId][projectile.BulletId] = projectile;
        }

        public Projectile GetProjectile(int objectId, int bulletId)
        {
            if (Projectiles.TryGetValue(objectId, out var projectiles))
                if (projectiles.TryGetValue(bulletId, out var ret))
                    return ret;
            return null;
        }

        public void RemoveProjectile(Projectile projectile)
        {
            if (Projectiles.ContainsKey(projectile.ObjectId))
                Projectiles[projectile.ObjectId].Remove(projectile.BulletId);
        }
    }
}
