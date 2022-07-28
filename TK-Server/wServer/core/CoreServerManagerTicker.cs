using System;
using System.Diagnostics;
using System.Threading;

namespace wServer.core
{
    public sealed class CoreServerManagerTicker
    {
        private CoreServerManager CoreServerManager;
        private TickData UpdateTime;

        public CoreServerManagerTicker(CoreServerManager realmManager, int tickRate)
        {
            CoreServerManager = realmManager;

            MsPerTick = 1000 / tickRate;
            UpdateTime = new TickData();
        }

        private int MsPerTick { get; }
        private bool Stopped { get; set; }

        public void Shutdown()
        {
            if (Stopped)
                return;

            Stopped = true;
        }

        public void Update()
        {
            var watch = Stopwatch.StartNew();
            var last = 0L;

            do
            {
                var current = UpdateTime.TotalElapsedMs = watch.ElapsedMilliseconds;
                var delta = (int)(current - last);

                if (Stopped)
                    break;

                UpdateTime.TickCount++;
                UpdateTime.ElaspedMsDelta = delta;

                CoreServerManager.ConnectionManager.Tick(UpdateTime.TotalElapsedMs);
                CoreServerManager.WorldManager.PortalMonitor.Tick();
                CoreServerManager.InterServerManager.Tick(UpdateTime.ElaspedMsDelta);

                var logicTime = (int)(watch.ElapsedMilliseconds - current);
                var sleepTime = Math.Max(0, MsPerTick - logicTime);

                Thread.Sleep(sleepTime);

                last = current;
            }
            while (true);
        }
    }
}
