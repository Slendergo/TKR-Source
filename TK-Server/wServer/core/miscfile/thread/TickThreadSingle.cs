using common;
using System;
using System.Diagnostics;
using System.Threading;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core
{
    public sealed class TickThreadSingle
    {
        public static bool TryNewSystem = false;

        private readonly WorldManager WorldManager;
        private World World;
        private readonly Thread Thread;
        private bool Stopped;

        public TickThreadSingle(WorldManager worldManager, World world)
        {
            WorldManager = worldManager;
            World = world;

            Thread = new Thread(Run);
            Thread.Name = $"{World.GetDisplayName()}_Logic";
            Thread.Start();
        }

        private void Run()
        {
            var watch = Stopwatch.StartNew();

            var sleep = 200; // 5 tps

            var lastMS = 0L;
            var mre = new ManualResetEvent(false);

            var realmTime = new TickTime();

            while (!Stopped)
            {
                if (TryNewSystem)
                {
                    var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                    var delta = (int)(currentMS - lastMS); //  (int)Math.Max(currentMS - lastMS, 200);
                    if (delta >= 200)
                    {
                        realmTime.TickCount++;
                        realmTime.ElaspedMsDelta = delta;

                        try
                        {
                            if (World.Update(ref realmTime))
                            {
                                _ = WorldManager.RemoveWorld(World);
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"World Tick: {e}");
                        }

                        lastMS = currentMS;
                    }
                }
                else
                {
                    if (sleep > 0)
                        _ = mre.WaitOne(sleep);

                    var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                    var delta = (int)Math.Max(currentMS - lastMS, 200);

                    realmTime.TickCount++;
                    realmTime.ElaspedMsDelta = delta;

                    var logicTime = watch.ElapsedMilliseconds;

                    try
                    {
                        if (World.Update(ref realmTime))
                        {
                            _ = WorldManager.RemoveWorld(World);
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"World Tick: {e}");
                    }

                    realmTime.LogicTime = sleep = 200 - (int)(watch.ElapsedMilliseconds - logicTime);

                    lastMS = currentMS;
                }
            }
        }

        public void Stop()
        {
            if (Stopped)
                return;
            Stopped = true;
            //Console.WriteLine("Before Join");
            //Thread.Join();
            //Console.WriteLine("After Join");
        }
    }
}
