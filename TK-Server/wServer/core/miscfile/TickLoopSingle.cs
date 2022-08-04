using System;
using System.Diagnostics;
using System.Threading;
using wServer.core.worlds;
using wServer.core.worlds.logic;

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

            var sleep = 200; // 5 tps

            var lastMS = 0L;
            var mre = new ManualResetEvent(false);

            var realmTime = new TickTime();

            while (true)
            {
                if (sleep > 0)
                    _ = mre.WaitOne(sleep);

                var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                var delta = (int)Math.Max(currentMS - lastMS, 200);

                if(delta >= 200)
                {
                    realmTime.TickCount++;
                    realmTime.ElaspedMsDelta = delta;

                    try
                    {
                        if (Stopped || World.Update(ref realmTime))
                        {
                            _ = TickThreadSingle.WorldManager.RemoveWorld(World);
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"World Tick: {e}");
                    }
                    lastMS = currentMS;

                    Thread.Yield();
                }

                //if (sleep > 0)
                //    _ = mre.WaitOne(sleep);

                //var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                //var delta = (int)Math.Max(currentMS - lastMS, 200);

                //realmTime.TickCount++;
                //realmTime.ElaspedMsDelta = delta;

                //var logicTime = watch.ElapsedMilliseconds;

                //try
                //{
                //    if (Stopped || World.Update(ref realmTime))
                //    {
                //        _ = TickThreadSingle.WorldManager.RemoveWorld(World);
                //        break;
                //    }
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine($"World Tick: {e}");
                //}

                //realmTime.LogicTime = sleep = 200 - (int)(watch.ElapsedMilliseconds - logicTime);

                //lastMS = currentMS;
            }

            Stop();
            //var watch = Stopwatch.StartNew();

            //var realmTime = new TickTime();
            //var last = 0L;

            //while (true)
            //{
            //    if (!CoreServerManager.Initialized)
            //    {
            //        Thread.Sleep(200);
            //        continue;
            //    }

            //    var current = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;
            //    var delta = Math.Max(200, (int)(current - last));

            //    realmTime.TickCount++;
            //    realmTime.ElaspedMsDelta = delta;

            //    try
            //    {
            //        if (Stopped || World.Update(ref realmTime))
            //        {
            //            _ = TickThreadSingle.WorldManager.RemoveWorld(World);
            //            break;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"World Tick: {e}");
            //    }

            //    var logicTime = (int)(watch.ElapsedMilliseconds - realmTime.TotalElapsedMs);

            //    Console.WriteLine($"[DeltaTime]: {World.DisplayName} -> {realmTime.ElaspedMsDelta} | {realmTime.LogicTime}");

            //    realmTime.LogicTime = logicTime;

            //    var sleepTime = Math.Max(0, 200 - logicTime);

            //    if(sleepTime > 0)
            //        Thread.Sleep(sleepTime);

            //    last = current;
            //}

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
