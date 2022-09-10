using common.resources;
using NLog;
using System;

namespace wServer.core.objects
{
    partial class Player
    {
        private const float MaxTimeDiff = 1.08f;
        private const float MinTimeDiff = 0.92f;

        private int _lastShootTime;
        private int _shotsLeft;

        private TimeCop _time = new TimeCop();

        public int SpeedCountTollerance;

        public PlayerShootStatus ValidatePlayerShoot(Item item, int time)
        {
            //if (item.ObjectType != Inventory[0].ObjectType)
            //    return PlayerShootStatus.ITEM_MISMATCH;

            // todo figure out a way to stop desync
            
            //var rateOfFire = item.RateOfFire;
            //var dt = (int)(1 / Stats.GetAttackFrequency() * 1 / rateOfFire);
            //if (time < _time.LastClientTime() + dt)
            //    return PlayerShootStatus.COOLDOWN_STILL_ACTIVE;

            //if (time != _lastShootTime)
            //{
            //    _lastShootTime = time;
            //    if (_shotsLeft != 0 && _shotsLeft < item.NumProjectiles)
            //    {
            //        _shotsLeft = 0;
            //        _time.Push(time, Environment.TickCount);
            //        return PlayerShootStatus.NUM_PROJECTILE_MISMATCH;
            //    }
            //    _shotsLeft = 0;
            //}

            //_shotsLeft++;
            //if (_shotsLeft >= item.NumProjectiles)
            //    _time.Push(time, Environment.TickCount);

            //var timeDiff = _time.TimeDiff();
            ////Log.Info($"timeDiff: {timeDiff}");
            //if (timeDiff < MinTimeDiff)
            //    return PlayerShootStatus.CLIENT_TOO_SLOW;
            //if (timeDiff > MaxTimeDiff)
            //    return PlayerShootStatus.CLIENT_TOO_FAST;
            return PlayerShootStatus.OK;
        }
    }
}
