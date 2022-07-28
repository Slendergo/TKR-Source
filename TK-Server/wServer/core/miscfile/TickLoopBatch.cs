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
            var tickData = new TickTime();
            var last = 0L;

            var spin = new SpinWait();

            while (true)
            {
                if (!CoreServerManager.Initialized)
                {
                    Thread.Sleep(200);
                    continue;
                }

                var current = tickData.TotalElapsedMs = watch.ElapsedMilliseconds;
                var delta = (int)(current - last);

                if (delta >= 200)
                {
                    tickData.TickCount++;
                    tickData.ElaspedMsDelta = delta;

                    var worlds = TickThreadBatch.Attached.ToArray();

                    foreach (var world in worlds)
                    {
                        if (world == null)
                        {
                            SLogger.Instance.Error("[world == null] THIS SHOULD NOT HAPPEN");
                            continue;
                        }

                        try
                        {
                            if(Stopped || world.Update(ref tickData))
                            {
                                _ = TickThreadBatch.WorldManager.RemoveWorld(world);
                            }

                            //if (Stopped || world.Tick(tickData))
                            //    TickThreadBatch.WorldManager.RemoveWorld(world);
                            //else
                            //{
                            //    world.ProcessNetworking(tickData);
                            //    world.TickLogic(tickData);
                            //    world.PlayerUpdate(tickData);
                            //}
                        }
                        catch (Exception e)
                        {
                            SLogger.Instance.Fatal(e);
                            continue;
                        }
                    }

                    last = current;
                }

                spin.SpinOnce();
            }
        }
    }
}
