using common.resources;

namespace wServer.core.objects
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(byte id, ProjectileDesc desc, ushort objType, int time, Position position, float angle)
        {
            projectileId = id;

            var dmg = Stats.GetAttackDamage(desc.MinDamage, desc.MaxDamage);

            var isFullHp = HP == Stats[0];
            if (TalismanExtraDamageOnHitHealth != 0.0)
                dmg += (int)(dmg * (isFullHp ? TalismanExtraDamageOnHitHealth : -TalismanExtraDamageOnHitHealth));

            var isFullMp = MP == Stats[1];
            if (TalismanExtraDamageOnHitMana != 0.0)
                dmg += (int)(dmg * (isFullMp ? TalismanExtraDamageOnHitMana : -TalismanExtraDamageOnHitMana));

            return CreateProjectile(desc, objType, dmg, C2STime(time), position, angle);
        }
    }
}
