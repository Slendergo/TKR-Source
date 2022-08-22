using common;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core
{
    public sealed class RootWorldThread
    {
        public static bool TryNewSystem = false;

        private readonly WorldManager WorldManager;
        private World World;
        private bool Stopped;

        public RootWorldThread(WorldManager worldManager, World world)
        {
            WorldManager = worldManager;
            World = world;

            Run();
        }

        private void Run()
        {
            Task.Factory.StartNew(() =>
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
                                Console.WriteLine($"World Tick: {e.StackTrace}");
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
                            Console.WriteLine($"World Tick: {e.StackTrace}");
                        }

                        realmTime.LogicTime = sleep = 200 - (int)(watch.ElapsedMilliseconds - logicTime);

                        lastMS = currentMS;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            if (Stopped)
                return;
            Stopped = true;
        }
    }
}
