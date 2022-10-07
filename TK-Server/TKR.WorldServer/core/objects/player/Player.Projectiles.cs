using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.objects
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(int time, int bulletId, ushort objectType, float angle, Position position, ProjectileDesc desc, bool ability = false)
        {
            projectileId = bulletId;
            var dmg = Stats.GetAttackDamage(desc.MinDamage, desc.MaxDamage, ability);
            return CreateProjectile(desc, objectType, dmg, C2STime(time), position, angle);
        }
    }
}
