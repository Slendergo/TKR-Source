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

            var realmTime = new TickTime();
            var last = 0L;

            var mre = new ManualResetEvent(false);

            while (true)
            {
                if (!CoreServerManager.Initialized)
                {
                    Thread.Sleep(200);
                    continue;
                }

                var current = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;
                var delta = (int)(current - last);

                realmTime.TickCount++;
                realmTime.ElaspedMsDelta = delta;

                try
                {
                    if (Stopped || World.Update(ref realmTime))
                    {
                        TickThreadSingle.WorldManager.RemoveWorld(World);
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"World Tick: {e}");
                }

                var logicTime = (int)(watch.ElapsedMilliseconds - realmTime.TotalElapsedMs);

                var sleepTime = Math.Max(0, 200 - logicTime);

                mre.WaitOne(sleepTime);

                last = current;
            }

            //var watch = Stopwatch.StartNew();

            //var time = new TickTime();
            //var last = 0L;

            //var spin = new SpinWait();

            //while (true)
            //{
            //    if (!CoreServerManager.Initialized)
            //    {
            //        Thread.Sleep(200);
            //        continue;
            //    }

            //    var current = time.TotalElapsedMs = watch.ElapsedMilliseconds;
            //    var delta = (int)(current - last);

            //    if (delta >= 200)
            //    {
            //        time.TickCount++;
            //        time.ElaspedMsDelta = delta;

            //        if (Stopped || World.Update(ref time))
            //        {
            //            _ = TickThreadSingle.WorldManager.RemoveWorld(World);
            //            break;
            //        }

            //        last = current;
            //    }
            //}
        }
    }
}
