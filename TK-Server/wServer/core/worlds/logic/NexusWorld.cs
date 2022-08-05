using common.resources;

namespace wServer.core.worlds.logic
{
    public sealed class NexusWorld : World
    {
        public PortalMonitor PortalMonitor { get; private set; }

        public NexusWorld(int id, WorldResource resource) : base(id, resource)
        {
        }

        public override void Init()
        {
            PortalMonitor = new PortalMonitor(GameServer, this);
            base.Init();
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }
    }
}
