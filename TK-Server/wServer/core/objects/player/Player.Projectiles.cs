using common.resources;

namespace wServer.core.objects
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(byte id, ProjectileDesc desc, ushort objType, int time, Position position, float angle)
        {
            projectileId = id;

            return CreateProjectile(desc, objType, Stats.GetAttackDamage(desc.MinDamage, desc.MaxDamage), C2STime(time), position, angle);
        }
    }
}
