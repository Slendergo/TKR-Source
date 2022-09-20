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

            var min = desc.MinDamage;
            var max = desc.MaxDamage;
            if (TalismanDamageIsAverage)
            {
                var avg = (int)((min + max) * 0.5);
                min = avg;
                max = avg;
            }

            var dmg = Stats.GetAttackDamage(min, max, ability);

            if(ability && TalismanExtraAbilityDamage > 0.0)
                dmg = (int)(dmg + (dmg * TalismanExtraAbilityDamage));

            var isFullHp = HP == Stats[0];
            if (TalismanExtraDamageOnHitHealth != 0.0)
                dmg += (int)(dmg * (isFullHp ? TalismanExtraDamageOnHitHealth : -TalismanExtraDamageOnHitHealth));

            var isFullMp = MP == Stats[1];
            if (TalismanExtraDamageOnHitMana != 0.0)
                dmg += (int)(dmg * (isFullMp ? TalismanExtraDamageOnHitMana : -TalismanExtraDamageOnHitMana));

            return CreateProjectile(desc, objectType, dmg, C2STime(time), position, angle);
        }
    }
}
