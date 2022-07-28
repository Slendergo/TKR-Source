using System;
using System.Diagnostics;
using System.Threading;
using wServer.utils;

namespace wServer.core
{
    public sealed class TickLoopBatch
    {
        public TickLoopBatch(TickThreadBatch tickThreadBatch) => TickThreadBatch = tickThreadBatch;

        private bool Stopped { get; set; }
        private TickThreadBatch TickThreadBatch { get; set; }

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

                var worlds = TickThreadBatch.Attached.ToArray();

                foreach (var world in worlds)
                {
                    if (world == null)
                        continue;

                    try
                    {
                        if (Stopped || world.Tick(realmTime))
                            TickThreadBatch.WorldManager.RemoveWorld(world);
                        else
                        {
                            world.ProcessNetworking(realmTime);
                            world.TickLogic(realmTime);
                            world.PlayerUpdate(realmTime);
                        }
                    }
                    catch (Exception e) { SLogger.Instance.Fatal(e); continue; }
                }

                if (Stopped)
                    break;

                var logicTime = (int)(watch.ElapsedMilliseconds - realmTime.TotalElapsedMs);
                var sleepTime = Math.Max(0, 50 - logicTime); // 50 ms -> 20 tps - time to do the update

                Thread.Sleep(sleepTime);

                last = current;
            }
        }
    }
}
