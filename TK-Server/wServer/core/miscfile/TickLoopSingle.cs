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

            var realmTime = new TickData();
            var last = 0L;

            while (true)
            {
                if (!CoreServerManager.Initialized)
                {
                    Thread.Sleep(50);
                    continue;
                }

                var current = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;
                var delta = (int)(current - last);

                realmTime.TickCount++;
                realmTime.ElaspedMsDelta = delta;

                if (Stopped || World.Tick(realmTime))
                {
                    TickThreadSingle.WorldManager.RemoveWorld(World);
                    break;
                }
                else
                {
                    World.ProcessNetworking(realmTime);
                    World.TickLogic(realmTime);
                    World.PlayerUpdate(realmTime);
                }

                var logicTime = (int)(watch.ElapsedMilliseconds - realmTime.TotalElapsedMs);

                var sleepTime = Math.Max(0, 50 - logicTime);// 50 ms -> 20 tps - time to do the update

                Thread.Sleep(sleepTime);

                last = current;
            }
        }
    }
}
