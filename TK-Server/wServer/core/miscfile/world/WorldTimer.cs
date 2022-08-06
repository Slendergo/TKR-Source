using System;
using wServer.core.worlds;

namespace wServer.core
{
    public class WorldTimer
    {
        private readonly Action<World, TickTime> Callback;
        private readonly Func<World, TickTime, bool> funcCallback;
        private readonly int Total;
        private int Remaining;

        public WorldTimer(int tickMs, Action<World, TickTime> callback)
        {
            Remaining = Total = tickMs;
            Callback = callback;
        }

        public WorldTimer(int tickMs, Func<World, TickTime, bool> callback)
        {
            Remaining = Total = tickMs;
            funcCallback = callback;
        }

        public bool Tick(World world, TickTime time)
        {
            Remaining -= time.ElaspedMsDelta;
            if (Remaining >= 0)
                return false;

            if (Callback != null)
            {
                Callback.Invoke(world, time);
                return true;
            }
            return funcCallback.Invoke(world, time);
        }

        public void Reset()
        {
            Remaining = Total;
        }
    }
}
