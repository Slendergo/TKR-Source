using wServer.core.worlds;

namespace wServer.core
{
    public abstract class TickThread
    {
        public WorldManager WorldManager;

        public TickThread(WorldManager worldManager) => WorldManager = worldManager;

        protected bool Stopped { get; set; }

        public abstract void Attach(World world);

        public abstract void Detatch(World world);

        public abstract void Stop();
    }
}
