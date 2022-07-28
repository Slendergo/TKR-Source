using System;
using System.Diagnostics;
using System.Threading;
using wServer.core.worlds;

namespace wServer.core
{
    public sealed class TickLoopSingle
    {
        public TickLoopSingle(TickThreadSingle tickThreadSingle, World world)
        {
            TickThreadSingle = tickThreadSingle;
            World = world;
        }

        private bool Stopped { get; set; }
        private TickThreadSingle TickThreadSingle { get; set; }
        private World World { get; set; }

        public void Stop()
        {
            if (Stopped)
                return;

            Stopped = true;
        }

        public void Tick()
        {
            var watch = Stopwatch.StartNew();

            var time = new TickTime();
            var last = 0L;

            var spin = new SpinWait();

            while (true)
            {
                if (!CoreServerManager.Initialized)
                {
                    Thread.Sleep(200);
                    continue;
                }

                var current = time.TotalElapsedMs = watch.ElapsedMilliseconds;
                var delta = (int)(current - last);

                if (delta >= 200)
                {
                    time.TickCount++;
                    time.ElaspedMsDelta = delta;

                    if(Stopped || World.Update(ref time))
                    {
                        _ = TickThreadSingle.WorldManager.RemoveWorld(World);
                        break;
                    }

                    last = current;
                }

                spin.SpinOnce();
            }
        }
    }
}
