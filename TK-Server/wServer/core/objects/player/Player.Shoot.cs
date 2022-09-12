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
    }
}
