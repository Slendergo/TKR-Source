using System.Collections.Generic;
using wServer.core.worlds;

namespace wServer.core
{
    public sealed class TickThreadSingle : TickThread
    {
        public Dictionary<World, ThreadSingle> Attached;

        public TickThreadSingle(WorldManager worldManager) : base(worldManager) => Attached = new Dictionary<World, ThreadSingle>();

        public override void Attach(World world)
        {
            var threadSingle = new ThreadSingle(this, world);

            Attached.Add(world, threadSingle);
        }

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

            var values = new List<ThreadSingle>(Attached.Values);

            foreach (var value in values)
                value.Stop();
        }
    }
}
