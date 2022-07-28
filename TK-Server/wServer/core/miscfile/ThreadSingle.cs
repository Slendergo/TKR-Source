using System.Threading;
using wServer.core.worlds;

namespace wServer.core
{
    public sealed class ThreadSingle
    {
        public TickThreadSingle TickThreadSingle;
        public World World;
        private Thread Thread;
        private TickLoopSingle TickLoopSingle;

        public ThreadSingle(TickThreadSingle tickThreadSingle, World world)
        {
            TickThreadSingle = tickThreadSingle;
            World = world;

            TickLoopSingle = new TickLoopSingle(tickThreadSingle, world);

            Thread = new Thread(TickLoopSingle.Tick);
            Thread.Start();
        }

        private bool Stopped { get; set; }

        public void Stop()
        {
            if (Stopped)
                return;

            Stopped = true;

            TickLoopSingle.Stop();
            Thread.Join();
        }
    }
}
