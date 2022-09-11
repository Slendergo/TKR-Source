using common.resources;

namespace wServer.core.objects
{
    public partial class Player
    {
        public byte[] NextBulletId = new byte[2] { 1, 128 };

        public byte GetNextBulletId(int index = 0)
        {
            var currentBulletId = NextBulletId[index];

            switch (index)
            {
                case 0:
                    NextBulletId[index] = (byte)((NextBulletId[index] + 1) % 128);
                    break;

                case 1:
                    NextBulletId[index] = (byte)((NextBulletId[index] + 1) % 256);
                    break;
            }

            return currentBulletId;
        }

        public int LastShootTime;
        public int LastAbilityTime;

        public bool IsValidAbilityTime(int time, int cooldown)
        {
            if (time < LastAbilityTime + cooldown)
                return false;
            LastAbilityTime = time;
            return true;
        }

        public bool IsValidShoot(int time, double rateOfFire)
        {
            var attackPeriod = (int)(1.0 / Stats.GetAttackFrequency() * (1.0 / rateOfFire));
            if (time < LastShootTime + attackPeriod)
                return false;
            LastShootTime = time;
            return true;
        }

        //internal Projectile PlayerShootProjectile(byte id, ProjectileDesc desc, ushort objType, int time, Position position, float angle, bool ability = false)
        //{
        //    var min = desc.MinDamage;
        //    var max = desc.MaxDamage;
        //    if (TalismanDamageIsAverage)
        //    {
        //        var avg = (int)((min + max) * 0.5);
        //        min = avg;
        //        max = avg;
        //    }

        //    var dmg = Stats.GetAttackDamage(min, max, ability);

        //    if(ability && TalismanExtraAbilityDamage > 0.0)
        //        dmg = (int)(dmg + (dmg * TalismanExtraAbilityDamage));

        //    var isFullHp = HP == Stats[0];
        //    if (TalismanExtraDamageOnHitHealth != 0.0)
        //        dmg += (int)(dmg * (isFullHp ? TalismanExtraDamageOnHitHealth : -TalismanExtraDamageOnHitHealth));

        //    var isFullMp = MP == Stats[1];
        //    if (TalismanExtraDamageOnHitMana != 0.0)
        //        dmg += (int)(dmg * (isFullMp ? TalismanExtraDamageOnHitMana : -TalismanExtraDamageOnHitMana));

        //    return CreateProjectile(desc, objType, dmg, time, position, angle, id);
        //}
    }
}
