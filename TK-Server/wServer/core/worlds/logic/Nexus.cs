using common.resources;
using wServer.networking;
using wServer.utils;

namespace wServer.core.worlds.logic
{
    internal class Nexus : World
    {
        public PortalMonitor PortalMonitor { get; private set; }

        public Nexus(int id, WorldResource resource) : base(id, resource) => IsDungeon = false;

        public override void Init()
        {

            base.Init();
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            PortalMonitor.Update(ref time);
            //SLogger.Instance.Warn($"[Nexus] {time.ElaspedMsDelta}");
            base.UpdateLogic(ref time);
        }
    }
}
