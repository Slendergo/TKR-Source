using System.Collections.Generic;
using System.Threading;
using wServer.core.worlds;

namespace wServer.core
{
    public sealed class TickThreadBatch : TickThread
    {
        public List<World> Attached;
        private Thread Thread;
        private TickLoopBatch TickLoopBatch;

        public TickThreadBatch(WorldManager worldManager) : base(worldManager)
        {
            Attached = new List<World>();
            TickLoopBatch = new TickLoopBatch(this);

            Thread = new Thread(TickLoopBatch.Tick);
            Thread.Start();
        }

        public override void Attach(World world) => Attached.Add(world);

        public override void Detatch(World world)
        {
            Attached.Remove(world);
            world.Delete();
        }

        public override void Stop()
        {
            if (Stopped)
                return;

            Stopped = true;

            TickLoopBatch.Stop();
            Thread.Join();
        }
    }
}
