using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TKR.WorldServer.core.worlds
{
    public sealed class RootWorldThread
    {
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
                    World.ProcessPlayerIO(ref realmTime);

                    var currentMS = realmTime.TotalElapsedMs = watch.ElapsedMilliseconds;

                    var delta = (int)(currentMS - lastMS);
                    if (delta >= TICK_TIME_MS)
                    {
                        realmTime.TickCount++;
                        realmTime.ElapsedMsDelta = delta;

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

                        lastMS = currentMS; //+= delta; // TICK_TIME_MS;
                    }

                    if (World.Players.Count == 0)
                        Thread.Sleep(TICK_TIME_MS);
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
