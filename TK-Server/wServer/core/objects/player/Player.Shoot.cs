using common.resources;
using NLog;
using System;

namespace wServer.core.objects
{
    partial class Player
    {
        private const float MaxTimeDiff = 1.08f;
        private const float MinTimeDiff = 0.92f;

        private static readonly Logger CheatLog = LogManager.GetCurrentClassLogger();

        private int _lastShootTime;
        private int _shotsLeft;

        private TimeCop _time = new TimeCop();

        public bool IsNoClipping()
        {
            if (World == null || !TileOccupied(RealX, RealY) && !TileFullOccupied(RealX, RealY))
                return false;
            CheatLog.Info($"{Name} is walking on an occupied tile. {RealX},{RealY}");
            return true;
        }

        public PlayerShootStatus ValidatePlayerShoot(Item item, int time)
        {
            if (item != Inventory[0])
                return PlayerShootStatus.ITEM_MISMATCH;
            /*
            var bigSkill = BigSkill11 ? 0.35 : 0;
            if (SmallSkill11 > 0)
                bigSkill += SmallSkill11 * 0.02;
            if (BigSkill1)
                bigSkill -= 0.10;
            if (BigSkill4)
                bigSkill -= 0.05; 
            var rateSkill = bigSkill * item.RateOfFire;*/
            var rateOfFire = item.RateOfFire;

            var dt = (int)(1 / Stats.GetAttackFrequency() * 1 / rateOfFire);
            if (time < _time.LastClientTime() + dt)
                return PlayerShootStatus.COOLDOWN_STILL_ACTIVE;

            if (time != _lastShootTime)
            {
                _lastShootTime = time;
                if (_shotsLeft != 0 && _shotsLeft < item.NumProjectiles)
                {
                    _shotsLeft = 0;
                    _time.Push(time, Environment.TickCount);
                    return PlayerShootStatus.NUM_PROJECTILE_MISMATCH;
                }
                _shotsLeft = 0;
            }

            _shotsLeft++;
            if (_shotsLeft >= item.NumProjectiles)
                _time.Push(time, Environment.TickCount);

            var timeDiff = _time.TimeDiff();
            //Log.Info($"timeDiff: {timeDiff}");
            if (timeDiff < MinTimeDiff)
                return PlayerShootStatus.CLIENT_TOO_SLOW;
            if (timeDiff > MaxTimeDiff)
                return PlayerShootStatus.CLIENT_TOO_FAST;
            return PlayerShootStatus.OK;
        }
    }
}
