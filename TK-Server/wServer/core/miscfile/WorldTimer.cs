using System;
using wServer.core.worlds;

namespace wServer.core
{
    public class WorldTimer
    {
        private readonly Action<World, TickData> _cb;
        private readonly Func<World, TickData, bool> _rcb;
        private readonly int _total;
        private int _remain;

        public WorldTimer(int tickMs, Action<World, TickData> callback)
        {
            _remain = _total = tickMs;
            _cb = callback;
        }

        public WorldTimer(int tickMs, Func<World, TickData, bool> callback)
        {
            _remain = _total = tickMs;
            _rcb = callback;
        }

        public void Reset()
        {
            _remain = _total;
        }

        public bool Tick(World world, TickData time)
        {
            _remain -= time.ElaspedMsDelta;

            if (_remain >= 0)
                return false;

            if (_cb != null)
            {
                _cb(world, time);
                return true;
            }

            return _rcb(world, time);
        }
    }
}
