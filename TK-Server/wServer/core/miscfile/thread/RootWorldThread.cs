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
        public static volatile bool DifferentTickThread;

        public const int TICK_TIME_MS = 200;

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

                var sleep = TICK_TIME_MS; // 5 tps

                var lastMS = 0L;
                var mre = new ManualResetEvent(false);

                var realmTime = new TickTime();

                while (!Stopped)
                {
                    if (DifferentTickThread)
                    {
                        var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                        var delta = (int)(currentMS - lastMS);

                        if (delta >= TICK_TIME_MS)
                        {
                            realmTime.TickCount++;
                            realmTime.ElaspedMsDelta = delta;

                            var logicTime = watch.ElapsedMilliseconds;

                            try
                            {
                                if (World.Update(ref realmTime))
                                {
                                    Stopped = true;
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"World Tick: {e.StackTrace}");
                            }

                            realmTime.LogicTime = sleep = TICK_TIME_MS - (int)(watch.ElapsedMilliseconds - logicTime);

                            lastMS = currentMS;
                        }
                    }
                    else
                    {
                        if (sleep > 0)
                            _ = mre.WaitOne(sleep);

                        var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                        var delta = (int)Math.Max(currentMS - lastMS, TICK_TIME_MS);

                        realmTime.TickCount++;
                        realmTime.ElaspedMsDelta = delta;

                        var logicTime = watch.ElapsedMilliseconds;

                        try
                        {
                            if (World.Update(ref realmTime))
                            {
                                Stopped = true;
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"World Tick: {e.StackTrace}");
                        }

                        realmTime.LogicTime = sleep = TICK_TIME_MS - (int)(watch.ElapsedMilliseconds - logicTime);

                        lastMS = currentMS;
                    }
                }

                WorldManager.RemoveWorld(World);

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
