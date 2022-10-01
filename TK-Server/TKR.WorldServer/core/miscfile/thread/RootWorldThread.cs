using TKR.Shared;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TKR.WorldServer.core;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.core.worlds.logic;

namespace TKR.WorldServer.core.miscfile.thread
{
    public sealed class RootWorldThread
    {
        public static volatile bool TickWithSleep;

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
                    if (!TickWithSleep)
                    {
                        //World.ProcessPlayerIO(ref realmTime);

                        var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                        var delta = (int)(currentMS - lastMS);
                        if (delta >= TICK_TIME_MS)
                        {
                            realmTime.TickCount++;
                            realmTime.ElapsedMsDelta = delta;

                            using (var t = new TimedProfiler($"[{World.IdName} {World.Id} -> {delta}"))
                            {
                                World.ProcessPlayerIO(ref realmTime);
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
                                    Console.WriteLine($"[{World.IdName} {World.Id}] Tick: {e.StackTrace}");
                                }
                                World.ProcessPlayerSendIO();
                            }

                            lastMS = currentMS; //+= delta; // TICK_TIME_MS;
                        }

                        //World.ProcessPlayerSendIO();

                        if (World.Players.Count == 0)
                            Thread.Sleep(TICK_TIME_MS);
                    }
                    else
                    {
                        if (sleep > 0)
                            _ = mre.WaitOne(sleep);

                        var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                        var delta = (int)Math.Max(currentMS - lastMS, TICK_TIME_MS);

                        realmTime.TickCount++;
                        realmTime.ElapsedMsDelta = delta;

                        var logicTime = watch.ElapsedMilliseconds;

                        try
                        {
                            World.ProcessPlayerIO(ref realmTime);
                            if (World.Update(ref realmTime))
                            {
                                Stopped = true;
                                break;
                            }
                            World.ProcessPlayerSendIO();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"[{World.IdName} {World.Id}] Tick: {e.StackTrace}");
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
