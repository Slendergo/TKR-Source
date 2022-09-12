using common.resources;

namespace wServer.core.objects
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(int time, int bulletId, ushort objectType, float angle, Position position, ProjectileDesc desc, bool ability = false)
        {
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

            var ret = new Projectile(time, Id, bulletId, objectType, angle, Pos.X, Pos.Y, dmg, desc);
            AddProjectile(ret);
            return ret;
        }
    }
}
